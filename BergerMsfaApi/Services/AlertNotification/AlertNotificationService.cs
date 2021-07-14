using AutoMapper;
using Berger.Common.Enumerations;
using Berger.Data.MsfaEntity.Setup;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Notification;
using BergerMsfaApi.Repositories;
using BergerMsfaApi.Services.Setup.Interfaces;
using BergerMsfaApi.Services.Notification.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;
using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using BergerMsfaApi.Services.Interfaces;
using Berger.Odata.Services;
using Microsoft.Extensions.Logging;
using Berger.Odata.Model;
using Berger.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using BergerMsfaApi.Services.Menus.Interfaces;

namespace BergerMsfaApi.Services.AlertNotification
{
    public class AlertNotificationService : IAlertNotificationService
    {
        private readonly FCMSettingsModel _settings;
        private readonly ILogger<AlertNotificationService> _logger;
        private ILeadService _leadService;
        private INotificationWorkerService _notificationWorkerService;
        private IODataNotificationService _oDataNotificationService;
        private readonly IAuthService _authService;
        private readonly IMenuService _menuService;
        private readonly string _rootPath;


        public AlertNotificationService(IOptions<FCMSettingsModel> settings,
            ILogger<AlertNotificationService> logger,
            ILeadService leadService,
            IODataNotificationService oDataNotificationService,
            IAuthService authService,
            IWebHostEnvironment env,
            IMenuService menuService,
            INotificationWorkerService notificationWorkerService)
        {
            this._settings = settings.Value;
            this._logger = logger;
            this._leadService = leadService;
            this._oDataNotificationService = oDataNotificationService;
            this._authService = authService;
            this._rootPath = env.WebRootPath;
            _menuService = menuService;
            _notificationWorkerService = notificationWorkerService;
        }

        private async Task<IList<AppAlertNotificationModel>> GetLeadFollowUpNotification()
        {
            var notifications = new List<AppAlertNotificationModel>();

            try
            {
                var leadFollowUps = await _notificationWorkerService.GetLeadFollowUpReminderNotification();

                foreach (var leadFollowUp in leadFollowUps)
                {
                    var title = $"Today you have lead follow up.";
                    var body = $"Lead follow up - Depot: {leadFollowUp.Depot}, Territory: {leadFollowUp.Territory}, " +
                        $"Zone: {leadFollowUp.Zone}, Code: {leadFollowUp.Code}, " +
                        $"Project Name: {leadFollowUp.ProjectName}, Project Address: {leadFollowUp.ProjectAddress}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Notification failed for Lead FollowUp to send for User Id: {userId}");
                //LoggerExtension.ToWriteLog($"Notification failed for Lead FollowUp to send for User Id ({userId}): {ex}", _rootPath);

                _logger.LogError(ex, $"Notification failed for Lead FollowUp to send ");
                LoggerExtension.ToWriteLog($"Notification failed for Lead FollowUp to send): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppAlertNotificationModel>> GetCheckBounceNotification()
        {
            var notifications = new List<AppAlertNotificationModel>();

            try
            {
                var checkBounces = await _notificationWorkerService.GetCheckBounceNotification();

                foreach (var checkBounce in checkBounces)
                {
                    var title = $"Today you have check bounce.";
                    var body = $"Check Bounce - Customer No: {checkBounce.CustomarNo}, Customer Name: {checkBounce.CustomerName}, " +
                        $"Credit Control Area: {checkBounce.CreditControlArea}, Amount: {checkBounce.Amount}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Notification failed for Check Bounce to send for User Id: {userId}");
                //LoggerExtension.ToWriteLog($"Notification failed for Check Bounce to send for User Id ({userId}): {ex}", _rootPath);

                _logger.LogError(ex, $"Notification failed for Check Bounce to send ");
                LoggerExtension.ToWriteLog($"Notification failed for Check Bounce to send): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppAlertNotificationModel>> GetCreditLimitCrossNotification()
        {
            var notifications = new List<AppAlertNotificationModel>();

            try
            {
                var creditLimitCrosses = await _notificationWorkerService.GetCreaditLimitNotification();

                foreach (var creditLimitCross in creditLimitCrosses)
                {
                    var title = $"Today you have Credit Limit Cross.";
                    var body = $"Credit Limit Cross - Customer No: {creditLimitCross.CustomarNo}, Customer Name: {creditLimitCross.CustomerName}, " +
                        $"Credit Control Area: {creditLimitCross.CreditControlArea}, Credit Limit: {creditLimitCross.CreditLimit}, " +
                        $"Total Due: {creditLimitCross.TotalDue}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Notification failed for Credit Limit Cross to send for User Id: {userId}");
                //LoggerExtension.ToWriteLog($"Notification failed for Credit Limit Cross to send for User Id ({userId}): {ex}", _rootPath);

                _logger.LogError(ex, $"Notification failed for Credit Limit Cross to send");
                LoggerExtension.ToWriteLog($"Notification failed for Credit Limit Cross to send): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppAlertNotificationModel>> GetPaymentFollowUpNotification()
        {
            var notifications = new List<AppAlertNotificationModel>();

            try
            {
                var paymentFollowUps = await _notificationWorkerService.GetRPRSPaymnetFollowup();

                foreach (var paymentFollowUp in paymentFollowUps.Where(x => x.PaymentFollowUpType == EnumPaymentFollowUpType.RPRS))
                {
                    var title = $"Today you have RPRS Follow Up.";
                    var body = $"RPRS Follow Up - Customer No: {paymentFollowUp.CustomerNo}, Customer Name: {paymentFollowUp.CustomerName}, " +
                        $"Invoice No: {paymentFollowUp.InvoiceNo}, Invoice Date: {paymentFollowUp.InvoiceDate}, " +
                        $"Invoice Age: {paymentFollowUp.InvoiceAge}, RPRS Date: {paymentFollowUp.RPRSDate}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Notification failed for Payment FollowUp to send for User Id: {userId}");
                //LoggerExtension.ToWriteLog($"Notification failed for Payment FollowUp to send for User Id ({userId}): {ex}", _rootPath);

                _logger.LogError(ex, $"Notification failed for Payment FollowUp to send");
                LoggerExtension.ToWriteLog($"Notification failed for Payment FollowUp to send): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppAlertNotificationModel>> GetFastPayCarryFollowUpNotification()
        {
            var notifications = new List<AppAlertNotificationModel>();

            try
            {
                var paymentFollowUps = await _notificationWorkerService.GetFastPayAndCarryPaymnetFollowup();

                foreach (var paymentFollowUp in paymentFollowUps.Where(x => x.PaymentFollowUpType == EnumPaymentFollowUpType.FastPayCarry))
                {
                    var title = $"Today you have Fast Pay Carry Follow Up.";
                    var body = $"Fast Pay Carry Follow Up - Customer No: {paymentFollowUp.CustomerNo}, Customer Name: {paymentFollowUp.CustomerName}, " +
                        $"Invoice No: {paymentFollowUp.InvoiceNo}, Invoice Date: {paymentFollowUp.InvoiceDate}, " +
                        $"Invoice Age: {paymentFollowUp.InvoiceAge}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Notification failed for Payment FollowUp to send for User Id: {userId}");
                //LoggerExtension.ToWriteLog($"Notification failed for Payment FollowUp to send for User Id ({userId}): {ex}", _rootPath);

                _logger.LogError(ex, $"Notification failed for Payment FollowUp to send ");
                LoggerExtension.ToWriteLog($"Notification failed for Payment FollowUp to send ): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppAlertNotificationModel>> GetCustomerOccasionNotification()
        {
            var notifications = new List<AppAlertNotificationModel>();

            try
            {
                var customerOccasions = await _notificationWorkerService.GetOccassionToCelebrste();

                foreach (var customerOccasion in customerOccasions)
                {
                    var title = string.Empty;
                    var body = string.Empty;
                    var today = DateTime.Now;

                    title = $"Dealer's Occasion.";

                    if (today.Date == CustomConvertExtension.ObjectToDateTime(customerOccasion.DOB).Date)
                    {
                        body = $"Today have {customerOccasion.CustomerName}'s ({customerOccasion.CustomarNo}) Birthdays.";
                    }
                    else if (today.Date == CustomConvertExtension.ObjectToDateTime(customerOccasion.SpouseDOB).Date)
                    {
                        body = $"Today have {customerOccasion.CustomerName}'s ({customerOccasion.CustomarNo}) spouse Birthdays.";
                    }
                    else if (today.Date == CustomConvertExtension.ObjectToDateTime(customerOccasion.FirsChildDOB).Date)
                    {
                        body = $"Today have {customerOccasion.CustomerName}'s ({customerOccasion.CustomarNo}) child Birthdays.";
                    }
                    else if (today.Date == CustomConvertExtension.ObjectToDateTime(customerOccasion.SecondChildDOB).Date)
                    {
                        body = $"Today have {customerOccasion.CustomerName}'s ({customerOccasion.CustomarNo}) child Birthdays.";
                    }
                    else if (today.Date == CustomConvertExtension.ObjectToDateTime(customerOccasion.ThirdChildDOB).Date)
                    {
                        body = $"Today have {customerOccasion.CustomerName}'s ({customerOccasion.CustomarNo}) child Birthdays.";
                    }

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Notification failed for Dealer Occasion to send for User Id: {userId}");
                //LoggerExtension.ToWriteLog($"Notification for Dealer Occasion failed to send for User Id ({userId}): {ex}", _rootPath);


                _logger.LogError(ex, $"Notification failed for Dealer Occasion to send ");
                LoggerExtension.ToWriteLog($"Notification for Dealer Occasion failed to send: {ex}", _rootPath);
            }

            return notifications;
        }

        public async Task<Dictionary<string, IList<AppAlertNotificationModel>>> GetNotificationByEmpRole()
        {
            Dictionary<string, IList<AppAlertNotificationModel>> keyValuePairs = new Dictionary<string, IList<AppAlertNotificationModel>>();

            var appUser = AppIdentity.AppUser;
            EnumEmployeeRole employeeRole = (EnumEmployeeRole)Enum.Parse(typeof(EnumEmployeeRole), appUser.EmployeeRole.ToString());
            TypeEnum typeEnum = (TypeEnum)Enum.Parse(typeof(TypeEnum), TypeEnum.Alert.ToString());
            var permission = await _menuService.GetAlertPermissionsByEmp(employeeRole, typeEnum);

            foreach (var item in permission)
            {
                switch (item.Name)
                {
                    case "Occasion to Celebrate":
                        keyValuePairs.Add("OccasiontoCelebrate", await GetCustomerOccasionNotification());
                        break;
                    case "Lead Followup Reminder":
                        keyValuePairs.Add("LeadFollowupReminder", await GetLeadFollowUpNotification());
                        break;

                    case "Cheque Bounce Notification":
                        keyValuePairs.Add("ChequeBounceNotification", await GetCheckBounceNotification());
                        break;
                    case "RPRS Notification":
                        keyValuePairs.Add("RPRSNotification", await GetPaymentFollowUpNotification());
                        break;

                    case "Fast Pay & Carry Notification":
                        keyValuePairs.Add("FastPay&CarryNotification", await GetFastPayCarryFollowUpNotification());
                        break;

                    case "Credit Limit Cross Notifiction ":
                        keyValuePairs.Add("CreditLimitCrossNotifiction ", await GetCreditLimitCrossNotification());
                        break;
                };
            };

            return keyValuePairs;

        }
    }

    public class AppAlertNotificationModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
