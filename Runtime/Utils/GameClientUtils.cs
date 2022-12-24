namespace mazing.common.Runtime.Utils
{
    public static class GameClientUtils
    {
        private const int DefaultAccountId = 0;
        
        public static int AccountId
        {
            get => SaveUtils.GetValue(SaveKeysCommon.AccountId) ?? DefaultAccountId;
            set => SaveUtils.PutValue(SaveKeysCommon.AccountId, value);
        }

        public static int GetDefaultGameId()
        {
            return 1;
        }

        public static string ServerApiUrl { get; set; } = "http://77.37.152.15:7000";
    }
}