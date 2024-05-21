using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DOOH.Adboard.Services;
using DOOH.Server.Models.DOOHDB;
using LibVLCSharp.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DOOH.Adboard.Workers
{
    public class PlaybackWorker : BackgroundService
    {
        private readonly ILogger<PlaybackWorker> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly DOOHDBService _doohdbService;
        private readonly int _adboardId;
        private readonly AdService _adService;

        public PlaybackWorker(
            ILogger<PlaybackWorker> logger,
            HttpClient httpClient,
            IConfiguration configuration,
            DOOHDBService doohdbService,
            AdService adService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _adboardId = _configuration.GetValue<int?>("Service:AdboardId") ?? 1;
            _doohdbService = doohdbService;
            _adService = adService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var adboard = await _doohdbService.GetAdboardByAdboardId(adboardId: _adboardId);
                    var wifi = await _doohdbService.GetAdboardWifiByAdboardId(adboardId: _adboardId);

                    if (wifi != null && wifi.HasConnected != true)
                    {
                        var ssid = wifi.SSID;
                        var password = wifi.Password;
                        _logger.LogInformation($"New Wifi Credentials: {ssid}/{password}");

                        // Configure wifi connection - code needed here

                        wifi.HasConnected = true;
                        wifi.ConnectedAt = DateTime.Now;
                        await _doohdbService.UpdateAdboardWifiByAdboardId(_adboardId, wifi);
                    }
                    if (_adService.Advertisements.Count == 0)
                    {
                        await _adService.Sync();
                    }
                    await PlayAdvertisementsAsync(_adService.Advertisements, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing advertisements.");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken); // Delay for 10 seconds
            }
        }

        private async Task PlayAdvertisementsAsync(IEnumerable<Advertisement> advertisements, CancellationToken cancellationToken)
        {
            using (var libvlc = new LibVLC(enableDebugLogs: false,
                options: new string[]{
                    "--quiet"
                }))
            using (var player = new MediaPlayer(libvlc)
            {
                EnableHardwareDecoding = true,
                NetworkCaching = 5000,
                Scale = 0
                
                
            })
            {

                player.EnableHardwareDecoding = true;
                var _tempAdvertisements = new List<Advertisement>(advertisements);
                foreach (var advertisement in _tempAdvertisements)
                {
                    try
                    {
                        var mediaUri = new Uri($"https://cdn.hallads.com/{advertisement.AttachmentKey}");
                        string[] option = new string[] {
                             //@":dshow-vdev=",
                             @":dshow-adev=none",
                             @":dshow-size=1080x1920",
                             @":dshow-aspect-ratio=9\:16",
                             @":live-caching=0",
                             @":clock-synchro=0",
                             //@":sout=#duplicate{dst=file{dst=C:\Users\matthias.mueller\Desktop\test.mp4},dst=display}",
                             //@":sout-all",
                             //@":sout-keep",
                             @"--video-filter","rotate{angle=270}",
                        };
                        var media = new Media(libvlc, mediaUri, option);
                        if (player.Play(media))
                        {
                            var playbackTcs = new TaskCompletionSource<bool>();
                            media.StateChanged += (_, e) =>
                            {
                                if (e.State == VLCState.Stopped || e.State == VLCState.Error || e.State == VLCState.Paused)
                                {
                                    Task.Run(() => playbackTcs.TrySetResult(true), cancellationToken);
                                }
                            };
                            player.PositionChanged += (_, e) =>
                            {
                                if (e.Position >= 0.98f)
                                {
                                    Task.Run(() => playbackTcs.TrySetResult(true), cancellationToken);
                                }
                            };
                            player.TimeChanged += (_, e) =>
                            {
                                if (((advertisement.Duration * 1000) - e.Time) <= 500)
                                {
                                    Task.Run(() => playbackTcs.TrySetResult(true), cancellationToken);
                                }

                            };
                            if (_tempAdvertisements.IndexOf(advertisement) == 1)
                            {
                                await Task.Run(() => _adService.Sync(), cancellationToken);
                            }
                            await playbackTcs.Task;
                            await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
                        break;
                    }
                    catch (Exception ex)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
                        continue;
                    }
                }
            }
        }
    }
}
