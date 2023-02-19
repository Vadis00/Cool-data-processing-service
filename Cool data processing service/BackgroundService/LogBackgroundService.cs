using Cool_data_processing_service.Service;

namespace Cool_data_processing_service.BackgroundService
{
    public class LogBackgroundService
    {
        private readonly LoggerService _logger;
        private CancellationTokenSource source;
        private CancellationToken token;
        public LogBackgroundService(LoggerService logger)
        {
            source = new CancellationTokenSource();
            _logger = logger;
        }
        public void Start()
        {
            token = source.Token;
            // Schedule a recurring job to run at 11 pm every day
            var schedule = new Schedule(19, 42);
            Task.Run(() => SaveDayLog(schedule), token);
        }

        public void Stop()
        {
            _logger.Show();

            if (source != null)
            {
                source.Cancel();
            }
        }

        async Task SaveDayLog(Schedule schedule)
        {
            while (true)
            {
                await Task.Delay(schedule.Delay);
                await _logger.Save();
            }
        }

    }
}
