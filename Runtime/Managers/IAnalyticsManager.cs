using System.Collections.Generic;

namespace mazing.common.Runtime.Managers
{
    public interface IAnalyticsProvider : IInit
    {
        void SendAnalytic(string _AnalyticId, IDictionary<string, object> _EventData = null);
    }
    
    public interface IAnalyticsManager : IAnalyticsProvider { }
}