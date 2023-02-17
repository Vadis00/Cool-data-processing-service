using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool_data_processing_service.Service
{
    public class FileWatcherService
    {
        private readonly FileSystemWatcher fileSystemWatcher;
        public FileWatcherService()
        {
         //   fileSystemWatcher = new();
        }

        public void ListenUpdate() => fileSystemWatcher.Changed += OnChanged;
        
        public void Stoplistening() => fileSystemWatcher.Changed -= OnChanged;


        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("ff");
        }

    }
}
