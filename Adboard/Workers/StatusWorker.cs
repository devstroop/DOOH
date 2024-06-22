using DOOH.Server.Hubs;
using DOOH.Server.Models.DOOHDB;

namespace DOOH.Adboard.Workers;
using Microsoft.AspNetCore.SignalR;

public class StatusWorker : BackgroundService
{
    private readonly ILogger<StatusWorker> _logger;
    private int _adboardId;
    private int _statusDelay;
    private readonly DOOHDBService _doohDbService;
    
    

    public  StatusWorker(ILogger<StatusWorker> logger, IConfiguration configuration, DOOHDBService doohDbService)
    {
        _logger = logger;
        _adboardId = configuration.GetValue<int?>("Service:AdboardId") ?? 1;
        _statusDelay = configuration.GetValue<int?>("Service:StatusDelay") ?? 60000;
        _doohDbService = doohDbService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            AdboardStatus? status = null;
            try
            {
                status = await _doohDbService.GetAdboardStatusByAdboardId(adboardId:_adboardId);
            }
            finally
            {
                if (status != null)
                {
                    status.Connected = true;
                    status.ConnectedAt = DateTime.Now;
                    status.Delay = _statusDelay;
                    await _doohDbService.UpdateAdboardStatus(adboardId:_adboardId, status);
                }
                else
                {
                    await _doohDbService.CreateAdboardStatus(new AdboardStatus
                    {
                        AdboardId = _adboardId,
                        Connected = true,
                        ConnectedAt = DateTime.Now,
                        Delay = _statusDelay
                    });
                }
            }

            await Task.Delay(_statusDelay, stoppingToken);
        }
    }
}