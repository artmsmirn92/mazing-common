using mazing.common.Runtime.Utils;

namespace mazing.common.Runtime.Enums
{
    public static class DialogViewerIdsCommon
    {
        public static int FullscreenCommon => CommonUtils.GetHash(nameof(FullscreenCommon));
        public static int MediumCommon     => CommonUtils.GetHash(nameof(MediumCommon));
    }
}