using System;
using System.Collections.Generic;
using mazing.common.Runtime.Enums;

namespace mazing.common.Runtime.Helpers
{
    public class NotificationInfoEx
    {
        public Dictionary<ELanguage, string> Title { get; set; }
        public Dictionary<ELanguage, string> Body  { get; set; }
        public TimeSpan                      Span  { get; set; }
    }
}