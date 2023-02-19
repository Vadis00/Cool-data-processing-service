using Cool_data_processing_service.BackgroundService;
using Cool_data_processing_service.Service;
using System.Configuration;

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
            _logBackgroundService.Start();
        }

        public void Stop()
        {
            _fileWatcherService.Stoplistening();
            _logBackgroundService.Stop();
        }

        public bool ConfigurationСheck(out string statusMsg)
        {
            var status = true;
            statusMsg = "Error!\n";

            var outputDirectoryPath = ConfigurationManager.AppSettings.Get("OutputFolder");
            var inputDirectoryPath = ConfigurationManager.AppSettings.Get("InputFolder");

            if (!Directory.Exists(outputDirectoryPath))
            {
                statusMsg += $"The specified directory does not exist!\n " +
                    $"Output Directory: {outputDirectoryPath}\n";
                status = false;
            }

            if (!Directory.Exists(inputDirectoryPath))
            {
                statusMsg += $"The specified directory does not exist!\n " +
                    $"Input Directory: {inputDirectoryPath}\n";
                status = false;

            }

            return status;
        }
    }
}
