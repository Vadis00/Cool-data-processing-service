using System.Configuration;
using System.IO;


namespace Cool_data_processing_service.Service
{
    public class FileWatcherService
    {
        private readonly FileSystemWatcher _fileSystemWatcher;
        private readonly FileProcessingService _fileProcessingService;
        private string folder { get; set; }
        public FileWatcherService(FileSystemWatcher fileSystemWatcher, FileProcessingService fileProcessingService)
        {
            folder = ConfigurationManager.AppSettings.Get("folder");

            if (!Directory.Exists(folder))
                throw new Exception($"The specified directory does not exist!\n " +
                    $"Directory: ${folder}");

            _fileSystemWatcher = fileSystemWatcher;
            _fileProcessingService = fileProcessingService;

        }

        public void ListenUpdate()
        {
            _fileSystemWatcher.Path = folder;
            _fileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            _fileSystemWatcher.Filter = "*.*";
            _fileSystemWatcher.EnableRaisingEvents = true;
            _fileSystemWatcher.Changed += OnChanged;

        }

        public void Stoplistening() => _fileSystemWatcher.Changed -= OnChanged;


        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _fileProcessingService.NewFile(e.FullPath);
        }

    }
}
