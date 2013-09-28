using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHelper.Model;
using HomeHelper.Repository.Abstract;
using HomeHelper.Utils;
using Windows.UI.Notifications;

namespace HomeHelper.Repository.Concret
{
    public class ScheduleWindows8 : IScheduleRepository
    {
        public void AddAlertToSchedule(AlertaUtilitate t)
        {
            if (t.IdAlertaUilitate != 0)
            {
                var found =
                    ToastNotificationManager.CreateToastNotifier()
                                            .GetScheduledToastNotifications()
                                            .FirstOrDefault(x => x.Id == t.IdAlertaUilitate.ToString());
                if (found != null)
                {
                    ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(found);
                }
            }
            var toast = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText01);
            var elements = toast.GetElementsByTagName("text");
            elements[0].AppendChild(
                toast.CreateTextNode(string.Format("{0} {1}", DbUtils.Loader.GetString(resource: "AlertaIndexConsum"), t.NumeUtilitate)));
            var toastNou = new ScheduledToastNotification(toast, t.DataAlerta) { Id = t.IdAlertaUilitate.ToString() };
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toastNou);
        }

        public void DeleteFromSchedule(AlertaUtilitate t)
        {
                var found =
                            ToastNotificationManager.CreateToastNotifier()
                                .GetScheduledToastNotifications()
                                .FirstOrDefault(x => x.Id == t.IdAlertaUilitate.ToString());
                if (found != null)
                {
                    ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(found);
                }
        }
    }
}
