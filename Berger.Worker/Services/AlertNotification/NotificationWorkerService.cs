using Berger.Common.Extensions;
using Berger.Data.MsfaEntity.AlertNotification;
using Berger.Odata.Common;
using Berger.Odata.Model;
using Berger.Odata.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Berger.Odata.Extensions;
using System.Globalization;
using Berger.Worker.Repositories;
using Berger.Data.MsfaEntity.SAPTables;
using Microsoft.Extensions.Logging;

namespace Berger.Worker.Services.AlertNotification
{
    public class NotificationWorkerService : INotificationWorkerService
    {
        private readonly IOccasionToCelebrateService _occasionToCelebrate;
        private readonly ICreditLimitCrossNotifictionService _crossNotifictionService;
        private readonly IChequeBounceNotificationService _chequeBounceNotificationService;
        private readonly IPaymentFollowupService _paymentFollowupService;
        private readonly IAlertNotificationDataService _alertNotificationDataService;


        private readonly IApplicationRepository<ChequeBounceNotification> _chequeBounceNotificationRepo;
        private readonly IApplicationRepository<DealerInfo> _dealerInfoRepo;
        private readonly ILogger<NotificationWorkerService> _logger;

        public NotificationWorkerService(IOccasionToCelebrateService occasionToCelebrate,
            IAlertNotificationDataService alertNotificationDataService,
            ICreditLimitCrossNotifictionService crossNotifictionService,
            IChequeBounceNotificationService chequeBounceNotificationService,
            IPaymentFollowupService paymentFollowupService,
            IApplicationRepository<ChequeBounceNotification> chequeBounceNotificationRepo,
            IApplicationRepository<DealerInfo> dealerInfoRepo, 
            ILogger<NotificationWorkerService> logger
            )
        {
            _occasionToCelebrate = occasionToCelebrate;
            _alertNotificationDataService = alertNotificationDataService;
            _crossNotifictionService = crossNotifictionService;
            _chequeBounceNotificationService = chequeBounceNotificationService;
            _paymentFollowupService = paymentFollowupService;
            _chequeBounceNotificationRepo = chequeBounceNotificationRepo;
            _dealerInfoRepo = dealerInfoRepo;
            _logger = logger;
        }


        public async Task<bool> SaveCheckBounceNotification()
        {
            try
            {
                var today = DateTime.Now;
                var chequeBounceODataNotifications = await _alertNotificationDataService.GetAllTodayCheckBounces();

                var chequeBounceNotifications = (from cbn in chequeBounceODataNotifications
                                                 join di in _dealerInfoRepo.GetAll()
                                                 on new { BusinessArea = cbn.BusinessArea, Territory = cbn.Territory, CustomerNo = cbn.CustomerNo }
                                                 equals new { BusinessArea = di.BusinessArea, Territory = di.Territory, CustomerNo = di.CustomerNo }
                                                 into cbninfoleftjoin
                                                 from cbninfo in cbninfoleftjoin.DefaultIfEmpty()
                                                 select new ChequeBounceNotification
                                                 {
                                                     Id = Guid.NewGuid(),
                                                     Depot = cbninfo.BusinessArea,
                                                     Territory = cbninfo.Territory,
                                                     Zone = cbninfo.CustZone,
                                                     CustomarNo = cbninfo.CustomerNo,
                                                     CustomerName = cbninfo.CustomerName,
                                                     ChequeNo = cbn.ChequeNo,
                                                     Amount = CustomConvertExtension.ObjectToDecimal(cbn.Amount),
                                                     NotificationDate = today
                                                 }).ToList();

                if (!chequeBounceNotifications.Any())
                    await _chequeBounceNotificationRepo.DeleteAsync(x => x.NotificationDate.Date < today.Date);

                if (chequeBounceNotifications.Any()) 
                    await _chequeBounceNotificationRepo.CreateListAsync(chequeBounceNotifications);
                
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SaveCreaditLimitNotification()
        {
            bool result = false;

            try
            {
                IList<CreditLimitCrossNotification> lstcreditLimitCrossNotifiction = new List<CreditLimitCrossNotification>();
                var creditLimitCrossNotifiction = await _alertNotificationDataService.GetAllTodayCreditLimitCross();

                if (creditLimitCrossNotifiction.Count > 0)
                {
                    foreach (var item in creditLimitCrossNotifiction)
                    {
                        CreditLimitCrossNotification creditLimit = new CreditLimitCrossNotification()
                        {
                            CreditControlArea = item.CreditControlArea,
                            CreditLimit = item.CreditLimit.ToString(),
                            CustomarNo = item.CustomerNo,
                            Zone = item.CustZone,
                            CustomerName = item.CustomerName,
                            Division = item.Division,
                            TotalDue = item.TotalDue.ToString(),
                            NotificationDate = DateTime.Now,
                            PriceGroup = item.PriceGroup
                        };

                        lstcreditLimitCrossNotifiction.Add(creditLimit);
                    }
                    result = await _crossNotifictionService.SaveMultipleCreditLimitCrossNotifiction(lstcreditLimitCrossNotifiction);
                }

            }
            catch (Exception)
            {


            }
            return result;

        }

        public async Task<bool> SaveOccassionToCelebrste()
        {
            bool result = false;

            try
            {
                IList<OccasionToCelebrateNotification> lstoccasionToCelebrates = new List<OccasionToCelebrateNotification>();
                var occassiontocelebrate = await _alertNotificationDataService.GetAllTodayCustomerOccasions();

                if (occassiontocelebrate.Count > 0)
                {
                    foreach (var item in occassiontocelebrate)
                    {
                        OccasionToCelebrateNotification occasion = new OccasionToCelebrateNotification()
                        {
                            CustomarNo = item.Customer,
                            CustomerName = item.Name,
                            DissChannel = item.DistrChannel,
                            Division = item.Division,
                            //DOB= string.IsNullOrEmpty(item.DOB)?null : item.DOB.DateFormatDate(),
                            DOB = convertDate(item.DOB),
                            NotificationDate = DateTime.Now,
                            FirsChildDOB = convertDate(item.FirstChildDOB),
                            SecondChildDOB = convertDate(item.SecondChildDOB),
                            ThirdChildDOB = convertDate(item.ThirdChildDOB),
                            SpouseDOB = convertDate(item.SpouseDOB)

                        };

                        lstoccasionToCelebrates.Add(occasion);
                    }
                    result = await _occasionToCelebrate.SaveOccasionToCelebrate(lstoccasionToCelebrates);
                }

            }
            catch (Exception)
            {


            }
            return result;

        }

        public async Task<bool> SavePaymnetFollowup()
        {
            bool result = false;

            try
            {
                IList<PaymentFollowupNotification> lstpaymentFollowup = new List<PaymentFollowupNotification>();
                var paymentFollowup = await _alertNotificationDataService.GetAllTodayPaymentFollowUp();

                if (paymentFollowup.Count > 0)
                {
                    foreach (var item in paymentFollowup)
                    {
                        PaymentFollowupNotification payment = new PaymentFollowupNotification()
                        {
                            InvoiceAge = item.Age,
                            InvoiceDate = Convert.ToDateTime(item.Date),
                            PostingDate = Convert.ToDateTime(item.PostingDate),
                            CustomarNo = item.CustomerNo,
                            CustomerName = item.CustomerName,
                            DayLimit = item.DayLimit,
                            InvoiceNo = item.InvoiceNo,
                            NotificationDate = DateTime.Now,
                        };

                        lstpaymentFollowup.Add(payment);
                    }
                    result = await _paymentFollowupService.SavePaymentFollowup(lstpaymentFollowup);
                }

            }
            catch (Exception ex)
            {


            }
            return result;

        }


        private DateTime? convertDate(string date)
        {
            DateTime result = DateTime.Now;
            try
            {

                if (date == "" || date == null)
                {
                    return null;
                }
                result = date.DateFormatDate();
            }
            catch (Exception ex)
            {
                result = DateTime.MinValue;
            }
            return result;

        }


    }

    public interface INotificationWorkerService
    {
        public Task<bool> SaveOccassionToCelebrste();
        public Task<bool> SaveCheckBounceNotification();
        public Task<bool> SaveCreaditLimitNotification();
        public Task<bool> SavePaymnetFollowup();


    }
}
