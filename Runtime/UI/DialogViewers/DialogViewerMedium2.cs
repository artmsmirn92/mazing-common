using mazing.common.Runtime.CameraProviders;
using mazing.common.Runtime.Enums;
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Ticker;

namespace mazing.common.Runtime.UI.DialogViewers
{
    public interface IDialogViewerMedium2 : IDialogViewerMedium { }
    
    public class DialogViewerMedium2Fake : DialogViewerMediumFake, IDialogViewerMedium2 { }
    
    public class DialogViewerMedium2 : DialogViewerCommonBase, IDialogViewerMedium2
    {
        protected override string PrefabName => "medium_2";
        
        public DialogViewerMedium2(
            IViewUICanvasGetter _CanvasGetter,
            IUITicker           _Ticker,
            ICameraProvider     _CameraProvider,
            IPrefabSetManager   _PrefabSetManager)
            : base(
                _CanvasGetter,
                _Ticker, 
                _CameraProvider, 
                _PrefabSetManager) { }

        public override int Id => DialogViewerTypesCommon.Medium2;
    }
}