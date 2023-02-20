using Cool_data_processing_service.Enum;
using System.Configuration;
using System.Text.Json;

namespace Cool_data_processing_service.Service
{
    public class LoggerService
    {
        private Model.Log log;
        private DateTime currentDate;
        private readonly DataService _dataService;
        private readonly string _directoryPath;
        private readonly string logFileName;

        public LoggerService(DataService dataService)
        {
            log = new()
            {
                ParsedFiles = 0,
                ParsedLines = 0,
                FoundErrors = 0,
            };

            _dataService = dataService;
            _directoryPath = ConfigurationManager.AppSettings.Get("OutputFolder");
            logFileName = "meta.log";
        }

        /// <summary>
        /// Print the current statistics to the console
        /// </summary>
        public void Show()
        {
            Console.WriteLine(generateMetaFile());
        }

        public void Error(string filleWay, FileStatus status)
        {
            log.FoundErrors++;

            if (status == FileStatus.InvalidFile && !log.InvalidFiles.Contains(filleWay))
            {
                log.InvalidFiles.Add(filleWay);
            }
        }

        public void Done(FileStatus status)
        {
            switch (status)
            {
                case FileStatus.DoneLine:
                    log.ParsedLines++;
                    break;
                case FileStatus.DoneFile:
                    log.ParsedFiles++;
                    break;
            }
        }

        /// <summary>
        /// Will save the result in meta.log. Reset statistics for the day
        /// </summary>
        /// <returns></returns>
        public async Task Save()
        {
            currentDate = DateTime.Now;
            var path = $@"{_directoryPath}\{currentDate.ToString("dd.MM.yyyy")}";
            string meta = generateMetaFile();

            await _dataService.SaveAsync($@"{path}\{logFileName}", meta);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine("Save meta.log..");
            Show();
            Console.WriteLine();
            Console.ResetColor();
            Console.Write("Command: ");
 
            log = new();
        }

        /// <summary>
        /// Generate a path for the meta.log file
        /// </summary>
        /// <returns>The path to the meta.log file</returns>
        private string generateMetaFile()
        {
            var meta = $"parsed_files: {log.ParsedFiles}\n" +
                $"parsed_lines: {log.ParsedLines}\n" +
                $"found_errors: {log.FoundErrors}\n" +
                $"invalid_files: \n";

            foreach (var fileName in log.InvalidFiles)
                meta += $"  {fileName}\n";

            if(log.InvalidFiles.Count ==0)
                meta += $"not found!";

            return meta;
        }
    }
}
