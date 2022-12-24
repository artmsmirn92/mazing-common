using UnityEngine.Events;

namespace mazing.common.Runtime
{
    public interface IShowAlertDialogAction
    {
        UnityAction<string, string> ShowAlertDialogAction { set; }
    }
}