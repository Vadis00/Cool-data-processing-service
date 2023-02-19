using System.Configuration;
using System.Text.Json;

namespace Cool_data_processing_service.Service
{
    public class LoggerService
    {
        private Model.Log log;
        private readonly DataService _dataService;
        private readonly string _directoryPath;

        public LoggerService(DataService dataService)
        {
            log = new();
            _dataService = dataService;
            _directoryPath = ConfigurationManager.AppSettings.Get("OutputFolder");
        }

        public void Error(string filleWay)
        {
            log.FoundErrors++;
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
            string json = JsonSerializer.Serialize(log);
            await _dataService.SaveAsync($@"{_directoryPath}\meta.log", json);
            log = new();
        }
    }
}
