using Berger.Odata.Model;
using Berger.Odata.Services;
using BergerMsfaApi.Extensions;
using BergerMsfaApi.Services.DemandGeneration.Interfaces;
using BergerMsfaApi.Services.Interfaces;
using BergerMsfaApi.Services.Notification.Interfaces;
using BergerMsfaApi.Services.Users.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BergerMsfaApi.Services.Workers
{
    public class NotificationWorker : BackgroundService
    {
        private readonly ILogger<NotificationWorker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private ILoginLogService _loginLogService;
        private INotificationService _notificationService;
        private ILeadService _leadService;
        private IODataNotificationService _oDataNotificationService;
        private IAuthService _authService;
        private readonly string _rootPath;
        private readonly int _timeOutHours = 24;
        public NotificationWorker(ILogger<NotificationWorker> logger
            , IServiceScopeFactory serviceScopeFactory, string rootPath, int timeOutHours)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _rootPath = rootPath;
            _timeOutHours = timeOutHours;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("App Push Notification Worker running at: {time}", DateTimeOffset.Now);
            LoggerExtension.ToWriteLog("App Push Notification Worker running at: " + DateTimeOffset.Now, _rootPath);
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        _loginLogService = scope.ServiceProvider.GetService<ILoginLogService>();
                        _notificationService = scope.ServiceProvider.GetService<INotificationService>();
                        _leadService = scope.ServiceProvider.GetService<ILeadService>();
                        _oDataNotificationService = scope.ServiceProvider.GetService<IODataNotificationService>();
                        _authService = scope.ServiceProvider.GetService<IAuthService>();

                        var loggedInUsers = await _loginLogService.GetAllLoggedInUsersAsync();

                        foreach (var loggedInUser in loggedInUsers)
                        {
                            // Send notification
                            if (!string.IsNullOrEmpty(loggedInUser.FCMToken))
                            {
                                await this.SendLeadFollowUpNotification(loggedInUser.UserId, loggedInUser.FCMToken);
                            
                                try
                                {
                                    var dealerIds = (await _authService.GetDealerByUserId(loggedInUser.UserId)).ToList();
                                        
                                    if (dealerIds.Any())
                                    {
                                        await this.SendCheckBounceNotification(loggedInUser.UserId, loggedInUser.FCMToken, dealerIds);
                                        await this.SendCreditLimitCrossNotification(loggedInUser.UserId, loggedInUser.FCMToken, dealerIds);
                                        await this.SendPaymentFollowUpNotification(loggedInUser.UserId, loggedInUser.FCMToken, dealerIds);
                                    }

                                    _logger.LogInformation($"Notification successfully send for User Id ({loggedInUser.UserId}) at: {DateTimeOffset.Now}, {string.Empty}");
                                    LoggerExtension.ToWriteLog($"Notification successfully send for User Id ({loggedInUser.UserId}) at: {DateTimeOffset.Now}, {string.Empty}", _rootPath);
                                    
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(ex, $"Notification failed to send for User Id: {loggedInUser.UserId}");
                                    LoggerExtension.ToWriteLog($"Notification failed to send for User Id ({loggedInUser.UserId}): {ex}", _rootPath);
                                }
                            }
                        }

                        _logger.LogInformation($"All Notification successfully send at: {DateTimeOffset.Now}, {string.Empty}");
                        LoggerExtension.ToWriteLog($"All Notification successfully send at: {DateTimeOffset.Now}, {string.Empty}", _rootPath);
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Notification failed to send");
                    LoggerExtension.ToWriteLog($"Notification failed to send: {ex}", _rootPath);
                }

                await Task.Delay(TimeSpan.FromHours(_timeOutHours), stoppingToken);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("App Push Notification Worker stop at: {time}", DateTimeOffset.Now);
            LoggerExtension.ToWriteLog("App Push Notification Worker stop at: " + DateTimeOffset.Now, _rootPath);
            return base.StopAsync(cancellationToken);
        }

        private async Task SendLeadFollowUpNotification(int userId, string fcmToken)
        {
            try
            {
                var leadFollowUps = await _leadService.GetAllTodayFollowUpByUserIdForNotificationAsync(userId);

                foreach (var leadFollowUp in leadFollowUps)
                {
                    var title = $"You have lead follow up.";
                    var body = $"Lead follow up - Territory: {leadFollowUp.Territory}, Zone: {leadFollowUp.Zone}, " +
                        $"Project Name: {leadFollowUp.ProjectName}, Project Address: {leadFollowUp.ProjectAddress}";

                    await _notificationService.SendPushNotificationAsync(fcmToken, title, body);
                }

                _logger.LogInformation($"Notification successfully send for User Id ({userId}) at: {DateTimeOffset.Now}, {string.Empty}");
                LoggerExtension.ToWriteLog($"Notification successfully send for User Id ({userId}) at: {DateTimeOffset.Now}, {string.Empty}", _rootPath);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification failed to send for User Id ({userId}): {ex}", _rootPath);
            }
        }

        private async Task SendCheckBounceNotification(int userId, string fcmToken, List<int> dealerIds)
        {
            try
            {
                var checkBounces = await _oDataNotificationService.GetAllTodayCheckBouncesByDealerIds(dealerIds);

                foreach (var checkBounce in checkBounces)
                {
                    var title = $"You have check bounce.";
                    var body = $"Check Bounce - Customer No: {checkBounce.CustomerNo}, Customer Name: {checkBounce.CustomerName}, " +
                        $"Credit Control Area: {checkBounce.CreditControlArea}, Amount: {checkBounce.Amount}";

                    await _notificationService.SendPushNotificationAsync(fcmToken, title, body);
                }

                _logger.LogInformation($"Notification successfully send for User Id ({userId}) at: {DateTimeOffset.Now}, {string.Empty}");
                LoggerExtension.ToWriteLog($"Notification successfully send for User Id ({userId}) at: {DateTimeOffset.Now}, {string.Empty}", _rootPath);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification failed to send for User Id ({userId}): {ex}", _rootPath);
            }
        }

        private async Task SendCreditLimitCrossNotification(int userId, string fcmToken, List<int> dealerIds)
        {
            try
            {

                var creditLimitCrosses = await _oDataNotificationService.GetAllTodayCreditLimitCrossByDealerIds(dealerIds);

                foreach (var creditLimitCross in creditLimitCrosses)
                {
                    var title = $"You have Credit Limit Cross.";
                    var body = $"Credit Limit Cross - Customer No: {creditLimitCross.CustomerNo}, Customer Name: {creditLimitCross.CustomerName}, " +
                        $"Credit Control Area: {creditLimitCross.CreditControlArea}, Credit Limit: {creditLimitCross.CreditLimit}, " +
                        $"Total Due: {creditLimitCross.TotalDue}";

                    await _notificationService.SendPushNotificationAsync(fcmToken, title, body);
                }

                _logger.LogInformation($"Notification successfully send for User Id ({userId}) at: {DateTimeOffset.Now}, {string.Empty}");
                LoggerExtension.ToWriteLog($"Notification successfully send for User Id ({userId}) at: {DateTimeOffset.Now}, {string.Empty}", _rootPath);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification failed to send for User Id ({userId}): {ex}", _rootPath);
            }
        }

        private async Task SendPaymentFollowUpNotification(int userId, string fcmToken, List<int> dealerIds)
        {
            try
            {

                var paymentFollowUps = await _oDataNotificationService.GetAllTodayPaymentFollowUpByDealerIds(dealerIds);

                foreach (var paymentFollowUp in paymentFollowUps.Where(x => x.PaymentFollowUpType == EnumPaymentFollowUpTypeModel.RPRS))
                {
                    var title = $"You have RPRS Follow Up.";
                    var body = $"RPRS Follow Up - Invoice No: {paymentFollowUp.InvoiceNo}, Invoice Date: {paymentFollowUp.InvoiceDate}, " +
                        $"Invoice Age: {paymentFollowUp.InvoiceAge}, RPRS Date: {paymentFollowUp.RPRSDate}";

                    await _notificationService.SendPushNotificationAsync(fcmToken, title, body);
                }
                foreach (var paymentFollowUp in paymentFollowUps.Where(x => x.PaymentFollowUpType == EnumPaymentFollowUpTypeModel.FastPayCarry))
                {
                    var title = $"You have Fast Pay Carry Follow Up.";
                    var body = $"Fast Pay Carry Follow Up - Invoice No: {paymentFollowUp.InvoiceNo}, Invoice Date: {paymentFollowUp.InvoiceDate}, " +
                        $"Invoice Age: {paymentFollowUp.InvoiceAge}";

                    await _notificationService.SendPushNotificationAsync(fcmToken, title, body);
                }

                _logger.LogInformation($"Notification successfully send for User Id ({userId}) at: {DateTimeOffset.Now}, {string.Empty}");
                LoggerExtension.ToWriteLog($"Notification successfully send for User Id ({userId}) at: {DateTimeOffset.Now}, {string.Empty}", _rootPath);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Notification failed to send for User Id: {userId}");
                LoggerExtension.ToWriteLog($"Notification failed to send for User Id ({userId}): {ex}", _rootPath);
            }
        }
    }
}
