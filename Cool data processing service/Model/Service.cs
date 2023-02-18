using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool_data_processing_service.Model
{
    public class Service
    {
        public Service()
        {
            Payers = new();
        }
        public string Name { get; set; } 
        public Payers Payers { get; set; } 

    }
}
