using BergerMsfaApi.Models.Common;
using BergerMsfaApi.Models.Tinting;
using BergerMsfaApi.Services.Notification.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace BergerMsfaApi.Services.AlertNotification
{
    public interface IAlertNotificationService
    {
        Task<IList<AppAlert>> GetNotificationByEmpRole();
    }
}
