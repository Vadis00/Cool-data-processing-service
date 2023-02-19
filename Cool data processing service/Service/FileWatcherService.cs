using Cool_data_processing_service.Const;
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
            folder = ConfigurationManager.AppSettings.Get("InputFolder");

            _fileSystemWatcher = fileSystemWatcher;
            _fileProcessingService = fileProcessingService;
        }

        /// <summary>
        /// Listen to folder updates for new files
        /// </summary>
        public void ListenUpdate()
        {
            _fileSystemWatcher.Path = folder;
            _fileSystemWatcher.Filter = "*.*";
            _fileSystemWatcher.EnableRaisingEvents = true;
            _fileSystemWatcher.Created += OnChanged;
        }

        /// <summary>
        /// Stop listening for folder updates for new files
        /// </summary>
        public void Stoplistening() => _fileSystemWatcher.Changed -= OnChanged;

        /// <summary>
        /// Called when a new file is added to the folder. Check the file type and process it.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // get the file's extension 
            string strFileExt = Path.GetExtension(e.FullPath);

            switch (strFileExt)
            {
                case ".txt":
                    _fileProcessingService.NewFileAsync(e.FullPath, FileType.Txt);
                    break;
                case ".csv":
                    _fileProcessingService.NewFileAsync(e.FullPath, FileType.Csv);
                    break;
            }

        }

    }
}
