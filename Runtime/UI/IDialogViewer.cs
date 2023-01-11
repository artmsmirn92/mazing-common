using System;
using UnityEngine;
using UnityEngine.Events;

namespace mazing.common.Runtime.UI
{
    public interface IDialogViewer : IInit
    {
        int           Id           { get; }
        string        CanvasName   { get; }
        IDialogPanel  CurrentPanel { get; }
        RectTransform Container    { get; }
        
        Func<bool> OtherDialogViewersShowing { get; set; }
        void       Back(UnityAction  _OnFinish                      = null);
        void       Show(IDialogPanel _Panel, float _AnimationSpeed = 1f, bool _HidePrevious = true);
    }
}