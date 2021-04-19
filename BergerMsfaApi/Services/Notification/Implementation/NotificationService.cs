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

namespace BergerMsfaApi.Services.Notification.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly FCMSettingsModel _settings;

        public NotificationService(IOptions<FCMSettingsModel> settings)
        {
            this._settings = settings.Value;
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
    }
}
