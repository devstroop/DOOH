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
        private readonly InterloopService _interloopService;
        private readonly CameraService _cameraService;

        public PlaybackWorker(
            ILogger<PlaybackWorker> logger,
            HttpClient httpClient,
            IConfiguration configuration,
            DOOHDBService doohdbService,
            AdService adService,
            InterloopService interloopService,
            CameraService cameraService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
            _adboardId = _configuration.GetValue<int?>("Service:AdboardId") ?? 1;
            _doohdbService = doohdbService;
            _adService = adService;
            _interloopService = interloopService;
            _cameraService = cameraService;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await ConfigureWifiAsync(cancellationToken);
            await SyncAdvertisementsAsync(cancellationToken);

            try
            {
                using (var libvlc = new LibVLC(enableDebugLogs: false, options: new string[] { "--quiet", "--fullscreen" }))
                using (var player = new MediaPlayer(libvlc)
                {
                    EnableHardwareDecoding = false,
                    NetworkCaching = 1000,
                    Scale = 0
                })
                {
                    await RunPlaybackLoopAsync(libvlc, player, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing playback.");
            }
        }

        private async Task ConfigureWifiAsync(CancellationToken cancellationToken)
        {
            try
            {
                var wifi = await _doohdbService.GetAdboardWifiByAdboardId(adboardId: _adboardId);
                if (wifi != null && wifi.HasConnected != true)
                {
                    var ssid = wifi.SSID;
                    var password = wifi.Password;
                    _logger.LogInformation("Configuring wifi connection...");
                    await _interloopService.SetWifiCredentials(ssid, password);

                    wifi.HasConnected = true;
                    wifi.ConnectedAt = DateTime.Now;
                    await _doohdbService.UpdateAdboardWifiByAdboardId(_adboardId, wifi);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while configuring wifi connection.");
            }
        }

        private async Task RunPlaybackLoopAsync(LibVLC libvlc, MediaPlayer player, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await PlayAdvertisementsAsync(libvlc, player, _adService.Advertisements, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Playback loop canceled.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while playing advertisements.");
                }
            }
        }

        private async Task SyncAdvertisementsAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _adService.Sync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while syncing advertisements.");
            }
        }

        private async Task PlayAdvertisementsAsync(LibVLC libvlc, MediaPlayer player, IEnumerable<Advertisement> advertisements, CancellationToken cancellationToken)
        {
            bool isSynced = false;
            int currentAdvertisementIndex = 0;
            int totalAdvertisements = advertisements.Count();

            foreach (var advertisement in advertisements)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var mediaUri = new Uri($"https://cdn.hallads.com/{advertisement.AttachmentKey}");
                    var media = new Media(libvlc, mediaUri);

                    if (await Task.Run(() => player.Play(media), cancellationToken))
                    {
                        _cameraService.StartCapturing();
                        var playbackTcs = new TaskCompletionSource<bool>();
                        cancellationToken.Register(() => playbackTcs.TrySetCanceled());

                        media.StateChanged += (_, e) =>
                        {
                            if (e.State == VLCState.Stopped || e.State == VLCState.Error || e.State == VLCState.Paused)
                            {
                                playbackTcs.TrySetResult(true);
                            }
                        };
                        player.PositionChanged += (_, e) =>
                        {
                            if (e.Position >= 0.98f)
                            {
                                playbackTcs.TrySetResult(true);
                            }
                        };
                        player.TimeChanged += (_, e) =>
                        {
                            if (((advertisement.Duration * 1000) - e.Time) <= 500)
                            {
                                playbackTcs.TrySetResult(true);
                            }
                        };

                        // Capture the video stream
                        //_cameraService.StartCapturing();

                        if (!isSynced && (currentAdvertisementIndex > 0 || (currentAdvertisementIndex == 0 && totalAdvertisements == 1)))
                        {
                            try
                            {
                                await _adService.Sync(cancellationToken);
                                isSynced = true;
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "An error occurred during sync operation.");
                            }
                        }

                        await playbackTcs.Task;
                        _cameraService.StopCapturing();
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Advertisement playback canceled at index {Index}", currentAdvertisementIndex);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error playing advertisement {Index}/{Total}", currentAdvertisementIndex + 1, totalAdvertisements);
                }
                finally
                {
                    currentAdvertisementIndex++;
                }
            }
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;
//using DOOH.Adboard.Services;
//using DOOH.Server.Models.DOOHDB;
//using LibVLCSharp.Shared;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;

//namespace DOOH.Adboard.Workers
//{
//    public class PlaybackWorker : BackgroundService
//    {
//        private readonly ILogger<PlaybackWorker> _logger;
//        private readonly HttpClient _httpClient;
//        private readonly IConfiguration _configuration;
//        private readonly DOOHDBService _doohdbService;
//        private readonly int _adboardId;
//        private readonly AdService _adService;
//        private readonly InterloopService _interloopService;

//        public PlaybackWorker(
//            ILogger<PlaybackWorker> logger,
//            HttpClient httpClient,
//            IConfiguration configuration,
//            DOOHDBService doohdbService,
//            AdService adService,
//            InterloopService interloopService)
//        {
//            _logger = logger;
//            _httpClient = httpClient;
//            _configuration = configuration;
//            _adboardId = _configuration.GetValue<int?>("Service:AdboardId") ?? 1;
//            _doohdbService = doohdbService;
//            _adService = adService;
//            _interloopService = interloopService;
//        }

//        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
//        {

//            await ConfigureWifiAsync(cancellationToken);
//            await SyncAdvertisementsAsync(cancellationToken);

//            try
//            {
//                using (var libvlc = new LibVLC(enableDebugLogs: false,
//                    options: new string[]{
//                            "--quiet",
//                            "--fullscreen"
//                    }))
//                using (var player = new MediaPlayer(libvlc)
//                {
//                    EnableHardwareDecoding = true,
//                    NetworkCaching = 10000,
//                    Scale = 0
//                })
//                {
//                    await RunPlaybackLoopAsync(libvlc, player, cancellationToken);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while initializing playback.");
//            }
//        }

//        private async Task ConfigureWifiAsync(CancellationToken cancellationToken)
//        {
//            try
//            {
//                var wifi = await _doohdbService.GetAdboardWifiByAdboardId(adboardId: _adboardId);
//                if (wifi != null && wifi.HasConnected != true)
//                {
//                    var ssid = wifi.SSID;
//                    var password = wifi.Password;
//                    _logger.LogInformation("Configuring wifi connection...");
//                    await _interloopService.SetWifiCredentials(ssid, password);

//                    wifi.HasConnected = true;
//                    wifi.ConnectedAt = DateTime.Now;
//                    await _doohdbService.UpdateAdboardWifiByAdboardId(_adboardId, wifi);
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while configuring wifi connection.");
//            }
//        }

//        private async Task RunPlaybackLoopAsync(LibVLC libvlc, MediaPlayer player, CancellationToken cancellationToken)
//        {
//            while (!cancellationToken.IsCancellationRequested)
//            {
//                try
//                {
//                    await PlayAdvertisementsAsync(libvlc, player, _adService.Advertisements, cancellationToken);
//                }
//                catch (OperationCanceledException)
//                {
//                    break;
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "An error occurred while playing advertisements.");
//                }
//            }
//        }
//        private async Task SyncAdvertisementsAsync(CancellationToken cancellationToken)
//        {
//            try
//            {
//                await _adService.Sync(cancellationToken);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while syncing advertisements.");
//            }
//        }

//        private async Task PlayAdvertisementsAsync(LibVLC libvlc, MediaPlayer player, IEnumerable<Advertisement> advertisements, CancellationToken cancellationToken)
//        {
//            bool isSynced = false;
//            int currentAdvertisementIndex = 0;
//            int totalAdvertisements = advertisements.Count();
//            foreach (var advertisement in advertisements)
//            {
//                try
//                {
//                    var mediaUri = new Uri($"https://cdn.hallads.com/{advertisement.AttachmentKey}");
//                    string[] option = new string[]
//                    {
//                        //"--video-filter=transform",
//                        //"--transform-type=270",
//                    };
//                    var media = new Media(libvlc, mediaUri, option);
//                    if (await Task.Run(() => player.Play(media), cancellationToken))
//                    {
//                        var playbackTcs = new TaskCompletionSource<bool>();
//                        cancellationToken.Register(() => playbackTcs.TrySetCanceled());
//                        media.StateChanged += (_, e) =>
//                        {
//                            if (e.State == VLCState.Stopped || e.State == VLCState.Error || e.State == VLCState.Paused)
//                            {
//                                Task.Run(() => playbackTcs.TrySetResult(true), cancellationToken);
//                            }
//                        };
//                        player.PositionChanged += (_, e) =>
//                        {
//                            if (e.Position >= 0.98f)
//                            {
//                                Task.Run(() => playbackTcs.TrySetResult(true), cancellationToken);
//                            }
//                        };
//                        player.TimeChanged += (_, e) =>
//                        {
//                            if (((advertisement.Duration * 1000) - e.Time) <= 500)
//                            {
//                                Task.Run(() => playbackTcs.TrySetResult(true), cancellationToken);
//                            }
//                        };

//                        if (!isSynced && currentAdvertisementIndex > 0 || (currentAdvertisementIndex == 0 && totalAdvertisements == 1))
//                        {
//                            try
//                            {
//                                await _adService.Sync(cancellationToken);
//                                isSynced = true;
//                            }
//                            catch (Exception ex)
//                            {
//                                _logger.LogError(ex, "An error occurred during sync operation.");
//                                await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
//                            }
//                        }

//                        await playbackTcs.Task;
//                    }
//                }
//                catch (OperationCanceledException)
//                {
//                    await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
//                    break;
//                }
//                catch (Exception ex)
//                {
//                    await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
//                    continue;
//                }
//                finally
//                {
//                    currentAdvertisementIndex++;
//                }
//            }
//        }
//    }
//}
