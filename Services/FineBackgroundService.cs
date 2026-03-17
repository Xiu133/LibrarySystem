using Library.Services.Interfaces;

namespace Library.Services
{
    public class FineBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FineBackgroundService> _logger;

        public FineBackgroundService(IServiceScopeFactory scopeFactory, ILogger<FineBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var delay = DateTime.Now.Date.AddDays(1) - DateTime.Now;
                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested) break;

                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var fineService = scope.ServiceProvider.GetRequiredService<IFineService>();
                    await fineService.ScanAndCreateFinesAsync();
                    _logger.LogInformation("逾期罰款自動掃描完成：{Time}", DateTime.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "逾期罰款自動掃描發生錯誤");
                }
            }
        }
    }
}
