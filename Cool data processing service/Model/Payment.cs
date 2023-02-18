using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cool_data_processing_service.Model
{
    public class Payment
    {
        public Payment()
        {
            Service = new();
        }
        public string City { get; set; }
        public  Service Service { get; set; }
        public decimal Total { get; set; }
    }
}
