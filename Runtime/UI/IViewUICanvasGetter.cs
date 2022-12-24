using UnityEngine;

namespace mazing.common.Runtime.UI
{
    public interface IViewUICanvasGetter : IInit
    {
        Canvas GetCanvas();
    }
}