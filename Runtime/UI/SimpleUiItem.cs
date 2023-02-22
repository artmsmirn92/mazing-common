using System.Collections.Generic;
using System.Linq;
using mazing.common.Runtime.Constants;
using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Enums;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Helpers;
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Ticker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace mazing.common.Runtime.UI
{
    public class SimpleUiItem : MonoBehInitBase
    {
        #region serialized fields

        [SerializeField] protected Image                 background;
        [SerializeField] private   List<TextMeshProUGUI> texts;

        #endregion
        
        #region nonpublic members

        protected IAudioManager        AudioManager        { get; set; }
        protected IUITicker            Ticker              { get; set; }
        protected ILocalizationManager LocalizationManager { get; private set; }

        private bool   m_IsBackgroundNotNull;
        private bool[] m_AreTextsNotNull;
        private Color  m_BorderDefaultColor;
        private Color  m_BorderHighlightedColor;

        #endregion
        
        #region api
        
        public bool Highlighted { get; set; }
        
        public virtual void Init(
            IUITicker            _UITicker,
            IAudioManager        _AudioManager,
            ILocalizationManager _LocalizationManager)
        {
            Ticker              = _UITicker;
            AudioManager        = _AudioManager;
            LocalizationManager = _LocalizationManager;
            
            Ticker.Register(this);
            CheckIfSerializedItemsNotNull();
            foreach (var text in texts.Where(_Text => _Text.IsNotNull()))
            {
                var locTextInfo = new LocTextInfo(text, ETextType.MenuUI,
                    _TextLocalizationType: ETextLocalizationType.OnlyFont);
                LocalizationManager.AddLocalization(locTextInfo);
            }
            base.Init();
        }
        
        #endregion
        
        #region nonpublic methods

        protected virtual void CheckIfSerializedItemsNotNull()
        {
            m_IsBackgroundNotNull = background.IsNotNull();
            m_AreTextsNotNull = texts.Select(_T => _T.IsNotNull()).ToArray();
        }

        
        protected void PlayButtonClickSound()
        {
            AudioManager.PlayClip(CommonAudioClipArgs.UiButtonClick);
        }
        
        #endregion
    }
}