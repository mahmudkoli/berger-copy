using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.AlertNotification;
using Berger.Odata.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.AlertNotification
{
    public class NotificationWorkerService : INotificationWorkerService
    {
        public IOccasionToCelebrateService _occasionToCelebrate { get; set; }
        public ICreditLimitCrossNotifictionService _crossNotifictionService { get; set; }
        public IChequeBounceNotificationService _chequeBounceNotificationService { get; set; }
        public IPaymentFollowupService _paymentFollowupService { get; set; }
        public IAlertNotificationDataService _alertNotificationData { get; set; }
        public NotificationWorkerService(IOccasionToCelebrateService occasionToCelebrate,
            IAlertNotificationDataService alertNotificationData,
            ICreditLimitCrossNotifictionService crossNotifictionService,
            IChequeBounceNotificationService chequeBounceNotificationService,
            IPaymentFollowupService paymentFollowupService
            )
        {
            _occasionToCelebrate = occasionToCelebrate;
            _alertNotificationData = alertNotificationData;
            _crossNotifictionService = crossNotifictionService;
            _chequeBounceNotificationService = chequeBounceNotificationService;
            _paymentFollowupService = paymentFollowupService;
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

        //public async Task<bool> GetRPRSPaymnetFollowup()
        //{
        //    var getPaymentFollowup =await _paymentFollowupService.GetToayPaymentFollowup();
        //    var getCreditLimit =await _crossNotifictionService.GetTodayCreditLimitCrossNotifiction();
        //}

        //public async Task<bool> GetFastPayAndCarryPaymnetFollowup()
        //{
        //    var getPaymentFollowup = await _paymentFollowupService.GetToayPaymentFollowup();
        //    var getCreditLimit = await _crossNotifictionService.GetTodayCreditLimitCrossNotifiction();
        //}
    }

    public interface INotificationWorkerService
    {
        public Task<bool> SaveOccassionToCelebrste();
        public Task<bool> SaveCheckBounceNotification();
        public Task<bool> SaveCreaditLimitNotification();
        public Task<bool> SavePaymnetFollowup();
        //public Task<bool> GetRPRSPaymnetFollowup();
        //public Task<bool> GetFastPayAndCarryPaymnetFollowup();

    }
}
