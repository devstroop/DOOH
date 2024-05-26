using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Iot.Device.Media;

namespace DOOH.Adboard.Services
{
    public class CameraService : IDisposable
    {
        private readonly VideoConnectionSettings _settings;
        private VideoDevice _device;
        private Channel<byte[]> _channel;
        private CancellationTokenSource _tokenSource;
        private bool _isCapturing;

        public CameraService()
        {
            // Initialize video settings: busId, resolution (width, height), pixel format
            _settings = new VideoConnectionSettings(0, (640, 480), VideoPixelFormat.H264);
            _device = VideoDevice.Create(_settings);
            _channel = Channel.CreateUnbounded<byte[]>();
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
                // Write the image buffer to the channel
                await _channel.Writer.WriteAsync(e.ImageBuffer);
                Console.Write(".");
            }
            catch (ObjectDisposedException)
            {
                // Ignore this exception as it is thrown when the stream is stopped
            }
        }

        public ChannelReader<byte[]> GetChannelReader()
        {
            return _channel.Reader;
        }

        public void Dispose()
        {
            if (_isCapturing)
            {
                StopCapturing();
            }

            _device?.Dispose();
            _tokenSource?.Dispose();
            _channel.Writer.TryComplete();
        }
    }
}
