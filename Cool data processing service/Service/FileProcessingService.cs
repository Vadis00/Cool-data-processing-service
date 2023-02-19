using Cool_data_processing_service.Const;
using Cool_data_processing_service.Enum;
using Cool_data_processing_service.Model;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Text.Json;

namespace Cool_data_processing_service.Service
{
    public class FileProcessingService
    {
        private readonly LoggerService logger;

        private readonly DataService _dataService;
        private readonly string _directoryPath;
        private DateTime _currentDate;
        public FileProcessingService(LoggerService loggerService, DataService dataService)
        {
            _directoryPath = ConfigurationManager.AppSettings.Get("OutputFolder");

            _currentDate = DateTime.Now;
            logger = loggerService;
            _dataService = dataService;
        }

        /// <summary>
        /// Parse the file, generate the result in json and save the file with the result
        /// </summary>
        /// <param name="filleWay">The path to the file</param>
        /// <param name="fileType">File type (.txt or .csv)</param>
        public async void NewFileAsync(string filleWay, FileType fileType)
        {
            var paymentList = await ParseFileAsync(filleWay, fileType);

            if(paymentList.Count == 0)
            {
                logger.Error(filleWay, FileStatus.InvalidLine);
            }
            string jsonString = JsonSerializer.Serialize(paymentList);

            string fillePath = generateFilleResultPath();

            await _dataService.SaveAsync(fillePath, jsonString);
        }


        private async Task<ICollection<Payment>> ParseFileAsync(string filleWay, FileType fileType)
        {
            var paymentList = new Collection<Payment>();
            int offset = 0;
            switch (fileType)
            {
                case FileType.Txt:
                    break;
                case FileType.Csv:
                    //shift by 2 rows for csv files
                    offset = 2;
                    break;
            }

            try
            {
                //Read text file as a list of strings
                var lines = await _dataService.ReadAsync(filleWay);

                //Parse each line of a text file
                foreach (var line in lines)
                {
                    if (offset != 0)
                    {
                        offset--;
                        continue;
                    }

                    var name = line.Split(new string[] { ", " }, StringSplitOptions.None);

                    //Skip the line if it is not in the correct format, update the log
                    if (!validationCheck(name))
                    {
                        logger.Error(filleWay, FileStatus.InvalidLine);
                        continue;
                    }

                    name[2] = name[2].Replace("“", "");

                    Payers payer = new()
                    {
                        Name = $"{name[0]} {name[1]}",
                        Payment = Decimal.Parse(name[5].Replace(".", ",")),
                        AccountNumber = Int64.Parse(name[7]),
                        Date = DateTime.ParseExact(name[6],
                                               "yyyy-dd-MM",
                                               CultureInfo.InvariantCulture,
                                               DateTimeStyles.None)
                    };

                    var payment = paymentList.Where(p => p.City == name[2])
                                             .DefaultIfEmpty(new() { City = name[2] })
                                             .FirstOrDefault();

                    var service = payment?.Service.Where(s => s.Name == name[8])
                                                  .DefaultIfEmpty(new() { Name = name[8] })
                                                  .FirstOrDefault();


                    service?.Payers.Add(payer);


                    service.Total = service.Payers.Sum(s => s.Payment);
                    payment.Total = payment.Service.Sum(s => s.Total);

                    if (!paymentList.Contains(payment))
                        paymentList.Add(payment);

                    if (!payment.Service.Contains(service))
                        payment?.Service.Add(service);

                    logger.Done(FileStatus.DoneLine);
                }


                logger.Done(FileStatus.DoneFile);
            }
            catch
            {
                logger.Error(filleWay, FileStatus.InvalidLine);
            }

            return paymentList;
        }

        /// <summary>
        /// Checks if a file string is in the correct format.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool validationCheck(string[] line)
        {
            decimal paymentSum;
            DateTime paymentDate;
            long accountNumber;

            if (line.Length != 9)
                return false;

            if (!Decimal.TryParse(line[5].Replace(".", ","), out paymentSum))
                return false;

            if (!DateTime.TryParseExact(line[6],
                           "yyyy-dd-MM",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out paymentDate))
                return false;

            if (!Int64.TryParse(line[7], out accountNumber))
                return false;

            return true;
        }

        /// <summary>
        /// Generates a path for an output file
        /// </summary>
        /// <returns></returns>
        private string generateFilleResultPath()
        {
            _currentDate = DateTime.Now;
            var path = $@"{_directoryPath}\{_currentDate.ToString("dd.MM.yyyy")}";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return $@"{path}\output-{Guid.NewGuid()}.json";
        }
    }
}