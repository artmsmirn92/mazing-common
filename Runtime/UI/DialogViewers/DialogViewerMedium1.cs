using mazing.common.Runtime.CameraProviders;
using mazing.common.Runtime.Enums;
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Ticker;

namespace mazing.common.Runtime.UI.DialogViewers
{
    public interface IDialogViewerMedium1 : IDialogViewerMedium { }
    
    public class DialogViewerMedium1Fake : DialogViewerMediumFake, IDialogViewerMedium1 { }
    
    public class DialogViewerMedium1 : DialogViewerCommonBase, IDialogViewerMedium1
    {
        protected override string PrefabName => "medium_1";
        
        public DialogViewerMedium1(
            IViewUICanvasGetter _CanvasGetter,
            IUITicker           _Ticker,
            ICameraProvider     _CameraProvider,
            IPrefabSetManager   _PrefabSetManager)
            : base(
                _CanvasGetter,
                _Ticker, 
                _CameraProvider, 
                _PrefabSetManager) { }

        public override int Id => DialogViewerIdsCommon.Medium1;
    }
}