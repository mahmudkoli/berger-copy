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
using Berger.Odata.Extensions;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Repositories;
using X.PagedList;

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
        private readonly IRepository<CreditLimitCrossNotification> _creditLimitCrossNotificationRepository;
        private readonly IRepository<ChequeBounceNotification> _chequeBounceNotificationRepository;
        private readonly IRepository<OccasionToCelebrateNotification> _occasionToCelebrateNotificationRepository;
        private readonly IAuthService _authService;
        private readonly ILeadService _leadService;
        private readonly IRepository<PaymentFollowupNotification> _paymentFollowupNotificationRepository;

        public NotificationWorkerService(IOccasionToCelebrateService occasionToCelebrate,
            IAlertNotificationDataService alertNotificationData,
            ICreditLimitCrossNotifictionService crossNotifictionService,
            IChequeBounceNotificationService chequeBounceNotificationService,
            IPaymentFollowupService paymentFollowupService,
            IAuthService authService,
            ILeadService leadService,
            IODataCommonService odataCommonService, 
            IRepository<PaymentFollowupNotification> paymentFollowupNotificationRepository, 
            IRepository<CreditLimitCrossNotification> creditLimitCrossNotificationRepository, 
            IRepository<ChequeBounceNotification> chequeBounceNotificationRepository, 
            IRepository<OccasionToCelebrateNotification> occasionToCelebrateNotificationRepository)
        {
            _occasionToCelebrate = occasionToCelebrate;
            _alertNotificationData = alertNotificationData;
            _crossNotifictionService = crossNotifictionService;
            _chequeBounceNotificationService = chequeBounceNotificationService;
            _paymentFollowupService = paymentFollowupService;
            _authService = authService;
            _leadService = leadService;
            _odataCommonService = odataCommonService;
            this._creditLimitCrossNotificationRepository = creditLimitCrossNotificationRepository;
            this._chequeBounceNotificationRepository = chequeBounceNotificationRepository;
            this._occasionToCelebrateNotificationRepository = occasionToCelebrateNotificationRepository;
            this._paymentFollowupNotificationRepository = paymentFollowupNotificationRepository;
        }

        public async Task<IList<ChequeBounceNotification>> GetCheckBounceNotification()
        {
            var appUser = AppIdentity.AppUser;
            //var customer = await _authService.GetDealerByUserId(appUser.UserId);
            //var checkBounce = await _chequeBounceNotificationService.GetChequeBounceNotification(customer);
            var tpDate = DateTime.Now;

            var checkBounce = _chequeBounceNotificationRepository.Where(p =>
                p.NotificationDate.Date == tpDate.Date &&
                (!appUser.PlantIdList.Any() || appUser.PlantIdList.Contains(p.Depot)) &&
                (!appUser.TerritoryIdList.Any() || appUser.TerritoryIdList.Contains(p.Territory)) &&
                (!appUser.ZoneIdList.Any() || appUser.ZoneIdList.Contains(p.Zone))).ToList();

            return checkBounce.ToList();

        }

        public async Task<IList<AppCreditLimitCrossNotificationModel>> GetCreaditLimitNotification()
        {
            var appUser = AppIdentity.AppUser;
            //var customer = await _authService.GetDealerByUserId(appUser.UserId);
            //var crossNotification = await _crossNotifictionService.GetTodayCreditLimitCrossNotifiction(customer);
            var tpDate = DateTime.Now;

            var crossNotification = _creditLimitCrossNotificationRepository.Where(p =>
                p.NotificationDate.Date == tpDate.Date && 
                (!appUser.PlantIdList.Any() || appUser.PlantIdList.Contains(p.Depot)) &&
                (!appUser.TerritoryIdList.Any() || appUser.TerritoryIdList.Contains(p.Territory)) &&
                (!appUser.ZoneIdList.Any() || appUser.ZoneIdList.Contains(p.Zone))).ToList();

            var groupData = crossNotification.GroupBy(x => new { x.CustomerNo }).ToList();

            var result = groupData.Select(x =>
            {
                var notifyModel = new AppCreditLimitCrossNotificationModel();
                notifyModel.CustomerNo = x.Key.CustomerNo.ToString();
                notifyModel.CustomerName = x.FirstOrDefault()?.CustomerName ?? string.Empty;
                notifyModel.CreditControlArea = x.FirstOrDefault()?.CreditControlArea ?? string.Empty;
                notifyModel.CreditLimit = x.GroupBy(g => new { g.CreditControlArea, CreditLimit = Convert.ToDecimal(g.CreditLimit) }).Sum(c => c.Key.CreditLimit);
                notifyModel.TotalDue = x.GroupBy(g => new { g.CreditControlArea, TotalDue = Convert.ToDecimal(g.TotalDue) }).Sum(c => c.Key.TotalDue);
                return notifyModel;
            }).ToList();

            result = result.Where(x => x.TotalDue > x.CreditLimit).ToList();


            return result;

        }

        public async Task<IList<OccasionToCelebrateNotification>> GetOccassionToCelebrste()
        {
            var appUser = AppIdentity.AppUser;
            //var customer = await _authService.GetDealerByUserId(appUser.UserId);

            //var checkBounce = await _occasionToCelebrate.GetOccasionToCelebrate(customer);
            var tpDate = DateTime.Now;

            var checkBounce = _occasionToCelebrateNotificationRepository.Where(p =>
                p.NotificationDate.Date == tpDate.Date &&
                (!appUser.PlantIdList.Any() || appUser.PlantIdList.Contains(p.Depot)) &&
                (!appUser.TerritoryIdList.Any() || appUser.TerritoryIdList.Contains(p.Territory)) &&
                (!appUser.ZoneIdList.Any() || appUser.ZoneIdList.Contains(p.Zone)) &&
                ((p.DOB.HasValue && (p.DOB.Value.Date.Month == tpDate.Month && p.DOB.Value.Day == tpDate.Day)) ||
                (p.FirsChildDOB.HasValue && (p.FirsChildDOB.Value.Date.Month == tpDate.Month && p.FirsChildDOB.Value.Day == tpDate.Day)) ||
                (p.SecondChildDOB.HasValue && (p.SecondChildDOB.Value.Date.Month == tpDate.Month && p.SecondChildDOB.Value.Day == tpDate.Day)) ||
                (p.ThirdChildDOB.HasValue && (p.ThirdChildDOB.Value.Date.Month == tpDate.Month && p.ThirdChildDOB.Value.Day == tpDate.Day)) ||
                (p.SpouseDOB.HasValue && (p.SpouseDOB.Value.Date.Month == tpDate.Month && p.SpouseDOB.Value.Day == tpDate.Day)))
            ).ToList();

            return checkBounce.ToList();
        }

        //public async Task<bool> SaveCheckBounceNotification()
        //{
        //    bool result = false;
        //    IList<ChequeBounceNotification> lstchequeBounceNotification = new List<ChequeBounceNotification>();
        //    var chequeBounceNotification = await _alertNotificationData.GetAllTodayCheckBounces();

        //    if (chequeBounceNotification.Count > 0)
        //    {
        //        foreach (var item in chequeBounceNotification)
        //        {
        //            ChequeBounceNotification chequeBounce = new ChequeBounceNotification()
        //            {
        //                Amount=Convert.ToDecimal(item.Amount),
        //                CreditControlArea=item.CreditControlArea,
        //                BankName=item.BankName,
        //                ChequeNo=item.ChequeNo,
        //                ClearDate=item.ClearDate,
        //                CustomarNo=item.CustomerNo,
        //                CustomerName=item.CustomerName,
        //                Depot=item.Depot,
        //                DocNumber=item.DocNumber,
        //                NotificationDate=DateTime.Now,


        //            };

        //            lstchequeBounceNotification.Add(chequeBounce);
        //        }
        //        result = await _chequeBounceNotificationService.SaveMultipleChequeBounceNotification(lstchequeBounceNotification);
        //    }

        //    return result;
        //}

        //public async Task<bool> SaveCreaditLimitNotification()
        //{
        //    bool result = false;
        //    IList<CreditLimitCrossNotifiction> lstcreditLimitCrossNotifiction = new List<CreditLimitCrossNotifiction>();
        //    var creditLimitCrossNotifiction = await _alertNotificationData.GetAllTodayCreditLimitCross();

        //    if (creditLimitCrossNotifiction.Count > 0)
        //    {
        //        foreach (var item in creditLimitCrossNotifiction)
        //        {
        //            CreditLimitCrossNotifiction creditLimit = new CreditLimitCrossNotifiction()
        //            {
        //                CreditControlArea=item.CreditControlArea,
        //                CreditLimit=item.CreditLimit.ToString(),
        //                CustomarNo=item.CustomerNo,
        //                Zone=item.CustZone,
        //                CustomerName=item.CustomerName,
        //                Division=item.Division,
        //                TotalDue=item.TotalDue.ToString(),
        //                NotificationDate=DateTime.Now,
        //                PriceGroup=item.PriceGroup
        //            };

        //            lstcreditLimitCrossNotifiction.Add(creditLimit);
        //        }
        //        result = await _crossNotifictionService.SaveMultipleCreditLimitCrossNotifiction(lstcreditLimitCrossNotifiction);
        //    }

        //    return result;
        //}

        //public async Task<bool> SaveOccassionToCelebrste()
        //{
        //    bool result = false;
        //    IList<OccasionToCelebrate> lstoccasionToCelebrates = new List<OccasionToCelebrate>();
        //    var occassiontocelebrate =await _alertNotificationData.GetAllTodayCustomerOccasions();

        //    if (occassiontocelebrate.Count > 0)
        //    {
        //        foreach (var item in occassiontocelebrate)
        //        {
        //            OccasionToCelebrate occasion = new OccasionToCelebrate()
        //            {
        //                CustomarNo=item.Customer,
        //                CustomerName=item.Name,
        //                DissChannel=item.DistrChannel,
        //                Division=item.Division,
        //                DOB= CustomConvertExtension.ObjectToDateTime(item.DOB),
        //                NotificationDate=DateTime.Now,
        //                FirsChildDOB= CustomConvertExtension.ObjectToDateTime(item.FirstChildDOB),
        //                SecondChildDOB = CustomConvertExtension.ObjectToDateTime(item.SecondChildDOB),
        //                ThirdChildDOB = CustomConvertExtension.ObjectToDateTime(item.ThirdChildDOB),
        //                SpouseDOB = CustomConvertExtension.ObjectToDateTime(item.SpouseDOB)

        //            };

        //            lstoccasionToCelebrates.Add(occasion);
        //        }
        //        result= await _occasionToCelebrate.SaveOccasionToCelebrate(lstoccasionToCelebrates);
        //    }

        //    return result;
        //}

        //public async Task<bool> SavePaymnetFollowup()
        //{
        //    bool result = false;
        //    IList<PaymentFollowup> lstpaymentFollowup = new List<PaymentFollowup>();
        //    var paymentFollowup = await _alertNotificationData.GetAllTodayPaymentFollowUp();

        //    if (lstpaymentFollowup.Count > 0)
        //    {
        //        foreach (var item in paymentFollowup)
        //        {
        //            PaymentFollowup payment = new PaymentFollowup()
        //            {
        //                InvoiceAge=item.Age,
        //                InvoiceDate=item.PostingDate,
        //                CustomarNo=item.CustomerNo,
        //                CustomerName=item.CustomerName,
        //                DayLimit=item.DayLimit,
        //                InvoiceNo=item.InvoiceNo,
        //                NotificationDate=DateTime.Now,
        //            };

        //            lstpaymentFollowup.Add(payment);
        //        }
        //        result = await _paymentFollowupService.SavePaymentFollowup(lstpaymentFollowup);
        //    }

        //    return result;
        //}

        public async Task<IList<PaymentFollowUpNotificationModel>> GetRPRSPaymnetFollowupBk()
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
                                    InvoiceDate = x.PostingDate.ToString(),
                                    InvoiceAge = x.InvoiceAge.ToString(),
                                    DayLimit = x.DayLimit.ToString()
                                }).ToList();


            var resultRPRS = new List<PaymentFollowUpNotificationModel>();

            var dealersRPRS = getCreditLimit.ToList();
            var dealerIdsRPRS = dealersRPRS.Select(x => x.CustomerNo.ToString()).Distinct().ToList();

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

            return resultRPRS;
        }


        public async Task<IEnumerable<PaymentFollowUpNotificationModel>> GetFastPayAndCarryPaymnetFollowup()
        {

            var appUser = AppIdentity.AppUser;
            var tpDate = DateTime.Now;

            return await _paymentFollowupNotificationRepository.Where(x =>
                    x.IsFastPayCarryPayment && x.NotificationDate.Date == tpDate.Date &&
                    (!appUser.PlantIdList.Any() || appUser.PlantIdList.Contains(x.Depot)) &&
                    (!appUser.TerritoryIdList.Any() || appUser.TerritoryIdList.Contains(x.Territory)) &&
                    (!appUser.ZoneIdList.Any() || appUser.ZoneIdList.Contains(x.Zone)))
                .Select(x => new PaymentFollowUpNotificationModel
                {
                    CustomerNo = x.CustomarNo,
                    CustomerName = x.CustomerName,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.PostingDate.ToString(),
                    InvoiceAge = x.InvoiceAge.ToString(),
                    DayLimit = x.DayLimit.ToString(),
                    InvoiceValue=x.InvoiceValue.ToString(),
                    PaymentFollowUpType = EnumPaymentFollowUpType.FastPayCarry,
                    RPRSDate = x.NotificationDate.AddDays(2).ToString("dd-MM-yyyy")
                }).ToListAsync();

        }
        
        public async Task<IList<PaymentFollowUpNotificationModel>> GetRPRSPaymnetFollowup()
        {

            var appUser = AppIdentity.AppUser;
            var tpDate = DateTime.Now;

            return await _paymentFollowupNotificationRepository.Where(x =>
                    x.IsRprsPayment && x.NotificationDate.Date == tpDate.Date &&
                    (!appUser.PlantIdList.Any() || appUser.PlantIdList.Contains(x.Depot)) &&
                    (!appUser.TerritoryIdList.Any() || appUser.TerritoryIdList.Contains(x.Territory)) &&
                    (!appUser.ZoneIdList.Any() || appUser.ZoneIdList.Contains(x.Zone)))
                .Select(x => new PaymentFollowUpNotificationModel
                {
                    CustomerNo = x.CustomarNo,
                    CustomerName = x.CustomerName,
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = x.PostingDate.ToString(),
                    InvoiceAge = x.InvoiceAge.ToString(),
                    DayLimit = x.DayLimit.ToString(),
                    InvoiceValue=x.InvoiceValue.ToString(),
                    PaymentFollowUpType = EnumPaymentFollowUpType.RPRS,
                    RPRSDate = x.NotificationDate.AddDays(3).ToString("dd-MM-yyyy")
                }).ToListAsync();

        }



        public async Task<IEnumerable<PaymentFollowUpNotificationModel>> GetFastPayAndCarryPaymnetFollowupBk()
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
                                    InvoiceDate = x.PostingDate.ToString(),
                                    InvoiceAge = x.InvoiceAge.ToString(),
                                    DayLimit = x.DayLimit.ToString()
                                }).ToList();
            var resultFastPayCarry = new List<PaymentFollowUpNotificationModel>();

            var dealersFastPayCarry = getCreditLimit.Where(x=>
                                                    (x.PriceGroup == ConstantsValue.PriceGroupCashBuyer ||
                                                    x.PriceGroup == ConstantsValue.PriceGroupFastPayCarry)).ToList();

            var dealerIdsFastPayCarry = dealersFastPayCarry.Select(x => x.CustomerNo.ToString()).Distinct().ToList();

            foreach (var item in result.Where(x => dealerIdsFastPayCarry.Contains(x.CustomerNo)))
            {
                var dayCount = 5;
                var dayNotifyCount = 3;
                item.RPRSDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayCount).DateFormat(resultDateFormat);
                item.NotificationDate = item.InvoiceDate.DateFormatDate(resultDateFormat).AddDays(dayNotifyCount).DateFormat(resultDateFormat);
                item.PaymentFollowUpType = EnumPaymentFollowUpType.FastPayCarry;

                if (item.NotificationDate.DateFormatDate(resultDateFormat).Date == today.Date)
                    resultFastPayCarry.Add(item);
            }
            return resultFastPayCarry;
        }

        public async Task<IList<AppLeadFollowUpNotificationModel>> GetLeadFollowUpReminderNotification()
        {
            var lead = await _leadService.GetAllTodayFollowUpByUserIdForNotificationAsync();

            return lead;
        }

    }

    public interface INotificationWorkerService
    {
        //public Task<bool> SaveOccassionToCelebrste();
        public Task<IList<OccasionToCelebrateNotification>> GetOccassionToCelebrste();
        //public Task<bool> SaveCheckBounceNotification();
        public Task<IList<ChequeBounceNotification>> GetCheckBounceNotification();
        //public Task<bool> SaveCreaditLimitNotification();
        public Task<IList<AppCreditLimitCrossNotificationModel>> GetCreaditLimitNotification();
        public Task<IList<AppLeadFollowUpNotificationModel>> GetLeadFollowUpReminderNotification();
        //public Task<bool> SavePaymnetFollowup();
        public Task<IList<PaymentFollowUpNotificationModel>> GetRPRSPaymnetFollowup();
        Task<IEnumerable<PaymentFollowUpNotificationModel>> GetFastPayAndCarryPaymnetFollowup();


    }
}
