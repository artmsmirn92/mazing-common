using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Enums;

namespace mazing.common.Runtime.Constants
{
    public static class CommonAudioClipArgs
    {
        public static AudioClipArgs UiButtonClick => new AudioClipArgs(
            "ui_button_click", EAudioClipType.UiSound);
    }
}