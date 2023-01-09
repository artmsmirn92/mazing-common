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

        private IViewUICanvasGetter      CanvasGetter            { get; }
        private IDialogViewerMedium1     DialogViewerMedium1     { get; }
        private IDialogViewerMedium2     DialogViewerMedium2     { get; }
        private IDialogViewerFullscreen1 DialogViewerFullscreen1 { get; }

        public DialogViewersController(
            IViewUICanvasGetter      _CanvasGetter,
            IDialogViewerMedium1     _DialogViewerMedium1,
            IDialogViewerMedium2     _DialogViewerMedium2,
            IDialogViewerFullscreen1 _DialogViewerFullscreen1)
        {
            CanvasGetter            = _CanvasGetter;
            DialogViewerMedium1     = _DialogViewerMedium1;
            DialogViewerMedium2     = _DialogViewerMedium2;
            DialogViewerFullscreen1 = _DialogViewerFullscreen1;
        }

        #endregion

        #region api

        public override void Init()
        {
            CanvasGetter.Init();
            RegisterDialogViewer(DialogViewerMedium1);
            RegisterDialogViewer(DialogViewerMedium2);
            RegisterDialogViewer(DialogViewerFullscreen1);
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
            DialogViewerMedium1.OtherDialogViewersShowing = () =>
            {
                var panels = new[]
                {
                    DialogViewerMedium2   .CurrentPanel,
                    DialogViewerFullscreen1   .CurrentPanel
                };
                return panels.Any(_P => _P != null &&
                                        _P.AppearingState != EAppearingState.Dissapeared);
            };
            DialogViewerMedium2.OtherDialogViewersShowing = () =>
            {
                var panels = new[]
                {
                    DialogViewerMedium1   .CurrentPanel,
                    DialogViewerFullscreen1   .CurrentPanel
                };
                return panels.Any(_P => _P != null &&
                                        _P.AppearingState != EAppearingState.Dissapeared);
            };
            DialogViewerFullscreen1.OtherDialogViewersShowing = () =>
            {
                var panels = new[]
                {
                    DialogViewerMedium1   .CurrentPanel,
                    DialogViewerMedium2   .CurrentPanel,
                };
                return panels.Any(_P => _P != null &&
                                        _P.AppearingState != EAppearingState.Dissapeared);
            };
        }

        #endregion
    }
}