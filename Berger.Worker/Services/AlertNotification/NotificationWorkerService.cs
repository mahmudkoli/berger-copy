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
using X.PagedList;

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
        private readonly IApplicationRepository<OccasionToCelebrateNotification> _occasionToCelebrateRepository;
        private readonly ILogger<NotificationWorkerService> _logger;
        private readonly IApplicationRepository<CreditLimitCrossNotification> _creditLimitCrossNotificationRepository;
        private readonly IApplicationRepository<PaymentFollowupNotification> _paymentFollowupNotificationRepository;
        private readonly IApplicationRepository<RPRSPolicy> _rprsPolicyRepository;

        public NotificationWorkerService(IOccasionToCelebrateService occasionToCelebrate,
            IAlertNotificationDataService alertNotificationDataService,
            ICreditLimitCrossNotifictionService crossNotifictionService,
            IChequeBounceNotificationService chequeBounceNotificationService,
            IPaymentFollowupService paymentFollowupService,
            IApplicationRepository<ChequeBounceNotification> chequeBounceNotificationRepo,
            IApplicationRepository<DealerInfo> dealerInfoRepo,
            IApplicationRepository<OccasionToCelebrateNotification> occasionToCelebrateRepository,
            ILogger<NotificationWorkerService> logger,
            IApplicationRepository<CreditLimitCrossNotification> creditLimitCrossNotificationRepository, IApplicationRepository<PaymentFollowupNotification> paymentFollowupNotificationRepository, IApplicationRepository<RPRSPolicy> rprsPolicyRepository)
        {
            _occasionToCelebrate = occasionToCelebrate;
            _alertNotificationDataService = alertNotificationDataService;
            _crossNotifictionService = crossNotifictionService;
            _chequeBounceNotificationService = chequeBounceNotificationService;
            _paymentFollowupService = paymentFollowupService;
            _chequeBounceNotificationRepo = chequeBounceNotificationRepo;
            _dealerInfoRepo = dealerInfoRepo;
            _occasionToCelebrateRepository = occasionToCelebrateRepository;
            _logger = logger;
            _creditLimitCrossNotificationRepository = creditLimitCrossNotificationRepository;
            _paymentFollowupNotificationRepository = paymentFollowupNotificationRepository;
            _rprsPolicyRepository = rprsPolicyRepository;
        }


        public async Task<bool> SaveCheckBounceNotification()
        {
            try
            {
                var today = DateTime.Now;
                var chequeBounceODataNotifications = await _alertNotificationDataService.GetAllTodayCheckBounces();



                var distinctPlan = chequeBounceODataNotifications.Select(x => x.BusinessArea).Distinct().ToList();
                var distinctCustomer = chequeBounceODataNotifications.Select(x => x.CustomerNo).Distinct().ToList();
                var distinctTerritory = chequeBounceODataNotifications.Select(x => x.Territory).Distinct().ToList();

                var dbRecord = await _dealerInfoRepo.Where(x => distinctPlan
                    .Contains(x.BusinessArea) && distinctCustomer.Contains(x.CustomerNo) &&
                                                                distinctTerritory.Contains(x.Territory)).Select(x => new
                                                                {
                                                                    x.CustomerNo,
                                                                    x.CustZone,
                                                                    x.Territory,
                                                                    x.BusinessArea,
                                                                    x.CustomerName
                                                                }).Distinct().ToListAsync();


                var chequeBounceNotifications = (from cbn in chequeBounceODataNotifications
                                                 join di in dbRecord
                                                 on new { BusinessArea = cbn.BusinessArea, Territory = cbn.Territory, CustomerNo = cbn.CustomerNo }
                                                 equals new { BusinessArea = di.BusinessArea, Territory = di.Territory, CustomerNo = di.CustomerNo }
                                                 into cbninfoleftjoin
                                                 from cbninfo in cbninfoleftjoin.DefaultIfEmpty()
                                                 select new ChequeBounceNotification
                                                 {
                                                     Depot = cbninfo.BusinessArea,
                                                     Territory = cbninfo.Territory,
                                                     Zone = cbninfo.CustZone,
                                                     CustomarNo = cbninfo.CustomerNo,
                                                     CustomerName = cbninfo.CustomerName,
                                                     ChequeNo = cbn.ChequeNo,
                                                     Amount = CustomConvertExtension.ObjectToDecimal(cbn.Amount),
                                                     NotificationDate = today
                                                 }).Distinct().ToList();

                chequeBounceNotifications.ForEach(x => x.Id = Guid.NewGuid());

                if (chequeBounceNotifications.Any())
                {
                    await _chequeBounceNotificationRepo.DeleteAsync(x => x.NotificationDate < today);
                    _logger.LogInformation("chequeBounce  deleting ....");
                }

                if (chequeBounceNotifications.Any())
                {
                    await _chequeBounceNotificationRepo.CreateListAsync(chequeBounceNotifications);
                    _logger.LogInformation("chequeBounce  insert ....");

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "occasion error");
                return false;
            }

            return true;
        }

        public async Task<bool> SaveCreaditLimitNotification()
        {
            bool result = false;

            try
            {
                var lstcreditLimitCrossNotifiction = new List<CreditLimitCrossNotification>();

                _logger.LogInformation("creditLimitCrossNotification  fetching data from odata ....");

                var creditLimitCrossNotifiction = await _alertNotificationDataService.GetAllTodayCreditLimitCross();

                DateTime today = DateTime.Now;
                if (creditLimitCrossNotifiction.Count > 0)
                {
                    foreach (var item in creditLimitCrossNotifiction)
                    {

                        CreditLimitCrossNotification creditLimit = new CreditLimitCrossNotification()
                        {
                            Zone = item.CustZone,
                            CreditControlArea = item.CreditControlArea,
                            Depot = item.BusinessArea,
                            CreditLimit = CustomConvertExtension.ObjectToDecimal(item.CreditLimit),
                            CustomerNo = item.CustomerNo,
                            CustomerName = item.CustomerName,
                            TotalDue = CustomConvertExtension.ObjectToDecimal(item.TotalDue),
                            NotificationDate = today,
                            Id = Guid.NewGuid(),
                            Territory = item.Territory,
                            PriceGroup = item.PriceGroup
                        };

                        lstcreditLimitCrossNotifiction.Add(creditLimit);
                    }


                    if (lstcreditLimitCrossNotifiction.Any())
                    {
                        await _creditLimitCrossNotificationRepository.DeleteAsync(x => x.NotificationDate < today);
                        _logger.LogInformation("creditLimitCrossNotification  deleting ....");
                    }

                    if (lstcreditLimitCrossNotifiction.Any())
                    {
                        await _creditLimitCrossNotificationRepository.CreateListAsync(lstcreditLimitCrossNotifiction);
                        _logger.LogInformation("creditLimitCrossNotification  insert ....");

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "creditLimitCrossNotification error");
                return false;

            }

            _logger.LogInformation("creditLimitCrossNotification done");
            return true;

        }

        public async Task<bool> SaveOccasionToCelebrate()
        {

            try
            {

                _logger.LogInformation("Fetching occasion from odata...");

                var occassiontocelebrate = await _alertNotificationDataService.GetAllTodayCustomerOccasions();

                var notificationDate = DateTime.Now;

                var distinctPlan = occassiontocelebrate.Select(x => x.Plant).Distinct().ToList();
                var distinctCustomer = occassiontocelebrate.Select(x => x.Customer).Distinct().ToList();

                var dbRecord = await _dealerInfoRepo.Where(x => distinctPlan
                    .Contains(x.BusinessArea) && distinctCustomer.Contains(x.CustomerNo)).Select(x => new
                    {
                        x.CustomerNo,
                        x.CustZone,
                        x.Territory,
                        x.BusinessArea
                    }).Distinct().ToListAsync();

                var data = (from cbn in occassiontocelebrate
                            join di in dbRecord
                               on new { BusinessArea = cbn.Plant, CustomerNo = cbn.Customer }
                               equals new { BusinessArea = di.BusinessArea, CustomerNo = di.CustomerNo }
                               into cbninfoleftjoin
                            from cbninfo in cbninfoleftjoin.DefaultIfEmpty()
                            select new OccasionToCelebrateNotification
                            {
                                CustomarNo = cbn.Customer,
                                CustomerName = cbn.Name,
                                Depot = cbn.Plant,
                                DOB = convertDate(cbn.DOB),
                                NotificationDate = notificationDate,
                                FirsChildDOB = convertDate(cbn.FirstChildDOB),
                                SecondChildDOB = convertDate(cbn.SecondChildDOB),
                                ThirdChildDOB = convertDate(cbn.ThirdChildDOB),
                                SpouseDOB = convertDate(cbn.SpouseDOB),
                                Territory = cbninfo?.Territory,
                                Zone = cbninfo?.CustZone
                            }).Distinct().ToList();



                data.ForEach(x => x.Id = Guid.NewGuid());

                if (data.Any())
                {
                    _logger.LogInformation("Delete existing occasion data...");
                    await _occasionToCelebrateRepository.DeleteAsync(x => x.NotificationDate <= notificationDate);

                }

                if (data.Any())
                {
                    _logger.LogInformation("Inserting occasion data...");
                    await _occasionToCelebrateRepository.CreateListAsync(data);

                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "occasion error");

                return false;


            }
            _logger.LogInformation("occasion scheduler work done..");

            return true;

        }

        public async Task<bool> SavePaymentFollowup()
        {

            try
            {
                IList<PaymentFollowupNotification> lstpaymentFollowup = new List<PaymentFollowupNotification>();

                var dbDealerRecord = await _dealerInfoRepo.Where(x => x.CustomerNo != "").Select(x => new
                {
                    x.CustZone,
                    x.BusinessArea,
                    x.Territory,
                    x.CustomerNo,
                    x.CustomerName,
                    x.PriceGroup
                }).Distinct().ToListAsync();

                var rprsDayPolicy = (await _rprsPolicyRepository.GetAllAsync()).ToList();

                foreach (string customerNo in dbDealerRecord.Select(x => x.CustomerNo).Distinct().OrderBy(o => o).ToList())
                {
                    var paymentFollowup = await _alertNotificationDataService.GetAllTodayPaymentFollowUp(customerNo);


                    var data = (from cbn in paymentFollowup
                                join di in dbDealerRecord
                            on new { CustomerNo = cbn.CustomerNo }
                            equals new { CustomerNo = di.CustomerNo }
                            into cbninfoleftjoin
                                from cbninfo in cbninfoleftjoin.DefaultIfEmpty()
                                select new PaymentFollowupNotification
                                {
                                    InvoiceAge = CustomConvertExtension.ObjectToInt(cbn.Age),
                                    PostingDate = Convert.ToDateTime(cbn.PostingDate),
                                    CustomarNo = cbn.CustomerNo,
                                    CustomerName = cbninfo.CustomerName,
                                    DayLimit = CustomConvertExtension.ObjectToInt(cbn.DayLimit),
                                    InvoiceNo = cbn.InvoiceNo,
                                    Depot = cbninfo.BusinessArea,
                                    Territory = cbninfo.Territory,
                                    Zone = cbninfo.CustZone,
                                    PriceGroup = cbninfo.PriceGroup,
                                    IsRprsPayment = cbninfo.PriceGroup == ConstantsValue.PriceGroupCreditBuyer,
                                    IsFastPayCarryPayment = cbninfo.PriceGroup == ConstantsValue.PriceGroupCashBuyer ||
                                                            cbninfo.PriceGroup == ConstantsValue.PriceGroupFastPayCarry,
                                    InvoiceValue = CustomConvertExtension.ObjectToInt(cbn.Amount),
                                }).Distinct().ToList();

                    foreach (var item in data)
                    {
                        int dayCount = 0;
                        item.Id = Guid.NewGuid();
                        if (item.IsRprsPayment)
                        {
                            dayCount = rprsDayPolicy
                               .FirstOrDefault(x => item.DayLimit >= x.FromDaysLimit && item.DayLimit <= x.ToDaysLimit)
                               ?.NotificationDays ?? 0;
                        }
                        else if (item.IsFastPayCarryPayment)
                        {
                            dayCount = 3;
                        }

                        item.NotificationDate = item.PostingDate.Value.AddDays(dayCount - 1);
                    }


                    if (data.Any())
                    {
                        _logger.LogInformation("Delete existing PaymentFollowup data...");
                        await _paymentFollowupNotificationRepository.DeleteAsync(x => x.CustomarNo == customerNo);
                    }


                    if (data.Any())
                    {
                        _logger.LogInformation("Inserting PaymentFollowup data...");
                        await _paymentFollowupNotificationRepository.CreateListAsync(data);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PaymentFollowup error.");
                return false;

            }
            return true;

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
        public Task<bool> SaveOccasionToCelebrate();
        public Task<bool> SaveCheckBounceNotification();
        public Task<bool> SaveCreaditLimitNotification();
        public Task<bool> SavePaymentFollowup();


    }
}
