namespace WelcometotheSigma.Services;

public class VipDisplayBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IVipDisplayStateService _displayState;
    private readonly ILogger<VipDisplayBackgroundService> _logger;

    public VipDisplayBackgroundService(
        IServiceScopeFactory scopeFactory,
        IVipDisplayStateService displayState,
        ILogger<VipDisplayBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _displayState = displayState;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope   = _scopeFactory.CreateScope();
                var svc           = scope.ServiceProvider.GetRequiredService<IVipScheduleService>();
                _displayState.Current = await svc.GetCurrentActiveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "VipDisplayBackgroundService error");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
