using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Iot.Device.Graphics;
using Iot.Device.Media;

namespace DOOH.Adboard.Services
{
    public class CameraService
    {
        private readonly VideoConnectionSettings _settings;
        private VideoDevice _device;
        private FileStream _fileStream;
        private CancellationTokenSource _tokenSource;
        private bool _isCapturing;
        private string outputPath = "/home/admin/stream.h264";
        public CameraService()
        {
            // int busId, string outputPath, uint width, uint height, VideoPixelFormat pixelFormat
            _settings = new VideoConnectionSettings(0, (640, 480), VideoPixelFormat.H264);
            _device = VideoDevice.Create(_settings);
            _fileStream = File.Create(outputPath);
            _tokenSource = new CancellationTokenSource();
            _isCapturing = false;

            _device.NewImageBufferReady += NewImageBufferReadyEventHandler;
        }

        public void StartCapturing()
        {
            if (_isCapturing)
            {
                throw new InvalidOperationException("Video capturing is already in progress.");
            }

            _isCapturing = true;
            _device.StartCaptureContinuous();
            new Thread(() => _device.CaptureContinuous(_tokenSource.Token)).Start();
        }

        public void StopCapturing()
        {
            if (!_isCapturing)
            {
                throw new InvalidOperationException("Video capturing is not in progress.");
            }

            _tokenSource.Cancel();
            _device.StopCaptureContinuous();
            _isCapturing = false;
        }

        private async void NewImageBufferReadyEventHandler(object sender, NewImageBufferReadyEventArgs e)
        {
            try
            {
                await _fileStream.WriteAsync(e.ImageBuffer, 0, e.Length);
                Console.Write(".");
            }
            catch (ObjectDisposedException)
            {
                // Ignore this exception as it is thrown when the stream is stopped
            }
        }

        public void Dispose()
        {
            if (_isCapturing)
            {
                StopCapturing();
            }

            _fileStream?.Dispose();
            _device?.Dispose();
            _tokenSource?.Dispose();
        }

        public FileStream GetStream()
        {
            return _fileStream;
        }
    }
}
