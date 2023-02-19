using Cool_data_processing_service.Service;

namespace LoggerService
{
    public class Worker : BackgroundService, IHostedService, IDisposable
    {
        private readonly Cool_data_processing_service.Service.LoggerService _logger;

        public Worker(Cool_data_processing_service.Service.LoggerService logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {          
            while (!stoppingToken.IsCancellationRequested)
            {
                await _logger.Save();
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}