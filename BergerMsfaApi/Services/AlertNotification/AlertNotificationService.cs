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
using Berger.Common.Constants;

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
                    var title = $"Lead Followup Reminder.";
                    var body = $"Lead ID: {leadFollowUp.Code}" +
                                $"Lead Name: {leadFollowUp.ProjectName}, " +
                                $"Lead Address: {leadFollowUp.ProjectAddress}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {

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
                var checkBouncesGroup = checkBounces.GroupBy(p => new { p.CustomarNo, p.CustomerName }).ToList();

                foreach (var item in checkBouncesGroup)
                {
                    var title = $"Cheque Bounce Notification.";
                    var body = $"Dealer ID: {item.Key.CustomarNo}, Dealer Name: {item.Key.CustomerName}, " +
                        $"Number of Cheque: {item.Where(x => (long.TryParse(x.ChequeNo, out long val))).Select(x => x.ChequeNo).Distinct().Count()}, " +
                        $"Total Amount of Cheque: {item.Sum(p => p.Amount)}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {

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
                    var title = $"Credit Limit Cross Notification .";
                    var body = $"Dealer ID: {creditLimitCross.CustomerNo}, Dealer Name: {creditLimitCross.CustomerName}, " +
                        $"Value Limit: {creditLimitCross.CreditLimit}" +
                        $"Total Due: {creditLimitCross.TotalDue}" +
                        $"Excess Value: {creditLimitCross.TotalDue - creditLimitCross.CreditLimit}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Notification failed for Credit Limit Cross to send");
                LoggerExtension.ToWriteLog($"Notification failed for Credit Limit Cross to send): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppAlertNotificationModel>> GetRPRSPaymentFollowUpNotification()
        {
            var notifications = new List<AppAlertNotificationModel>();

            try
            {
                var paymentFollowUps = await _notificationWorkerService.GetRPRSPaymnetFollowup();

                foreach (var paymentFollowUp in paymentFollowUps.Where(x => x.PaymentFollowUpType == EnumPaymentFollowUpType.RPRS))
                {
                    var title = $"RPRS Payment Followup.";
                    var body = $"Dealer ID: {paymentFollowUp.CustomerNo}, Dealer Name: {paymentFollowUp.CustomerName}, " +
                        $"Invoice Value: {paymentFollowUp.InvoiceValue}, " +
                        $"Invoice Age: {paymentFollowUp.InvoiceAge}, RPRS Date: {paymentFollowUp.RPRSDate}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }

            }
            catch (Exception ex)
            {

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
                    var title = $"Fast Pay & Carry Payment Followup.";
                    var body = $"Dealer ID: {paymentFollowUp.CustomerNo}, Dealer Name: {paymentFollowUp.CustomerName}, " +
                        $"Invoice Value: {paymentFollowUp.InvoiceValue}, " +
                        $"Invoice Age: {paymentFollowUp.InvoiceAge}, Payment Date: {paymentFollowUp.RPRSDate}";

                    notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {

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

                    title = $"Occasion to Celebrate.";

                    if (today.Day == CustomConvertExtension.ObjectToDateTime(customerOccasion.DOB).Day && today.Month == CustomConvertExtension.ObjectToDateTime(customerOccasion.DOB).Month)
                    {
                        body = $"Dealer ID: {customerOccasion.CustomarNo}', Name: ({customerOccasion.CustomerName}) ,Occasion Name: Birthdays.";

                    }
                    else if (today.Day == CustomConvertExtension.ObjectToDateTime(customerOccasion.SpouseDOB).Day && today.Month == CustomConvertExtension.ObjectToDateTime(customerOccasion.SpouseDOB).Month)
                    {
                        body = $"Dealer ID: {customerOccasion.CustomarNo}', Name: ({customerOccasion.CustomerName}) ,Occasion Name:Spouse Birthdays.";

                    }
                    else if (today.Day == CustomConvertExtension.ObjectToDateTime(customerOccasion.FirsChildDOB).Day && today.Month == CustomConvertExtension.ObjectToDateTime(customerOccasion.FirsChildDOB).Month)
                    {
                        body = $"Dealer ID: {customerOccasion.CustomarNo}', Name: ({customerOccasion.CustomerName}) ,Occasion Name:First Child Birthdays.";

                    }
                    else if (today.Day == CustomConvertExtension.ObjectToDateTime(customerOccasion.SecondChildDOB).Day && today.Month == CustomConvertExtension.ObjectToDateTime(customerOccasion.SecondChildDOB).Month)
                    {
                        body = $"Dealer ID: {customerOccasion.CustomarNo}', Name: ({customerOccasion.CustomerName}) ,Occasion Name:Second Child Birthdays.";

                    }
                    else if (today.Day == CustomConvertExtension.ObjectToDateTime(customerOccasion.ThirdChildDOB).Day &&
                             today.Month == CustomConvertExtension.ObjectToDateTime(customerOccasion.ThirdChildDOB)
                                 .Month)
                    {
                        body =
                            $"Dealer ID: {customerOccasion.CustomarNo}', Name: ({customerOccasion.CustomerName}) ,Occasion Name:Third Child Birthdays.";
                    }


                    if (!string.IsNullOrWhiteSpace(body))
                        notifications.Add(new AppAlertNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {


                _logger.LogError(ex, $"Notification failed for Dealer Occasion to send ");
                LoggerExtension.ToWriteLog($"Notification for Dealer Occasion failed to send: {ex}", _rootPath);
            }

            return notifications;
        }

        public async Task<IList<AppAlert>> GetNotificationByEmpRole()
        {

            IList<AppAlert> appAlerts = new List<AppAlert>();

            EnumEmployeeRole employeeRole = (EnumEmployeeRole)Enum.Parse(typeof(EnumEmployeeRole), AppIdentity.AppUser.EmployeeRole.ToString());
            TypeEnum typeEnum = (TypeEnum)Enum.Parse(typeof(TypeEnum), TypeEnum.Alert.ToString());
            var permission = await _menuService.GetAlertPermissionsByEmp(employeeRole, typeEnum);

            foreach (var item in permission)
            {
                AppAlert app = new AppAlert();
                switch (item.Name)
                {
                    case ConstantAlertNotificationValue.OccasiontoCelebrate:
                        var occasiontoCelebrate = await GetCustomerOccasionNotification();
                        app.cateegoryName = "Occasion to Celebrate Notification";
                        app.notifications = occasiontoCelebrate;
                        appAlerts.Add(app);
                        break;

                    case ConstantAlertNotificationValue.LeadFollowupReminder:
                        var leadFollowupReminder = await GetLeadFollowUpNotification();
                        app.cateegoryName = "Lead Followup Reminder Notification";
                        app.notifications = leadFollowupReminder;
                        appAlerts.Add(app);
                        break;

                    case ConstantAlertNotificationValue.ChequeBounceNotification:
                        var chequeBounceNotification = await GetCheckBounceNotification();
                        app.cateegoryName = "Cheque Bounce Notification";
                        app.notifications = chequeBounceNotification;
                        appAlerts.Add(app);
                        break;

                    case ConstantAlertNotificationValue.RPRSNotification:
                        var rprsNotification = await GetRPRSPaymentFollowUpNotification();

                        app.cateegoryName = "RPRS Notification";
                        app.notifications = rprsNotification;
                        appAlerts.Add(app);
                        break;

                    case ConstantAlertNotificationValue.FastPayCarryNotification:
                        var FastPayCarryNotification = await GetFastPayCarryFollowUpNotification();
                        app.cateegoryName = "Fast Pay & Carry Notification";
                        app.notifications = FastPayCarryNotification;
                        appAlerts.Add(app);
                        break;

                    case ConstantAlertNotificationValue.CreditLimitCrossNotifiction:
                        var CreditLimitCrossNotifiction = await GetCreditLimitCrossNotification();
                        app.cateegoryName = "Credit Limit Cross Notifiction";
                        app.notifications = CreditLimitCrossNotifiction;
                        appAlerts.Add(app);
                        break;
                }

            }
            return appAlerts;

        }
    }

    public class AppAlertNotificationModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class AppAlert
    {
        public AppAlert()
        {
            notifications = new List<AppAlertNotificationModel>();
        }
        public string cateegoryName { get; set; }
        public IList<AppAlertNotificationModel> notifications { get; set; }
    }


}
