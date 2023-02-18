using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool_data_processing_service.Model
{
    public class Log
    {
        public Log()
        {
            //  InvalidFiles = new();

        }
        public int ParsedFiles { get; set; }
        public int ParsedLines { get; set; }
        public int FoundErrors { get; set; }
        public ICollection<string> InvalidFiles;
    }
}
