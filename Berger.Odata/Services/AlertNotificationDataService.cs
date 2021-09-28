using Berger.Common.Extensions;
using Berger.Odata.Common;
using Berger.Odata.Extensions;
using Berger.Odata.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Berger.Odata.Services
{
   public class AlertNotificationDataService: IAlertNotificationDataService
    {
        private readonly IAlertNotificationODataService _alertNotificationOData;
        private readonly IODataService _oODataService;

        public AlertNotificationDataService(
            IAlertNotificationODataService alertNotificationOData,
            IODataService oODataService
            )
        {
            _alertNotificationOData = alertNotificationOData;
            _oODataService = oODataService;
        }

        public async Task<IList<CollectionDataModel>> GetAllTodayCheckBounces()
        {
            var today = DateTime.Now;
            var fromDate = today.DateTimeFormat();
            var toDate = today.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder
                                .AddProperty(CollectionColDef.CustomerNo)
                                .AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.BusinessArea)
                                .AddProperty(CollectionColDef.Territory)
                                .AddProperty(CollectionColDef.ChequeNo)
                                .AddProperty(CollectionColDef.Amount);

            var data = (await _oODataService.GetCollectionData(selectQueryBuilder, 
                                                startPostingDate: fromDate, endPostingDate: toDate,
                                                bounceStatus: ConstantsValue.ChequeBounceStatus,
                                                docType: ConstantsValue.ChequeDocTypeDA)).ToList();

            return data;
        }

        public async Task<IList<CustomerDataModel>> GetAllTodayCreditLimitCross()
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();

            selectQueryBuilder.AddProperty(nameof(CustomerDataModel.SalesOffice))
                                .AddProperty(nameof(CustomerDataModel.SalesGroup))
                                .AddProperty(nameof(CustomerDataModel.CustZone))
                                .AddProperty(nameof(CustomerDataModel.PriceGroup))
                                .AddProperty(nameof(CustomerDataModel.Division))
                                .AddProperty(nameof(CustomerDataModel.Channel))
                                .AddProperty(nameof(CustomerDataModel.BusinessArea))
                                .AddProperty(nameof(CustomerDataModel.CustomerName))
                                .AddProperty(nameof(CustomerDataModel.CustomerNo))
                                .AddProperty(nameof(CustomerDataModel.Channel))
                                .AddProperty(nameof(CustomerDataModel.CreditControlArea))
                                .AddProperty(nameof(CustomerDataModel.CreditLimit))
                                .AddProperty(nameof(CustomerDataModel.TotalDue));

            var data = (await _alertNotificationOData.GetCustomerDataByMultipleCustomerNo(selectQueryBuilder)).ToList();

            //var groupData = data.GroupBy(x => new { x.CustomerNo, x.CreditControlArea }).ToList();

            //var result = groupData.Select(x =>
            //{
            //    var notifyModel = new AppCreditLimitCrossNotificationModel();
            //    notifyModel.CustomerNo = x.Key.CustomerNo.ToString();
            //    notifyModel.CustomerName = x.FirstOrDefault()?.CustomerName ?? string.Empty;
            //    notifyModel.CreditControlArea = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
            //    notifyModel.CreditLimit = x.Where(f => f.Channel == ConstantsValue.DistrbutionChannelDealer).GroupBy(g => g.CreditLimit).Sum(c => c.Key);
            //    notifyModel.TotalDue = x.Where(f => f.Channel == ConstantsValue.DistrbutionChannelDealer).GroupBy(g => g.TotalDue).Sum(c => c.Key);
            //    return notifyModel;
            //}).ToList();

            //result = result.Where(x => x.TotalDue > x.CreditLimit).ToList();


            return data;
        }

        public async Task<IList<FinancialDataModel>> GetAllTodayPaymentFollowUp()
        {
            var today = DateTime.Now;
            var fromDate = today.DateTimeFormat();
            var toDate = today.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)

                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.DayLimit)
                                .AddProperty(FinancialColDef.Date);


            var data = (await _alertNotificationOData.GetFinancialDataByCustomer(selectQueryBuilder,startDate: fromDate, endDate: toDate)).ToList();

            return data;
        }

        public async Task<IList<CustomerOccasionDataModel>> GetAllTodayCustomerOccasions()
        {
            var today = DateTime.Now;
            var oldDate = new DateTime(1000, 01, 01);
            var dateFormat = "yyyyMMdd";
            var resultDateFormat = "dd MMM yyyy";

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(CustomerOccasionColDef.Customer)
                                .AddProperty(CustomerOccasionColDef.Name)
                                .AddProperty(CustomerOccasionColDef.SalesOffice)
                                .AddProperty(CustomerOccasionColDef.DistrChannel)
                                .AddProperty(CustomerOccasionColDef.Division)
                                .AddProperty(CustomerOccasionColDef.DOB)
                                .AddProperty(CustomerOccasionColDef.SpouseDOB)
                                .AddProperty(CustomerOccasionColDef.FirstChildDOB)
                                .AddProperty(CustomerOccasionColDef.SecondChildDOB)
                                .AddProperty(CustomerOccasionColDef.ThirdChildDOB);

            var data = (await _alertNotificationOData.GetCustomerOccasionData(selectQueryBuilder)).ToList();

            return data;
        }
    }
}
