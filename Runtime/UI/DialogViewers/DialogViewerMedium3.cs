using mazing.common.Runtime.CameraProviders;
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Ticker;

namespace mazing.common.Runtime.UI.DialogViewers
{
    public interface IDialogViewerMedium3 : IDialogViewerMedium { }
    
    public class DialogViewerMedium3Fake : DialogViewerMediumFake, IDialogViewerMedium3 { }
    
    public class DialogViewerMedium3 : DialogViewerMediumBase, IDialogViewerMedium3
    {
        protected override string PrefabName => "medium_3";
        
        public DialogViewerMedium3(
            IViewUICanvasGetter _CanvasGetter,
            IUITicker           _Ticker,
            ICameraProvider     _CameraProvider,
            IPrefabSetManager   _PrefabSetManager)
            : base(
                _CanvasGetter,
                _Ticker, 
                _CameraProvider, 
                _PrefabSetManager) { }
    }
}