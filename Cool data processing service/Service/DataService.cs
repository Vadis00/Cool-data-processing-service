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

        public async Task SaveAsync(string path, string data)
        {
            if (!File.Exists(path))
            {
                using (var stream = File.Create(path))
                {
                    using (TextWriter tw = new StreamWriter(stream))
                    {
                        await tw.WriteAsync(data);
                    }
                }
            }
        }
    }
}
