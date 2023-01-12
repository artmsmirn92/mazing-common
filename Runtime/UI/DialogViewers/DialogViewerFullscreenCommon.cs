using mazing.common.Runtime.CameraProviders;
using mazing.common.Runtime.Enums;
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Ticker;

namespace mazing.common.Runtime.UI.DialogViewers
{
    public interface IDialogViewerFullscreenCommon : IDialogViewer { }
    
    public class DialogViewerFullscreenCommonFake : DialogViewerMediumFake, IDialogViewerFullscreenCommon { }
    
    public class DialogViewerFullscreenCommon : DialogViewerCommonBase, IDialogViewerFullscreenCommon
    {
        protected override string PrefabName => "fullscreen_common";
        
        public DialogViewerFullscreenCommon(
            IViewUICanvasGetter _CanvasGetter,
            IUITicker           _Ticker,
            ICameraProvider     _CameraProvider,
            IPrefabSetManager   _PrefabSetManager)
            : base(
                _CanvasGetter,
                _Ticker, 
                _CameraProvider, 
                _PrefabSetManager) { }

        public override int    Id         => DialogViewerIdsCommon.FullscreenCommon;
        public override string CanvasName => CommonCanvasNames.CommonScreenSpace;
    }
}