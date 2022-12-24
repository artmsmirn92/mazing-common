using System;
using System.Collections.Generic;
using mazing.common.Runtime.Helpers;

namespace mazing.common.Runtime.Managers.Notifications
{
    public class NotificationsManagerFake : InitBase, INotificationsManager
    {
        public ENotificationsOperatingMode OperatingMode        { get; set; }
        public List<PendingNotification>   PendingNotifications => new List<PendingNotification>();
        public int?                        LastNotificationsCountToReschedule { get; set; }

        public void EnableNotifications(bool _Enable)
        {
            Dbg.LogWarning("Enabling/disabling notifications is available on device only.");
        }

        public void SendNotification(
            string   _Title,
            string   _Body,
            TimeSpan _Span,
            int?     _BadgeNumber = null,
            bool     _Reschedule  = false,
            string   _ChannelId   = null,
            string   _SmallIcon   = null,
            string   _LargeIcon   = null) { }

        public void ClearAllNotifications() { }
    }
}