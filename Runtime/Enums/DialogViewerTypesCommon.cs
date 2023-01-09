using mazing.common.Runtime.Utils;

namespace mazing.common.Runtime.Enums
{
    public static class DialogViewerTypesCommon
    {
        public static int Fullscreen1 => CommonUtils.GetHash(nameof(Fullscreen1));
        public static int Medium1     => CommonUtils.GetHash(nameof(Medium1));
        public static int Medium2     => CommonUtils.GetHash(nameof(Medium2));
    }
}