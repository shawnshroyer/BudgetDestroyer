using BudgetDestroyer.Models;
using BudgetDestroyer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace BudgetDestroyer.Helpers
{
    public class NotificationHelper
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        //private string userID = HttpContext.Current.User.Identity.GetUserId();

        //public ICollection<TicketNotification> NotificationRead ()
        //{
        //    return db.TicketNotifications.Where(t => t.RecipientId == userID && t.IsRead == true).OrderByDescending(o => o.Sent).ToList();
        //}

        //public ICollection<TicketNotification> NotificationUnread()
        //{
        //    return db.TicketNotifications.Where(t => t.RecipientId == userID && t.IsRead == false).OrderByDescending(o => o.Sent).ToList();
        //}

        //public ICollection<TicketNotification> NotificationAll()
        //{
        //    return db.TicketNotifications.Where(t => t.RecipientId == userID).OrderByDescending(o => o.Sent).ToList();
        //}

        public string HowLongAgo (DateTimeOffset dateTimeOffset)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dateTimeOffset.UtcTicks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
    }
}