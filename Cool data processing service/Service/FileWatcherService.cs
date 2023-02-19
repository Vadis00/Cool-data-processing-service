﻿using Cool_data_processing_service.Const;
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

        public void ListenUpdate()
        {
            _fileSystemWatcher.Path = folder;
            _fileSystemWatcher.Filter = "*.*";
            _fileSystemWatcher.EnableRaisingEvents = true;
            _fileSystemWatcher.Created += OnChanged;


        }

        public void Stoplistening() => _fileSystemWatcher.Changed -= OnChanged;

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
