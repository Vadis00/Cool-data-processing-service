using System.Collections.ObjectModel;

namespace Cool_data_processing_service.Model
{
    public class Log
    {
        public Log()
        {
            InvalidFiles = new Collection<string>();
        }

        public int ParsedFiles { get; set; }
        public int ParsedLines { get; set; }
        public int FoundErrors { get; set; }

        public ICollection<string> InvalidFiles;
    }
}
