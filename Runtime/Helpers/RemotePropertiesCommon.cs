using System.Collections.Generic;

namespace mazing.common.Runtime.Helpers
{
    public interface IRemotePropertiesCommon
    {
        bool                      DebugEnabled          { get; set; }
        List<string>              TestDeviceIdsForAdmob { get; set; }
        IList<NotificationInfoEx> Notifications         { get; set; }  
    }
    
    public class RemotePropertiesCommon : IRemotePropertiesCommon
    {
        public bool                      DebugEnabled          { get; set; }
        public List<string>              TestDeviceIdsForAdmob { get; set; }
        public IList<NotificationInfoEx> Notifications         { get; set; }  
    }
}