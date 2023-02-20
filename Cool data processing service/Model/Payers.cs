using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cool_data_processing_service.Model
{
    public class Payers
    {
        public string Name { get; set; }
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }

        [JsonPropertyName("account_number")]
        public long AccountNumber { get; set; }
    }
}
