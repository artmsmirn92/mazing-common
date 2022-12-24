using UnityEngine;

namespace mazing.common.Runtime.Utils
{
    public static class NetworkUtils
    {
        public static bool IsPacketSuccess(long _ResponseCode)
        {
            return MathUtils.IsInRange(_ResponseCode, 200, 299);
        }

        public static bool IsInternetConnectionAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
}