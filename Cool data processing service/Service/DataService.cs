using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Cool_data_processing_service.Model;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace Cool_data_processing_service.Service
{
    public class DataService
    {
        private readonly string directoryPath;
        private readonly DateTime currentDate;
        public DataService()
        {
            currentDate = DateTime.Now;

            directoryPath = ConfigurationManager.AppSettings.Get("OutputFolder");

            if (!Directory.Exists(directoryPath))
                throw new Exception($"The specified directory does not exist!\n " +
                    $"Directory: ${directoryPath}");


        }

        public async Task<Collection<string>> ReadAsync(string path)
        {
            Collection<string> lines = new();
            string line = String.Empty;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        while ((line = await sr.ReadLineAsync()) != null)
                        {
                            lines.Add(line);
                        }
                    }
                    break;
                }
                catch (IOException)
                {
                    await Task.Delay(700);
                }
            }
            return lines;
        }

        public async Task SaveAsync(string json)
        {
            int prefix = Directory.GetFiles(directoryPath).Length;
            string fillePath = GenerateFillePath(prefix);

            if (!File.Exists(fillePath))
            {
                using (var stream = File.Create(fillePath))
                {
                    using (TextWriter tw = new StreamWriter(stream))
                    {
                        await tw.WriteAsync(json);
                    }
                }
            }
        }

        private string GenerateFillePath(int prefix)
        {
            return $@"{directoryPath}\{currentDate.ToString("dd.MM.yyyy")}-{prefix}.json";
        }

    }
}
