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
        private readonly IODataService _odataService;
        //private readonly IODataCommonService _odataCommonService;

        public AlertNotificationDataService(
            IODataService odataService
            //IODataCommonService odataCommonService
            )
        {
            _odataService = odataService;
            //_odataCommonService = odataCommonService;
        }

        public async Task<IList<AppChequeBounceNotificationModel>> GetAllTodayCheckBounces()
        {
            var today = DateTime.Now;
            var resultDateFormat = "dd MMM yyyy";
            var fromDate = today.DateTimeFormat();
            var toDate = today.DateTimeFormat();

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(CollectionColDef.CustomerNo)

                                .AddProperty(CollectionColDef.Depot)
                                .AddProperty(CollectionColDef.BusinessArea)
                                .AddProperty(CollectionColDef.BounceStatus)
                                .AddProperty(CollectionColDef.PostingDate)
                                .AddProperty(CollectionColDef.CustomerName)
                                .AddProperty(CollectionColDef.DocNumber)
                                .AddProperty(CollectionColDef.ChequeNo)
                                .AddProperty(CollectionColDef.BankName)
                                .AddProperty(CollectionColDef.ClearDate)
                                .AddProperty(CollectionColDef.Amount)
                                .AddProperty(CollectionColDef.CreditControlArea);

            var data = (await _odataService.GetCustomerAndCreditControlArea(selectQueryBuilder, startClearDate: fromDate, endClearDate: toDate)).ToList();

            var result = data.Select(x =>
                                new AppChequeBounceNotificationModel()
                                {
                                    ReversalDate = CustomConvertExtension.ObjectToDateTime(x.PostingDate).DateFormat(resultDateFormat),
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    CreditControlArea = x.CreditControlArea,
                                    Amount = CustomConvertExtension.ObjectToDecimal(x.Amount),
                                    InstrumentNo = x.ChequeNo,
                                    DocumentNo = x.DocNumber,
                                    BankName = x.BankName,
                                    Reason = "Cheque Bounce-Insuff"
                                }).ToList();

            #region Credit Control Area 
            //var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            //foreach (var item in result)
            //{
            //    item.CreditControlAreaName = creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.CreditControlArea)?.Description ?? string.Empty;
            //}
            #endregion

            return result;
        }

        public async Task<IList<AppCreditLimitCrossNotificationModel>> GetAllTodayCreditLimitCross()
        {
            var selectQueryBuilder = new SelectQueryOptionBuilder();

            selectQueryBuilder.AddProperty(nameof(CustomerDataModel.SalesOffice))
                                .AddProperty(nameof(CustomerDataModel.SalesGroup))
                                .AddProperty(nameof(CustomerDataModel.CustZone))
                                .AddProperty(nameof(CustomerDataModel.Division))
                                .AddProperty(nameof(CustomerDataModel.Channel))
                                .AddProperty(nameof(CustomerDataModel.BusinessArea))
                                .AddProperty(nameof(CustomerDataModel.CustomerName))
                                .AddProperty(nameof(CustomerDataModel.CustomerNo))
                                .AddProperty(nameof(CustomerDataModel.Channel))
                                .AddProperty(nameof(CustomerDataModel.CreditControlArea))
                                .AddProperty(nameof(CustomerDataModel.CreditLimit))
                                .AddProperty(nameof(CustomerDataModel.TotalDue));

            var data = (await _odataService.GetCustomerDataByMultipleCustomerNo(selectQueryBuilder)).ToList();

            var groupData = data.GroupBy(x => new { x.CustomerNo, x.CreditControlArea }).ToList();

            var result = groupData.Select(x =>
            {
                var notifyModel = new AppCreditLimitCrossNotificationModel();
                notifyModel.CustomerNo = x.Key.CustomerNo.ToString();
                notifyModel.CustomerName = x.FirstOrDefault()?.CustomerName ?? string.Empty;
                notifyModel.CreditControlArea = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
                notifyModel.CreditLimit = x.Where(f => f.Channel == ConstantsValue.DistrbutionChannelDealer).GroupBy(g => g.CreditLimit).Sum(c => c.Key);
                notifyModel.TotalDue = x.Where(f => f.Channel == ConstantsValue.DistrbutionChannelDealer).GroupBy(g => g.TotalDue).Sum(c => c.Key);
                return notifyModel;
            }).ToList();

            result = result.Where(x => x.TotalDue > x.CreditLimit).ToList();

            #region Credit Control Area 
            //var creditControlAreas = await _odataCommonService.GetAllCreditControlAreasAsync();

            //foreach (var item in result)
            //{
            //    item.CreditControlAreaName = creditControlAreas.FirstOrDefault(f => f.CreditControlAreaId.ToString() == item.CreditControlArea)?.Description ?? string.Empty;
            //}
            #endregion

            return result;
        }

        public async Task<IList<AppPaymentFollowUpNotificationModel>> GetAllTodayPaymentFollowUp()
        {
            var today = DateTime.Now;
            var dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
            var resultDateFormat = "dd MMM yyyy";
            //var fromDate = (new DateTime(2011, 01, 01)).DateTimeFormat(); // need to get all data so date not fixed

            var selectCustomerQueryBuilder = new SelectQueryOptionBuilder();
            selectCustomerQueryBuilder.AddProperty(nameof(CustomerDataModel.CustomerNo))
                                .AddProperty(nameof(CustomerDataModel.Channel))
                                .AddProperty(nameof(CustomerDataModel.PriceGroup));

            var selectQueryBuilder = new SelectQueryOptionBuilder();
            selectQueryBuilder.AddProperty(FinancialColDef.CustomerNo)
                                .AddProperty(FinancialColDef.CreditControlArea)
                                .AddProperty(FinancialColDef.CustomerName)
                                .AddProperty(FinancialColDef.InvoiceNo)
                                .AddProperty(FinancialColDef.PostingDate)
                                .AddProperty(FinancialColDef.Age)
                                .AddProperty(FinancialColDef.DayLimit);

            var customerData = (await _odataService.GetCustomerDataByMultipleCustomerNo(selectCustomerQueryBuilder)).ToList();

            var data = (await _odataService.GetFinancialDataByCustomer(selectQueryBuilder)).ToList();

            #region data call by single customer
            //var data = new List<FinancialDataModel>();

            //foreach (var dealerId in dealerIds)
            //{
            //    //var dataSingle = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, dealerId.ToString(), fromDate)).ToList();
            //    var dataSingle = (await _odataService.GetFinancialDataByCustomerAndCreditControlArea(selectQueryBuilder, dealerId.ToString())).ToList();
            //    if (dataSingle.Any())
            //    {
            //        data.AddRange(dataSingle);
            //    }
            //}
            #endregion

            var result = data.Select(x =>
                                new AppPaymentFollowUpNotificationModel()
                                {
                                    CustomerNo = x.CustomerNo,
                                    CustomerName = x.CustomerName,
                                    InvoiceNo = x.InvoiceNo,
                                    InvoiceDate = x.PostingDate.DateFormatDate(format: dateFormat).DateFormat(resultDateFormat),
                                    InvoiceAge = x.Age,
                                    DayLimit = x.DayLimit
                                }).ToList();

            //var resultRPRS = new List<AppPaymentFollowUpNotificationModel>();
            //var resultFastPayCarry = new List<AppPaymentFollowUpNotificationModel>();

            //#region RPRS
            //var dealersRPRS = customerData.Where(x => x.Channel == ConstantsValue.DistrbutionChannelDealer &&
            //                                        x.PriceGroup == ConstantsValue.PriceGroupCreditBuyer).ToList();
            //var dealerIdsRPRS = dealersRPRS.Select(x => x.CustomerNo.ToString()).Distinct().ToList();

            //var rprsDayPolicy = await _odataCommonService.GetAllRPRSPoliciesAsync();

            //foreach (var item in result.Where(x => dealerIdsRPRS.Contains(x.CustomerNo)))
            //{
            //    var dayCount = rprsDayPolicy.FirstOrDefault(x => CustomConvertExtension.ObjectToInt(item.DayLimit) >= x.FromDaysLimit &&
            //                                    CustomConvertExtension.ObjectToInt(item.DayLimit) <= x.ToDaysLimit)?.RPRSDays ?? 0;
            //    var dayNotifyCount = rprsDayPolicy.FirstOrDefault(x => CustomConvertExtension.ObjectToInt(item.DayLimit) >= x.FromDaysLimit &&
            //                                    CustomConvertExtension.ObjectToInt(item.DayLimit) <= x.ToDaysLimit)?.NotificationDays ?? 0;
            //    item.RPRSDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayCount).DateFormat(resultDateFormat);
            //    item.NotificationDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayNotifyCount).DateFormat(resultDateFormat);
            //    item.PaymentFollowUpType = EnumPaymentFollowUpTypeModel.RPRS;

            //    if (item.NotificationDate.DateFormatDate(resultDateFormat).Date == today.Date)
            //        resultRPRS.Add(item);
            //}
            //#endregion

            //#region FastPayCarry
            //var dealersFastPayCarry = customerData.Where(x => x.Channel == ConstantsValue.DistrbutionChannelDealer &&
            //                                        (x.PriceGroup == ConstantsValue.PriceGroupCashBuyer ||
            //                                        x.PriceGroup == ConstantsValue.PriceGroupFastPayCarry)).ToList();
            //var dealerIdsFastPayCarry = dealersFastPayCarry.Select(x => x.CustomerNo.ToString()).Distinct().ToList();

            //foreach (var item in result.Where(x => dealerIdsFastPayCarry.Contains(x.CustomerNo)))
            //{
            //    //var dayCount = 5;
            //    var dayNotifyCount = 3;
            //    //item.RPRSDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayCount).DateFormat(resultDateFormat);
            //    item.NotificationDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayNotifyCount).DateFormat(resultDateFormat);
            //    item.PaymentFollowUpType = EnumPaymentFollowUpTypeModel.FastPayCarry;

            //    if (item.NotificationDate.DateFormatDate(resultDateFormat).Date == today.Date)
            //        resultFastPayCarry.Add(item);
            //}
            //#endregion

            //result = resultRPRS.Concat(resultFastPayCarry).ToList();

            return result;
        }

        public async Task<IList<AppCustomerOccasionNotificationModel>> GetAllTodayCustomerOccasions()
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

            var data = (await _odataService.GetCustomerOccasionData(selectQueryBuilder)).ToList();

            var result = data.Select(x =>
                                new AppCustomerOccasionNotificationModel()
                                {
                                    CustomerNo = x.Customer,
                                    CustomerName = x.Name,
                                    DOB = !string.IsNullOrEmpty(x.DOB) ? x.DOB.DateFormatDate(format: dateFormat).DateFormat(resultDateFormat) :
                                                                        oldDate.DateFormat(resultDateFormat),
                                    SpouseDOB = !string.IsNullOrEmpty(x.SpouseDOB) ? x.SpouseDOB.DateFormatDate(format: dateFormat).DateFormat(resultDateFormat) :
                                                                                    oldDate.DateFormat(resultDateFormat),

                                    ChildDOB = !string.IsNullOrEmpty(x.FirstChildDOB) ? x.FirstChildDOB.DateFormatDate(format: dateFormat).DateFormat(resultDateFormat) :
                                                    !string.IsNullOrEmpty(x.SecondChildDOB) ? x.SecondChildDOB.DateFormatDate(format: dateFormat).DateFormat(resultDateFormat) :
                                                        !string.IsNullOrEmpty(x.ThirdChildDOB) ? x.ThirdChildDOB.DateFormatDate(format: dateFormat).DateFormat(resultDateFormat) :
                                                            oldDate.DateFormat(resultDateFormat),
                                }).ToList();

            result = result.Where(x => x.DOB.DateFormatDate(format: resultDateFormat).Date == today.Date ||
                                        x.SpouseDOB.DateFormatDate(format: resultDateFormat).Date == today.Date ||
                                        x.ChildDOB.DateFormatDate(format: resultDateFormat).Date == today.Date).ToList();

            return result;
        }
    }
}
