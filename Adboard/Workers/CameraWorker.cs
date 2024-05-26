using DOOH.Adboard.Services;
using Iot.Device.Media;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DOOH.Adboard.Workers
{
    public class CameraWorker : BackgroundService
    {
        private readonly ILogger<CameraWorker> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly DOOHDBService _doohdbService;

        private VideoDevice _device;

        public CameraWorker(
            ILogger<CameraWorker> logger,
            HttpClient httpClient,
            IConfiguration configuration,
            DOOHDBService doohdbService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _doohdbService = doohdbService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var settings = new VideoConnectionSettings(busId: 0, captureSize: (640, 480), pixelFormat: VideoPixelFormat.H264);

            try
            {
                _logger.LogInformation("Initializing the camera device...");
                _device = VideoDevice.Create(settings);
                _device.ImageBufferPoolingEnabled = true;
                _device.NewImageBufferReady += NewImageBufferEvent;
                _device.StartCaptureContinuous();
                _device.CaptureContinuous(cancellationToken);
                _logger.LogInformation("Camera capture started.");

                while (!cancellationToken.IsCancellationRequested)
                {
                    Thread.SpinWait(1);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during camera initialization or capture.");
            }
            finally
            {
                if (_device != null)
                {
                    _logger.LogInformation("Stopping and disposing the camera device...");
                    try
                    {
                        _device.StopCaptureContinuous();
                        _device.NewImageBufferReady -= NewImageBufferEvent;
                        _device.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error disposing the camera device.");
                    }
                    _logger.LogInformation("Camera device disposed.");
                }
            }
        }

        private void NewImageBufferEvent(object sender, NewImageBufferReadyEventArgs e)
        {
            try
            {
                var image = e.ImageBuffer;
                _logger.LogInformation($"New image buffer received with size: {image.Length} bytes.");
                // Example processing or saving image to the database
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing the image buffer.");
            }
        }


        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CameraWorker is stopping.");

            if (_device != null)
            {
                _device.NewImageBufferReady -= NewImageBufferEvent;
                _device.Dispose();
                _logger.LogInformation("Camera device disposed.");
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
