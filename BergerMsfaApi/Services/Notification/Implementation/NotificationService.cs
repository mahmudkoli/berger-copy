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

namespace BergerMsfaApi.Services.Notification.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly FCMSettingsModel _settings;
        private readonly ILogger<NotificationService> _logger;
        private ILeadService _leadService;
        private IODataNotificationService _oDataNotificationService;
        private IAuthService _authService;
        private readonly string _rootPath;

        public NotificationService(IOptions<FCMSettingsModel> settings,
            ILogger<NotificationService> logger,
            ILeadService leadService,
            IODataNotificationService oDataNotificationService,
            IAuthService authService,
            IWebHostEnvironment env)
        {
            this._settings = settings.Value;
            this._logger = logger;
            this._leadService = leadService;
            this._oDataNotificationService = oDataNotificationService;
            this._authService = authService;
            this._rootPath = env.WebRootPath;
        }

        public async Task<bool> SendPushNotificationAsync(string fcmToken, string title, string body)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var defaultApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _settings.FileName))
                });
                //Console.WriteLine(defaultApp.Name); // "[DEFAULT]"
            }

            var notification = new AndroidNotification
            {
                Title = title,
                Body = body
            };

            var message = new Message()
            {
                Android = new AndroidConfig
                {
                    //Notification = notification,
                    Priority = Priority.High,
                    //TimeToLive = TimeSpan.FromSeconds(0),
                    Data = new Dictionary<string, string>()
                    {
                        { "title", title },
                        { "body", body }
                    }
                },
                //Data = new Dictionary<string, string>()
                //{
                //    { "score", "850" },
                //    { "time", "2:45" }
                //},
                //Apns = new ApnsConfig
                //{
                //    Headers = new Dictionary<string, string>()
                //    {
                //        {"apns-priority", "5" }
                //    }
                //},
                //Notification = new FirebaseAdmin.Messaging.Notification
                //{
                //    Title = title,
                //    Body = body
                //},
                //Topic = "all",
                Token = fcmToken
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
            //Console.WriteLine(result); //projects/myapp/messages/2492588335721724324

            return true;
        }

        public async Task<IList<AppNotificationModel>> GetAllTodayNotification(int userId)
        {
            var notifications = new List<AppNotificationModel>();
            var dealerIds = (await _authService.GetDealerByUserId(userId)).ToList();

            var leadNotify = await GetLeadFollowUpNotification(userId);
            var checkNotify = await GetCheckBounceNotification(userId, dealerIds);
            var creditNotify = await GetCreditLimitCrossNotification(userId, dealerIds);
            var paymentNotify = await GetPaymentFollowUpNotification(userId, dealerIds);
            var occasionNotify = await GetCustomerOccasionNotification(userId, dealerIds);

            if (leadNotify.Any()) notifications.AddRange(leadNotify);
            if (checkNotify.Any()) notifications.AddRange(checkNotify);
            if (creditNotify.Any()) notifications.AddRange(creditNotify);
            if (paymentNotify.Any()) notifications.AddRange(paymentNotify);
            if (occasionNotify.Any()) notifications.AddRange(occasionNotify);

            return notifications;
        }

        private async Task<IList<AppNotificationModel>> GetLeadFollowUpNotification(int userId)
        {
            var notifications = new List<AppNotificationModel>();

            try
            {
                var leadFollowUps = await _leadService.GetAllTodayFollowUpByUserIdForNotificationAsync(userId);

                foreach (var leadFollowUp in leadFollowUps)
                {
                    var title = $"Today you have lead follow up.";
                    var body = $"Lead follow up - Territory: {leadFollowUp.Territory}, Zone: {leadFollowUp.Zone}, " +
                        $"Project Name: {leadFollowUp.ProjectName}, Project Address: {leadFollowUp.ProjectAddress}";

                    notifications.Add(new AppNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed for Lead FollowUp to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification failed for Lead FollowUp to send for User Id ({userId}): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppNotificationModel>> GetCheckBounceNotification(int userId, List<int> dealerIds)
        {
            var notifications = new List<AppNotificationModel>();

            try
            {
                var checkBounces = await _oDataNotificationService.GetAllTodayCheckBouncesByDealerIds(dealerIds);

                foreach (var checkBounce in checkBounces)
                {
                    var title = $"Today you have check bounce.";
                    var body = $"Check Bounce - Customer No: {checkBounce.CustomerNo}, Customer Name: {checkBounce.CustomerName}, " +
                        $"Credit Control Area: {checkBounce.CreditControlArea}, Amount: {checkBounce.Amount}";

                    notifications.Add(new AppNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed for Check Bounce to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification failed for Check Bounce to send for User Id ({userId}): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppNotificationModel>> GetCreditLimitCrossNotification(int userId, List<int> dealerIds)
        {
            var notifications = new List<AppNotificationModel>();

            try
            {
                var creditLimitCrosses = await _oDataNotificationService.GetAllTodayCreditLimitCrossByDealerIds(dealerIds);

                foreach (var creditLimitCross in creditLimitCrosses)
                {
                    var title = $"Today you have Credit Limit Cross.";
                    var body = $"Credit Limit Cross - Customer No: {creditLimitCross.CustomerNo}, Customer Name: {creditLimitCross.CustomerName}, " +
                        $"Credit Control Area: {creditLimitCross.CreditControlArea}, Credit Limit: {creditLimitCross.CreditLimit}, " +
                        $"Total Due: {creditLimitCross.TotalDue}";

                    notifications.Add(new AppNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed for Credit Limit Cross to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification failed for Credit Limit Cross to send for User Id ({userId}): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppNotificationModel>> GetPaymentFollowUpNotification(int userId, List<int> dealerIds)
        {
            var notifications = new List<AppNotificationModel>();

            try
            {
                var paymentFollowUps = await _oDataNotificationService.GetAllTodayPaymentFollowUpByDealerIds(dealerIds);

                foreach (var paymentFollowUp in paymentFollowUps.Where(x => x.PaymentFollowUpType == EnumPaymentFollowUpTypeModel.RPRS))
                {
                    var title = $"Today you have RPRS Follow Up.";
                    var body = $"RPRS Follow Up - Customer No: {paymentFollowUp.CustomerNo}, Customer Name: {paymentFollowUp.CustomerName}, " +
                        $"Invoice No: {paymentFollowUp.InvoiceNo}, Invoice Date: {paymentFollowUp.InvoiceDate}, " +
                        $"Invoice Age: {paymentFollowUp.InvoiceAge}, RPRS Date: {paymentFollowUp.RPRSDate}";

                    notifications.Add(new AppNotificationModel() { Title = title, Body = body });
                }

                foreach (var paymentFollowUp in paymentFollowUps.Where(x => x.PaymentFollowUpType == EnumPaymentFollowUpTypeModel.FastPayCarry))
                {
                    var title = $"Today you have Fast Pay Carry Follow Up.";
                    var body = $"Fast Pay Carry Follow Up - Customer No: {paymentFollowUp.CustomerNo}, Customer Name: {paymentFollowUp.CustomerName}, " +
                        $"Invoice No: {paymentFollowUp.InvoiceNo}, Invoice Date: {paymentFollowUp.InvoiceDate}, " +
                        $"Invoice Age: {paymentFollowUp.InvoiceAge}";

                    notifications.Add(new AppNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed for Payment FollowUp to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification failed for Payment FollowUp to send for User Id ({userId}): {ex}", _rootPath);
            }

            return notifications;
        }

        private async Task<IList<AppNotificationModel>> GetCustomerOccasionNotification(int userId, List<int> dealerIds)
        {
            var notifications = new List<AppNotificationModel>();

            try
            {
                var customerOccasions = await _oDataNotificationService.GetAllTodayCustomerOccasionsByDealerIds(dealerIds);

                foreach (var customerOccasion in customerOccasions)
                {
                    var title = string.Empty;
                    var body = string.Empty;
                    var today = DateTime.Now;

                    title = $"Dealer's Occasion.";

                    if (today.Date == CustomConvertExtension.ObjectToDateTime(customerOccasion.DOB).Date)
                    {
                        body = $"Today have {customerOccasion.CustomerName}'s ({customerOccasion.CustomerNo}) Birthdays.";
                    }
                    else if (today.Date == CustomConvertExtension.ObjectToDateTime(customerOccasion.SpouseDOB).Date)
                    {
                        body = $"Today have {customerOccasion.CustomerName}'s ({customerOccasion.CustomerNo}) spouse Birthdays.";
                    }
                    else if (today.Date == CustomConvertExtension.ObjectToDateTime(customerOccasion.ChildDOB).Date)
                    {
                        body = $"Today have {customerOccasion.CustomerName}'s ({customerOccasion.CustomerNo}) child Birthdays.";
                    }

                    notifications.Add(new AppNotificationModel() { Title = title, Body = body });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed for Dealer Occasion to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification for Dealer Occasion failed to send for User Id ({userId}): {ex}", _rootPath);
            }

            return notifications;
        }
    }

    public class AppNotificationModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
