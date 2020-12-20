using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Berger.Common.HttpClient;
using Berger.Common.JSONParser;
using Berger.Odata.Common;
using Berger.Odata.Model;
using ODataHttpClient;
using ODataHttpClient.Models;

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
                MatrialCode =  a.Matnr,
                UnitOfMeasurement = a.Gk,
                SalesOrg =  a.Vkorg,
                CustomerNoSold =  a.Kunrg,
                CancelledInvoiceNo = a.Sfakn,
                CustomerNoShip = a.Kunnr_Sh,
                EmployeeNumber =  a.PayerBp,//
                InvoiceNo = a.Vbeln,
                DealerOrSubDealer = a.PayResponsible,//
                PriceGroup = a.Konda,
                CustomerGroup = a.Kdgrp,
                SalesDistrict =  a.Bzirk,
                SalesOffice = a.VkburC,//
                SalesGroup =  a.VkgrpC,//
                CustomerClassification = a.Kukla,
                BillingType =  a.Fkart,
                CustomerNumber = a.Payer,
                Date =a.Fkdat,
                LineNumber = a.Posnr,
                UnitOfMeasure = a.Meins,
                VolumeUnit = a.Voleh,
                Territory = a.Territory,
                Zone =  a.Szone,
                CustomerNaem = a.Cname,
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
                DistributionChannel = a.Vtweg
            });

            return data;
        }

        public async Task<IEnumerable<SalesDataModel>> GetInvoiceHistory(SalesDataSearchModel model)
        {
            model.FromDate = "2011-09-01T00:00:00";
            model.ToDate = "2011-09-08T00:00:00";
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

            data = data.Select(a => new SalesDataModel{InvoiceNo = a.InvoiceNo, Date = a.Date, NetAmount = a.NetAmount});
            
            return data;

        }
        public async Task<IEnumerable<SalesDataModel>> GetInvoiceDetails(SalesDataSearchModel model)
        {
            model.FromDate = "2011-09-01T00:00:00";
            model.ToDate = "2011-09-08T00:00:00";
            //model.FromDate = DateTime.Now.ToString("s");
            //model.ToDate = DateTime.Now.AddMonths(-1).ToString("s");
            model.CustomerNo = 24;
            model.Division = 10;
            var filterQuery =
                $"&$filter={DataColumnDef.CustomerNoSold} eq '{model.CustomerNo}' " +
                $"and {DataColumnDef.Division} eq '{model.Division}' " +
                $"and {DataColumnDef.Date} gt datetime'{model.FromDate}' " +
                $"and {DataColumnDef.Date} lt datetime'{model.ToDate}' " +
                $"and {DataColumnDef.InvoiceNo} eq '{model.InvoiceNo}'";

            var data = await GetSalesData(filterQuery);

            data = data.Select(a => new SalesDataModel { InvoiceNo = a.InvoiceNo, Date = a.Date, NetAmount = a.NetAmount });

            return data;

        }
    }
}
