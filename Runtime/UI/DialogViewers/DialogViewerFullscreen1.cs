using mazing.common.Runtime.CameraProviders;
using mazing.common.Runtime.Enums;
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Ticker;

namespace mazing.common.Runtime.UI.DialogViewers
{
    public interface IDialogViewerFullscreen1 : IDialogViewerMedium { }
    
    public class DialogViewerFullscreen1Fake : DialogViewerMediumFake, IDialogViewerFullscreen1 { }
    
    public class DialogViewerFullscreen1 : DialogViewerCommonBase, IDialogViewerFullscreen1
    {
        protected override string PrefabName => "fullscreen_1";
        
        public DialogViewerFullscreen1(
            IViewUICanvasGetter _CanvasGetter,
            IUITicker           _Ticker,
            ICameraProvider     _CameraProvider,
            IPrefabSetManager   _PrefabSetManager)
            : base(
                _CanvasGetter,
                _Ticker, 
                _CameraProvider, 
                _PrefabSetManager) { }

        public override int Id => DialogViewerIdsCommon.Fullscreen1;
    }
}