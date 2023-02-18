using Cool_data_processing_service.Model;
using Microsoft.VisualBasic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace Cool_data_processing_service.Service
{
    public class FileProcessingService
    {

        public async Task NewFile(string filleWay)
        {
            await ParseFile(filleWay);
        }
        private async Task<ICollection<Payment>> ParseFile(string filleWay)
        {
            Collection<Payment> paymentList = new Collection<Payment>();
            //John, Doe, “Lviv, Kleparivska 35, 4”, 500.0, 2022-27-01, 1234567, Water

            var fille = await File.ReadAllLinesAsync(filleWay);

            foreach (var line in fille)
            {
                var payment = new Payment();

                var name = line.Split(',');

                decimal paymentSum;
                DateTime paymentDate;
                long accountNumber;

                payment.Service.Payers.Name = $"{name[0]} {name[1]}";
                payment.City = $"{name[2]} {name[3]} {name[4]}";

                if (Decimal.TryParse(name[5], out paymentSum))
                    payment.Service.Payers.Payment = paymentSum;
                else
                {
                    //incorrect paymentSum type
                }


                if (DateTime.TryParseExact(name[6].Replace(" ", ""),
                                           "yyyy-dd-MM",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out paymentDate))
                    payment.Service.Payers.Date = paymentDate;
                else
                {
                    //incorrect paymentDate type
                }

                if (Int64.TryParse(name[7], out accountNumber))
                    payment.Service.Payers.AccountNumber = accountNumber;
                else
                {
                    //incorrect accountNumber type
                }

                payment.Service.Name = name[8];

                paymentList.Add(payment);
            }
            return paymentList;
        }
    }
}
