using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace Cool_data_processing_service.Model
{
    public class Service
    {
        public Service()
        {
            Payers = new Collection<Payers>();
        }
        public string Name { get; set; } 
        public ICollection<Payers> Payers { get; set; }

        public decimal Total { get; set; }
    }
}
