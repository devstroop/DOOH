using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SkiaSharp;
using NcnnDotNet;
using UltraFaceDotNet;
using Iot.Device.Media;

namespace DOOH.Adboard.Workers
{
    public class AnalyticWorker : BackgroundService
    {
        private readonly ILogger<AnalyticWorker> _logger;
        private readonly UltraFace _ultraFace;
        private VideoDevice _device;
        private List<SKBitmap> _detectedFaces;

        public AnalyticWorker(ILogger<AnalyticWorker> logger, string binPath, string paramPath)
        {
            _logger = logger;
            _detectedFaces = new List<SKBitmap>();

            // Configure UltraFace
            var param = new UltraFaceParameter
            {
                BinFilePath = binPath,
                ParamFilePath = paramPath,
                InputWidth = 320,
                InputLength = 240,
                NumThread = 1,
                ScoreThreshold = 0.7f
            };

            _ultraFace = UltraFace.Create(param);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var bytes = _device.Capture();
                    if (bytes == null)
                    {
                        await Task.Delay(1000);
                        continue;
                    }

                    using (var ms = new MemoryStream(bytes))
                    {
                        using (var skImage = SKBitmap.Decode(ms))
                        {
                            // Convert SKBitmap to Mat
                            using (var imageMat = new Mat(skImage.Width, skImage.Height, 3))
                            {
                                // Lock the Mat for writing


                                // Detect faces
                                var faceInfos = _ultraFace.Detect(imageMat);

                                // Draw rectangles around detected faces
                                foreach (var face in faceInfos)
                                {
                                    using (var canvas = new SKCanvas(skImage))
                                    using (var paint = new SKPaint())
                                    {
                                        paint.Style = SKPaintStyle.Stroke;
                                        paint.Color = SKColors.Red;
                                        paint.StrokeWidth = 2;

                                        var rect = new SKRect(face.X1, face.Y1, face.X2, face.Y2);
                                        canvas.DrawRect(rect, paint);
                                    }
                                }

                                // Add the bitmap with drawn rectangles to the list
                                _detectedFaces.Add(skImage);

                                // Log the number of detected faces
                                _logger.LogInformation($"Detected {faceInfos.Count()} faces.");
                                _logger.LogInformation($"Total {_detectedFaces.Count} faces.");

                                // Adjust delay as needed
                                await Task.Delay(10);
                            }
                        }
                    }
                }
            }, stoppingToken);
        }

        public List<SKBitmap> GetDetectedFaces()
        {
            return _detectedFaces;
        }

        public override void Dispose()
        {
            _ultraFace.Dispose();
            base.Dispose();
        }
    }
}
