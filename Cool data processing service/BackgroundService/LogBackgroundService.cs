using Cool_data_processing_service.Service;
using System.Configuration;

namespace Cool_data_processing_service.BackgroundService
{
    public class LogBackgroundService
    {
        private readonly LoggerService _logger;
        private CancellationTokenSource source;
        private CancellationToken token;
        private int saveLogInHours;
        private int saveLogInMinutes;

        /// <summary>
        /// Service for saving a meta file by timer. 
        /// Works in the background.
        /// </summary>
        /// <param name="logger"></param>
        public LogBackgroundService(LoggerService logger)
        {
            source = new CancellationTokenSource();
            _logger = logger;
            saveLogInHours = Int32.Parse(ConfigurationManager.AppSettings.Get("SaveLogInHours"));
            saveLogInMinutes = Int32.Parse(ConfigurationManager.AppSettings.Get("SaveLogInMinutes"));
        }
        public void Start()
        {
            token = source.Token;
            // Schedule a recurring job to run at 11 pm every day
            var schedule = new Schedule(saveLogInHours, saveLogInMinutes);
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
                await Task.Delay(10000);
                schedule = new Schedule(saveLogInHours, saveLogInMinutes);
            }
        }

    }
}
