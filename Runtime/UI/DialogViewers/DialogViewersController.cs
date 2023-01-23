using System.Collections.Generic;
using System.Linq;
using mazing.common.Runtime.Enums;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Helpers;

namespace mazing.common.Runtime.UI.DialogViewers
{
    public interface IDialogViewersController : IInit
    {
        void          RegisterDialogViewer(IDialogViewer _DialogViewer);
        IDialogViewer GetViewer(int _DialogViewerId);
    }

    public class DialogViewersControllerFake : InitBase, IDialogViewersController
    {
        public void          RegisterDialogViewer(IDialogViewer _DialogViewer) { }
        public IDialogViewer GetViewer(int                      _DialogViewerId) => null;
    }
    
    public class DialogViewersController : InitBase, IDialogViewersController
    {
        #region nonpublic members

        private readonly Dictionary<int, IDialogViewer> m_ViewersDict 
            = new Dictionary<int, IDialogViewer>();

        #endregion
        
        #region inject

        private IViewUICanvasGetter           CanvasGetter                 { get; }
        private IDialogViewerMediumCommon     DialogViewerMediumCommon     { get; }
        private IDialogViewerFullscreenCommon DialogViewerFullscreenCommon { get; }

        public DialogViewersController(
            IViewUICanvasGetter           _CanvasGetter,
            IDialogViewerMediumCommon     _DialogViewerMediumCommon,
            IDialogViewerFullscreenCommon _DialogViewerFullscreenCommon)
        {
            CanvasGetter                 = _CanvasGetter;
            DialogViewerMediumCommon     = _DialogViewerMediumCommon;
            DialogViewerFullscreenCommon = _DialogViewerFullscreenCommon;
        }

        #endregion

        #region api

        public override void Init()
        {
            CanvasGetter.Init();
            RegisterDialogViewer(DialogViewerMediumCommon);
            RegisterDialogViewer(DialogViewerFullscreenCommon);
            SetOtherDialogViewersShowingActions();
            base.Init();
        }

        public void RegisterDialogViewer(IDialogViewer _DialogViewer)
        {
            _DialogViewer.Init();
            m_ViewersDict.Add(_DialogViewer.Id, _DialogViewer);
        }

        public IDialogViewer GetViewer(int _DialogViewerId)
        {
            return m_ViewersDict.GetSafe(_DialogViewerId, out _);
        }

        #endregion

        #region nonpublic methods
        
        private void SetOtherDialogViewersShowingActions()
        {
            var viewers = m_ViewersDict.Values.ToList();
            foreach (var dialogViewer in viewers)
            {
                dialogViewer.OtherDialogViewersShowing = () =>
                {
                    var panels = viewers
                        .Except(new[] {dialogViewer})
                        .Where(_V => _V.CanvasName == dialogViewer.CanvasName)
                        .Select(_V => _V.CurrentPanel);
                    return panels.Any(_P => _P != null &&
                                            _P.AppearingState != EAppearingState.Dissapeared);
                };
            }
        }

        #endregion
    }
}