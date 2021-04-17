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
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _settings.FileName))
            });
            Console.WriteLine(defaultApp.Name); // "[DEFAULT]"
            var notification = new AndroidNotification
            {
                Title = title,
                Body = body
            };
            var message = new Message()
            {
                Android = new AndroidConfig
                {
                    Notification = notification,
                    Priority = Priority.High,
                    TimeToLive = TimeSpan.FromSeconds(0)
                },
                //Data = new Dictionary<string, string>()
                //{
                //    { "score", "850" },
                //    { "time", "2:45" },
                //},
                //Apns = new ApnsConfig
                //{
                //    Headers = new Dictionary<string, string>()
                //    {
                //        {"apns-priority","5" }
                //    }
                //},
                //Notification = new FirebaseAdmin.Messaging.Notification
                //{
                //    Title = title,
                //    Body = body
                //},
                //Topic = "all",
                //Token = "fTDrp8YCRyWXu3_KwPpDDW:APA91bHJJdTzI081sY5LnLulQ4IKDjIB1gkpqnjKX5EKic8AB395sDVB7ZPTivQuGr3z-WI3bybftBHGWDhPz5z0RFTI_fF8d4-EsY35pop2HCqG6btwwTImcJ0n57_hYH-6JwIRwvEc",
                //Token = "cRm8t_Z-SxmVa4BLxS1YQi:APA91bGJqjJ0X6GmPtb1TKquz8bziYnn-evyRX9OAj01AqRLZuqmTQmZD_O20perL2NNFz50JANarNgl1QLJxTUJfII_Qyjz-nLdoREybXzMAoZ7_rPdf4cQ2X_eq2hBpxpHYMhzU11E",
                Token = fcmToken,

            };

            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(message);
            Console.WriteLine(result); //projects/myapp/messages/2492588335721724324

            return true;
        }
    }
}
