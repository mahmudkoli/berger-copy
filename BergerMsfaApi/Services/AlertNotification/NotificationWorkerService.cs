using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.AlertNotification;
using Berger.Odata.Common;
using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using BergerMsfaApi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.AlertNotification
{
    public class NotificationWorkerService : INotificationWorkerService
    {
        private readonly IOccasionToCelebrateService _occasionToCelebrate;
        private readonly ICreditLimitCrossNotifictionService _crossNotifictionService;
        private readonly IChequeBounceNotificationService _chequeBounceNotificationService;
        private readonly IPaymentFollowupService _paymentFollowupService;
        private readonly IAlertNotificationDataService _alertNotificationData;
        private readonly IODataCommonService _odataCommonService;
        private readonly IAuthService _authService;
        private readonly ILeadService _leadService;


        public NotificationWorkerService(IOccasionToCelebrateService occasionToCelebrate,
            IAlertNotificationDataService alertNotificationData,
            ICreditLimitCrossNotifictionService crossNotifictionService,
            IChequeBounceNotificationService chequeBounceNotificationService,
            IPaymentFollowupService paymentFollowupService,
            IAuthService authService,
            ILeadService leadService
            )
        {
            _occasionToCelebrate = occasionToCelebrate;
            _alertNotificationData = alertNotificationData;
            _crossNotifictionService = crossNotifictionService;
            _chequeBounceNotificationService = chequeBounceNotificationService;
            _paymentFollowupService = paymentFollowupService;
            _authService = authService;
            _leadService = leadService;
        }

        public async Task<bool> GetCheckBounceNotification()
        {
            var appUser = AppIdentity.AppUser;
            var customer =await _authService.GetDealerByUserId(appUser.UserId);
            var checkBounce = _chequeBounceNotificationService.GetChequeBounceNotification(customer);


            return true;

        }

        public async Task<bool> GetCreaditLimitNotification()
        {
            var appUser = AppIdentity.AppUser;
            var customer = await _authService.GetDealerByUserId(appUser.UserId);
            var checkBounce = _chequeBounceNotificationService.GetChequeBounceNotification(customer);


            return true;

        }

        public async Task<bool> GetOccassionToCelebrste()
        {
            var appUser = AppIdentity.AppUser;
            var customer = await _authService.GetDealerByUserId(appUser.UserId);

            var checkBounce =await _occasionToCelebrate.GetOccasionToCelebrate(customer);

            return true;
        }

        public async Task<bool> SaveCheckBounceNotification()
        {
            bool result = false;
            IList<ChequeBounceNotification> lstchequeBounceNotification = new List<ChequeBounceNotification>();
            var chequeBounceNotification = await _alertNotificationData.GetAllTodayCheckBounces();

            if (chequeBounceNotification.Count > 0)
            {
                foreach (var item in chequeBounceNotification)
                {
                    ChequeBounceNotification chequeBounce = new ChequeBounceNotification()
                    {
                        Amount=Convert.ToDecimal(item.Amount),
                        CreditControlArea=item.CreditControlArea,
                        BankName=item.BankName,
                        ChequeNo=item.ChequeNo,
                        ClearDate=item.ClearDate,
                        CustomarNo=item.CustomerNo,
                        CustomerName=item.CustomerName,
                        Depot=item.Depot,
                        DocNumber=item.DocNumber,
                        NotificationDate=DateTime.Now,
                        
                        
                    };

                    lstchequeBounceNotification.Add(chequeBounce);
                }
                result = await _chequeBounceNotificationService.SaveMultipleChequeBounceNotification(lstchequeBounceNotification);
            }

            return result;
        }

        public async Task<bool> SaveCreaditLimitNotification()
        {
            bool result = false;
            IList<CreditLimitCrossNotifiction> lstcreditLimitCrossNotifiction = new List<CreditLimitCrossNotifiction>();
            var creditLimitCrossNotifiction = await _alertNotificationData.GetAllTodayCreditLimitCross();

            if (creditLimitCrossNotifiction.Count > 0)
            {
                foreach (var item in creditLimitCrossNotifiction)
                {
                    CreditLimitCrossNotifiction creditLimit = new CreditLimitCrossNotifiction()
                    {
                        CreditControlArea=item.CreditControlArea,
                        CreditLimit=item.CreditLimit.ToString(),
                        CustomarNo=item.CustomerNo,
                        Zone=item.CustZone,
                        CustomerName=item.CustomerName,
                        Division=item.Division,
                        TotalDue=item.TotalDue.ToString(),
                        NotificationDate=DateTime.Now,
                        PriceGroup=item.PriceGroup
                    };

                    lstcreditLimitCrossNotifiction.Add(creditLimit);
                }
                result = await _crossNotifictionService.SaveMultipleCreditLimitCrossNotifiction(lstcreditLimitCrossNotifiction);
            }

            return result;
        }

        public async Task<bool> SaveOccassionToCelebrste()
        {
            bool result = false;
            IList<OccasionToCelebrate> lstoccasionToCelebrates = new List<OccasionToCelebrate>();
            var occassiontocelebrate =await _alertNotificationData.GetAllTodayCustomerOccasions();

            if (occassiontocelebrate.Count > 0)
            {
                foreach (var item in occassiontocelebrate)
                {
                    OccasionToCelebrate occasion = new OccasionToCelebrate()
                    {
                        CustomarNo=item.Customer,
                        CustomerName=item.Name,
                        DissChannel=item.DistrChannel,
                        Division=item.Division,
                        DOB= CustomConvertExtension.ObjectToDateTime(item.DOB),
                        NotificationDate=DateTime.Now,
                        FirsChildDOB= CustomConvertExtension.ObjectToDateTime(item.FirstChildDOB),
                        SecondChildDOB = CustomConvertExtension.ObjectToDateTime(item.SecondChildDOB),
                        ThirdChildDOB = CustomConvertExtension.ObjectToDateTime(item.ThirdChildDOB),
                        SpouseDOB = CustomConvertExtension.ObjectToDateTime(item.SpouseDOB)

                    };

                    lstoccasionToCelebrates.Add(occasion);
                }
                result= await _occasionToCelebrate.SaveOccasionToCelebrate(lstoccasionToCelebrates);
            }

            return result;
        }

        public async Task<bool> SavePaymnetFollowup()
        {
            bool result = false;
            IList<PaymentFollowup> lstpaymentFollowup = new List<PaymentFollowup>();
            var paymentFollowup = await _alertNotificationData.GetAllTodayPaymentFollowUp();

            if (lstpaymentFollowup.Count > 0)
            {
                foreach (var item in paymentFollowup)
                {
                    PaymentFollowup payment = new PaymentFollowup()
                    {
                        InvoiceAge=item.Age,
                        InvoiceDate=item.Date,
                        CustomarNo=item.CustomerNo,
                        CustomerName=item.CustomerName,
                        DayLimit=item.DayLimit,
                        InvoiceNo=item.InvoiceNo,
                        NotificationDate=DateTime.Now,
                    };

                    lstpaymentFollowup.Add(payment);
                }
                result = await _paymentFollowupService.SavePaymentFollowup(lstpaymentFollowup);
            }

            return result;
        }

        public async Task<bool> GetRPRSPaymnetFollowup()
        {
            var today = DateTime.Now;
            var dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
            var resultDateFormat = "dd MMM yyyy";
            var appUser = AppIdentity.AppUser;
            var customer = await _authService.GetDealerByUserId(appUser.UserId);
            var getPaymentFollowup = await _paymentFollowupService.GetToayPaymentFollowup(customer);

            var getCreditLimit = await _crossNotifictionService.GetTodayCreditLimitCrossNotifiction(customer);

            var result = getPaymentFollowup.Select(x =>
                                new PaymentFollowUpNotificationModel()
                                {
                                    CustomerNo = x.CustomarNo,
                                    CustomerName = x.CustomerName,
                                    InvoiceNo = x.InvoiceNo,
                                    //InvoiceDate = x.InvoiceDate.DateFormatDate(format: dateFormat).DateFormat(resultDateFormat),
                                    InvoiceAge = x.InvoiceAge,
                                    DayLimit = x.DayLimit
                                }).ToList();


            var resultRPRS = new List<PaymentFollowUpNotificationModel>();

            #region RPRS
            var dealersRPRS = getCreditLimit.Where(x => x.Channel == ConstantsValue.DistrbutionChannelDealer &&
                                                    x.PriceGroup == ConstantsValue.PriceGroupCreditBuyer).ToList();
            var dealerIdsRPRS = dealersRPRS.Select(x => x.CustomarNo.ToString()).Distinct().ToList();

            var rprsDayPolicy = await _odataCommonService.GetAllRPRSPoliciesAsync();

            foreach (var item in result.Where(x => dealerIdsRPRS.Contains(x.CustomerName)))
            {
                var dayCount = rprsDayPolicy.FirstOrDefault(x => CustomConvertExtension.ObjectToInt(item.DayLimit) >= x.FromDaysLimit &&
                                                CustomConvertExtension.ObjectToInt(item.DayLimit) <= x.ToDaysLimit)?.RPRSDays ?? 0;
                var dayNotifyCount = rprsDayPolicy.FirstOrDefault(x => CustomConvertExtension.ObjectToInt(item.DayLimit) >= x.FromDaysLimit &&
                                                CustomConvertExtension.ObjectToInt(item.DayLimit) <= x.ToDaysLimit)?.NotificationDays ?? 0;
                item.RPRSDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayCount).DateFormat(resultDateFormat);
                item.NotificationDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayNotifyCount).DateFormat(resultDateFormat);

                if (item.NotificationDate.DateFormatDate(resultDateFormat).Date == today.Date)
                    resultRPRS.Add(item);
            }
            #endregion
        }

        public async Task<bool> GetFastPayAndCarryPaymnetFollowup()
        {
            var appUser = AppIdentity.AppUser;
            var customer = await _authService.GetDealerByUserId(appUser.UserId);
            var getPaymentFollowup = await _paymentFollowupService.GetToayPaymentFollowup(customer);
            var getCreditLimit = await _crossNotifictionService.GetTodayCreditLimitCrossNotifiction(customer);
            var result = getPaymentFollowup.Select(x =>
                                new PaymentFollowUpNotificationModel()
                                {
                                    CustomerNo = x.CustomarNo,
                                    CustomerName = x.CustomerName,
                                    InvoiceNo = x.InvoiceNo,
                                    //InvoiceDate = x.InvoiceDate.DateFormatDate(format: dateFormat).DateFormat(resultDateFormat),
                                    InvoiceAge = x.InvoiceAge,
                                    DayLimit = x.DayLimit
                                }).ToList();
            var resultFastPayCarry = new List<PaymentFollowUpNotificationModel>();

            #region FastPayCarry
            var dealersFastPayCarry = getCreditLimit.Where(x => x.Channel == ConstantsValue.DistrbutionChannelDealer &&
                                                    (x.PriceGroup == ConstantsValue.PriceGroupCashBuyer ||
                                                    x.PriceGroup == ConstantsValue.PriceGroupFastPayCarry)).ToList();
            var dealerIdsFastPayCarry = dealersFastPayCarry.Select(x => x.CustomarNo.ToString()).Distinct().ToList();

            foreach (var item in result.Where(x => dealerIdsFastPayCarry.Contains(x.CustomerNo)))
            {
                //var dayCount = 5;
                var dayNotifyCount = 3;
                //item.RPRSDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayCount).DateFormat(resultDateFormat);
                item.NotificationDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayNotifyCount).DateFormat(resultDateFormat);
                item.PaymentFollowUpType = EnumPaymentFollowUpType.FastPayCarry;

                if (item.NotificationDate.DateFormatDate(resultDateFormat).Date == today.Date)
                    resultFastPayCarry.Add(item);
            }
            #endregion

        }

        public async Task<bool> GetLeadFollowUpReminderNotification()
        {
            var lead = _leadService.GetAllTodayFollowUpByUserIdForNotificationAsync(AppIdentity.AppUser.UserId);

            return true;
        }
    }

    public interface INotificationWorkerService
    {
        public Task<bool> SaveOccassionToCelebrste();
        public Task<bool> GetOccassionToCelebrste();
        public Task<bool> SaveCheckBounceNotification();
        public Task<bool> GetCheckBounceNotification();
        public Task<bool> SaveCreaditLimitNotification();
        public Task<bool> GetCreaditLimitNotification();
        public Task<bool> GetLeadFollowUpReminderNotification();
        public Task<bool> SavePaymnetFollowup();
        //public Task<bool> GetRPRSPaymnetFollowup();
        //public Task<bool> GetFastPayAndCarryPaymnetFollowup();


    }
}
