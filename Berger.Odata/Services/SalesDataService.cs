using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;

namespace Berger.Odata.Services
{
    public class SalesDataService : ISalesData
    {
        private readonly IHttpClientService _httpClient;

        public SalesDataService(IHttpClientService httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<SalesDataModel>> GetSalesData(string filter)
        {

            string fullUrl =
                $"{OdataUrlBuilder.SalesUrl}{filter}";

            var responseBody = _httpClient.GetHttpResponse(fullUrl, OdataUrlBuilder.userName,
                OdataUrlBuilder.password);
            var parsedData = Parser<SalesDataRootModel>.ParseJson(responseBody);
            var data = parsedData.results.Select(a => new SalesDataModel()
            {
                CompanyCode = a.Bukrs,
                Division = a.Spart,
                MatarialBrand = a.Matkl,
                MatrialCode = a.Matnr,
                UnitOfMeasurement = a.Gk,
                SalesOrg = a.Vkorg,
                CustomerNoSold = a.Kunrg,
                CancelledInvoiceNo = a.Sfakn,
                CustomerNoShip = a.Kunnr_Sh,
                EmployeeNumber = a.PayerBp,//
                InvoiceNo = a.Vbeln,
                DealerOrSubDealer = a.PayResponsible,//
                PriceGroup = a.Konda,
                CustomerGroup = a.Kdgrp,
                SalesDistrict = a.Bzirk,
                SalesOffice = a.VkburC,//
                SalesGroup = a.VkgrpC,//
                CustomerClassification = a.Kukla,
                BillingType = a.Fkart,
                CustomerNumber = a.Payer,
                Date = a.Fkdat,
                LineNumber = a.Posnr,
                UnitOfMeasure = a.Meins,
                VolumeUnit = a.Voleh,
                Territory = a.Territory,
                Zone = a.Szone,
                CustomerName = a.Cname,
                MobileNo = a.MobileNo,
                DivisionName = a.SpartText,//
                NetAmount = a.Revenue,
                VATAmount = a.Mwas,
                Currency = a.Zsdx,
                MRP = a.Zmrp,
                DefaultDiscount = a.Zdis,
                AdditionalDiscount = a.Zdit,
                SchemeBenefit = a.Yfg3,
                PlantOrBusinessArea = a.Gsber,
                CollectionTax = a.Zcol,
                Quantity = a.Fkimg,
                Volume = a.Volum,
                CustomerAccountGroup = a.Ktokd,
                PackSize = a.Groes,
                InvoiceCreateBy = a.Ernam,
                SDDocCategory = a.Vbtyp,
                NoOfDocCondition = a.Knumv,
                OutboundDeliveryDocNo = a.Vgbel,
                SalesOrderNo = a.Aubel,
                ItemCategory = a.Pstyv,
                DistributionChannel = a.Vtweg,
                Brand = a.Wgbez
            });

            return data;
        }

        public async Task<IEnumerable<SalesDataModel>> GetInvoiceHistory(SalesDataSearchModel model)
        {
            model.FromDate = "2011-09-01T00:00:00";
            model.ToDate = "2011-10-01T00:00:00";
            //model.FromDate = DateTime.Now.ToString("s");
            //model.ToDate = DateTime.Now.AddMonths(-1).ToString("s");
            model.CustomerNo = 24;
            model.Division = 10;
            var filterQuery =
                $"&$filter={DataColumnDef.CustomerNoSold} eq '{model.CustomerNo}' " +
                $"and {DataColumnDef.Division} eq '{model.Division}' " +
                $"and {DataColumnDef.Date} gt datetime'{model.FromDate}' " +
                $"and {DataColumnDef.Date} lt datetime'{model.ToDate}'";

            var data = await GetSalesData(filterQuery);

            data = data.Select(a => new SalesDataModel { InvoiceNo = a.InvoiceNo, Date = a.Date, NetAmount = a.NetAmount });
            //return  await GetInvoiceDetails(new SalesDataSearchModel());
            return data;

        }
        public async Task<dynamic> GetInvoiceDetails(SalesDataSearchModel model)
        {
            model.FromDate = "2011-09-01T00:00:00";
            model.ToDate = "2011-10-08T00:00:00";
            //model.FromDate = DateTime.Now.ToString("s");
            //model.ToDate = DateTime.Now.AddMonths(-1).ToString("s");
            model.CustomerNo = 24;
            model.Division = 10;
            model.InvoiceNo = "30199128";
            var filterQuery =
                $"&$filter={DataColumnDef.CustomerNoSold} eq '{model.CustomerNo}' " +
                //$"and {DataColumnDef.Division} eq '{model.Division}' " +
                $"and {DataColumnDef.Date} gt datetime'{model.FromDate}' " +
                $"and {DataColumnDef.Date} lt datetime'{model.ToDate}' " +
                $"and {DataColumnDef.InvoiceNo} eq '{model.InvoiceNo}'";

            var data = await GetSalesData(filterQuery);

            var headerView = data.Select(a => new SalesDataModel
            {
                CustomerName = a.CustomerName,
                DivisionName = a.DivisionName,
                Date = a.Date,
                NetAmount = data.Sum(b => Convert.ToDecimal(b.NetAmount)).ToString(),
            });

            return headerView;

        }

        public async Task<dynamic> GetInvoiceItemDetails(SalesDataSearchModel model)
        {
            model.FromDate = "2011-09-01T00:00:00";
            model.ToDate = "2011-10-08T00:00:00";
            //model.FromDate = DateTime.Now.ToString("s");
            //model.ToDate = DateTime.Now.AddMonths(-1).ToString("s");
            model.CustomerNo = 24;
            model.Division = 10;
            model.InvoiceNo = "30199128";
            var filterQuery =
                $"&$filter={DataColumnDef.CustomerNoSold} eq '{model.CustomerNo}' " +
                //$"and {DataColumnDef.Division} eq '{model.Division}' " +
                $"and {DataColumnDef.Date} gt datetime'{model.FromDate}' " +
                $"and {DataColumnDef.Date} lt datetime'{model.ToDate}' " +
                $"and {DataColumnDef.InvoiceNo} eq '{model.InvoiceNo}'";

            var data = await GetSalesData(filterQuery);

            var itemDetails = data.Select(a => new
            {
                Matarial = a.MatarialBrand,
                MatarialDesc = a.MatarialName,
                Quantity = a.Quantity,
                NetAmount = a.NetAmount
            });
            return itemDetails;
        }

        public async Task<dynamic> GetBrandWiseMTDDetails(SalesDataSearchModel model)
        {
            var currendate = new DateTime(2011, 09, 01);

            var cyfd = currendate.GetCYFD().DateFormat();
            var cyld = currendate.GetCYLCD().DateFormat();

            var lyfd = currendate.GetLYFD().DateFormat();
            var lyld = currendate.GetLYLCD().DateFormat();
            var dic = new Dictionary<string, IEnumerable<SalesDataModel>>();

            var filterCyQuery = $"&$filter=" + $"{DataColumnDef.Division} eq '{model.Division}' "
                                           + $"and {DataColumnDef.Date} ge datetime'{cyfd}' "
                                           + $"and {DataColumnDef.Date} le datetime'{cyld}' "
                                           + $"&$top=10";

            
            var dataCy = await GetSalesData(filterCyQuery);


            var filterLyQuery = $"&$filter=" + $"{DataColumnDef.Division} eq '{model.Division}' "
                                         + $"and {DataColumnDef.Date} ge datetime'{lyfd}' "
                                         + $"and {DataColumnDef.Date} le datetime'{lyld}' "
                                         + $"&$top=10";

            var dataLy= await GetSalesData(filterLyQuery);

           

            for (var i = 1; i <= 3; i++)
            {
                int number = int.Parse($"-{i}");
                var monthQuery = $"&$filter=" + $"{DataColumnDef.Division} eq '{model.Division}' "
                                     + $"and {DataColumnDef.Date} ge datetime'{currendate.GetMonthDate(number).GetCYFD().DateFormat()}' "
                                     + $"and {DataColumnDef.Date} le datetime'{currendate.GetMonthDate(number).GetCYLD().DateFormat()}' "
                                     + $"&$top=10";
                dic.Add(currendate.GetMonthName(number), await GetSalesData(monthQuery));
            }

         

       
         
             
            //{
            //    var sum = f.Value.GroupBy(g => g.Brand).Select(s=>s.Sum(s=>Convert.ToDouble(s.Volume)));
            //    return new { f.Key, sum };

            //});
                
            // var  result = dic.ToList();

            //var result = dataLyQuery
            //    .GroupBy(g => g.Brand)
            //    .Select(s =>
            //    {

            //        //var LYMTD = s
            //        //        //.Where(f => Convert.ToDateTime(f.Date).Date <= currentDate.GetLYFD().Date && Convert.ToDateTime(f.Date) >= currentDate.GetLYLD().Date)
            //        //        .Sum(s => Convert.ToDouble(s.Volume));

            //        //var CYMTD = s
            //        //        //.Where(f => Convert.ToDateTime(f.Date).Date <= currentDate.GetCYFD().Date && Convert.ToDateTime(f.Date) >= currentDate.GetCYLD().Date)
            //        //        .Sum(s => Convert.ToDouble(s.Volume));

            //        //var GROWTH = (LYMTD - CYMTD);

            //        return new Dictionary<string, object>
            //        {
            //            //{ "Brand", s.Key },
            //            //{ "LY MTD",LYMTD},
            //            //{ "CY MTD", CYMTD},
            //            //{ "Growth%",GROWTH},
            //            //{
            //            //    currentDate.GetMonthName(-1),
            //            //     s
            //            //    // .Where(f => Convert.ToDateTime(f.Date).Date <= currentDate.GetMonthDate(-1).GetCYFD().Date && Convert.ToDateTime(f.Date) >= currentDate.GetMonthDate(-1).GetCYLD().Date)
            //            //     .Sum(s => Convert.ToDouble(s.Volume))
            //            //},
            //            //{    currentDate.GetMonthName(-2),
            //            //     s
            //            // //    .Where(f => Convert.ToDateTime(f.Date).Date <= currentDate.GetMonthDate(-2).GetCYFD().Date && Convert.ToDateTime(f.Date) >= currentDate.GetMonthDate(-2).GetCYLD().Date)
            //            //    .Sum(s => Convert.ToDouble(s.Volume))
            //            //},
            //            //{   currentDate.GetMonthName(-3),
            //            //    s
            //            //   // .Where(f => Convert.ToDateTime(f.Date).Date <= currentDate.GetMonthDate(-3).GetCYFD().Date && Convert.ToDateTime(f.Date) >= currentDate.GetMonthDate(-3).GetCYLD().Date)
            //            //    .Sum(s => Convert.ToDouble(s.Volume))
            //            //},
            //            //{ "Date",s.FirstOrDefault(f => f.Brand == s.Key).Date},
            //        };


            //    });


            return dic.ToList();
        }

        private async Task<Dictionary<string, IEnumerable<SalesDataModel>>> GetDataByFilterQueryAsync(string key, string query)
        {
            var result = new Dictionary<string, IEnumerable<SalesDataModel>>();
            var data = await GetSalesData(query);
            result.Add(key, data);
            return result;

        }
    }

}

