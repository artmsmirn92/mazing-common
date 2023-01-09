using mazing.common.Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace mazing.common.Runtime.UI
{
    public delegate void ClosePanelAction(UnityAction _OnFinishClosing);
    
    public interface IDialogPanel
    {
        int               DialogViewerId     { get; }
        EAppearingState   AppearingState     { get; set; }
        RectTransform     PanelRectTransform { get; }
        Animator          Animator           { get; }
        
        void              LoadPanel(RectTransform _Container, ClosePanelAction _OnClose);
    }
    
    public class DialogPanelFake : IDialogPanel
    {
        public int             DialogViewerId     => default;
        public EAppearingState AppearingState     { get; set; } = EAppearingState.Dissapeared;
        public RectTransform   PanelRectTransform => null;
        public Animator        Animator           => null;
        
        public void              LoadPanel(RectTransform _Container, ClosePanelAction _OnClose) { }
    }
}