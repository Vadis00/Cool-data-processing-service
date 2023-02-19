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
            log = new();
            _dataService = dataService;
            _directoryPath = ConfigurationManager.AppSettings.Get("OutputFolder");
            logFileName = "meta.log";
        }

        public void Show()
        {
            Console.WriteLine(generateMetaFile());
        }

        public void Error(string filleWay)
        {
            log.FoundErrors++;

            if (!log.InvalidFiles.Contains(filleWay))
                log.InvalidFiles.Add(filleWay);
        }

        public void Done(string status)
        {
            switch (status)
            {
                case "line":
                    log.ParsedLines++;
                    break;
                case "fille":
                    log.ParsedFiles++;
                    break;
            }
        }

        public async Task Save()
        {
            currentDate = DateTime.Now;
            var path = $@"{_directoryPath}\{currentDate.ToString("dd.MM.yyyy")}";
            string meta = generateMetaFile();

            await _dataService.SaveAsync($@"{path}\{logFileName}", meta);
            log = new();
        }

        private string generateMetaFile()
        {
            var meta = $"parsed_files: {log.ParsedFiles}\n" +
                $"parsed_lines: {log.ParsedLines}\n" +
                $"found_errors: {log.FoundErrors}\n" +
                $"invalid_files: \n";

            foreach (var fileName in log.InvalidFiles)
                meta += $"  {fileName}\n";

            return meta;
        }
    }
}
