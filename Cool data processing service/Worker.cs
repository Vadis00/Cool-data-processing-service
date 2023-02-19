using Cool_data_processing_service.BackgroundService;
using Cool_data_processing_service.Service;


namespace Cool_data_processing_service
{
    public class Worker
    {
        private readonly LogBackgroundService _logBackgroundService;
        private readonly FileWatcherService _fileWatcherService;

        public Worker(
            FileWatcherService fileWatcherService, 
            LogBackgroundService logBackgroundService)
        {
            _logBackgroundService = logBackgroundService;
            _fileWatcherService = fileWatcherService;
        }

        public void Start()
        {
            _fileWatcherService.ListenUpdate();
            _logBackgroundService.SaveDayLog();
        }

        public void Stop()
        {
            _fileWatcherService.Stoplistening();
        }

    }
}
