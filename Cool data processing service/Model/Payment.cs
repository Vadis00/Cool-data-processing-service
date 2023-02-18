using System.Collections.ObjectModel;

namespace Cool_data_processing_service.Model
{
    public class Payment
    {
        public Payment()
        {
            Service = new Collection<Service>();
        }
        public string City { get; set; }
        public ICollection<Service> Service { get; set; }

        public decimal Total { get; set; }
    }
}
