using Cool_data_processing_service.Service;

namespace Cool_data_processing_service.BackgroundService
{
    public class LogBackgroundService
    {
        private readonly LoggerService _logger;
        public LogBackgroundService(LoggerService logger)
        {
            _logger = logger;
        }
        public void SaveDayLog()
        { 
            // Schedule a recurring job to run at 11 pm every day
            var schedule = new Schedule(18, 46); 
            Task.Run(() => Save(schedule));
        }

        async Task Save(Schedule schedule)
        {
            while (true)
            {
                await Task.Delay(schedule.Delay);
                await _logger.Save();
            }
        }

    }
}
