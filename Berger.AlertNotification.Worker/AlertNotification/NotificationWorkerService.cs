using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Berger.AlertNotification.Worker.AlertNotification
{
    public class NotificationWorkerService : INotificationWorkerService
    {
        public void get()
        {
            throw new NotImplementedException();
        }
    }

    public interface INotificationWorkerService
    {
        public void get();

    }
}
