using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Tinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.Notification.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendPushNotificationAsync(string fcmToken, string title, string body);
    }
}
