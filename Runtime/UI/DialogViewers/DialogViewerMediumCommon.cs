using mazing.common.Runtime.CameraProviders;
using mazing.common.Runtime.Enums;
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Ticker;

namespace mazing.common.Runtime.UI.DialogViewers
{
    public interface IDialogViewerMediumCommon : IDialogViewerMedium { }
    
    public class DialogViewerMediumCommonFake : DialogViewerMediumFake, IDialogViewerMediumCommon { }
    
    public class DialogViewerMediumCommon : DialogViewerCommonBase, IDialogViewerMediumCommon
    {
        protected override string PrefabName => "medium_common";
        
        public DialogViewerMediumCommon(
            IViewUICanvasGetter _CanvasGetter,
            IUITicker           _Ticker,
            ICameraProvider     _CameraProvider,
            IPrefabSetManager   _PrefabSetManager)
            : base(
                _CanvasGetter,
                _Ticker, 
                _CameraProvider, 
                _PrefabSetManager) { }

        public override int    Id         => DialogViewerIdsCommon.MediumCommon;
        public override string CanvasName => CommonCanvasNames.CommonScreenSpace;
    }
}