using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool_data_processing_service.Service
{
    public class LoggerService
    {
        private readonly Model.Log log;
        public LoggerService()
        {
            log = new();
        }

        public void Error(string filleWay)
        {
            log.FoundErrors++;
            log.InvalidFiles.Add(filleWay);
        }

        public void Done()
        {

        }
    }
}
