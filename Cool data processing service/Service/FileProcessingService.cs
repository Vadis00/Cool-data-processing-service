﻿using Cool_data_processing_service.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Xml.Linq;
using System.Linq;

namespace Cool_data_processing_service.Service
{
    public class FileProcessingService
    {
        private readonly LoggerService logger;
        private readonly DataService _dataService;
        public FileProcessingService(LoggerService loggerService, DataService dataService)
        {
            logger = loggerService;
            _dataService = dataService;
        }

        public async void NewFileAsync(string filleWay)
        {
            var paymentList = await ParseFileAsync(filleWay);

            string jsonString = JsonSerializer.Serialize(paymentList);

            await _dataService.SaveAsync(jsonString);
        }

        private async Task<ICollection<Payment>> ParseFileAsync(string filleWay)
        {
            var paymentList = new Collection<Payment>();

            var lines = await _dataService.ReadAsync(filleWay);

            foreach(var line in lines)
            {
                var name = line.Split(new string[] { ", " }, StringSplitOptions.None);

                if (!ValidationCheck(name))
                {
                    logger.Error(filleWay);
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

                logger.Done("line");
            }


            logger.Done("fille");

            return paymentList;
        }

        private bool ValidationCheck(string[] line)
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
    }
}

