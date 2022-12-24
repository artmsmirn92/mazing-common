using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Enums;
using UnityEngine.Events;

namespace mazing.common.Runtime.Managers
{
    public interface ILocalizationManager : IInit, IFontProvider
    {
        event UnityAction<ELanguage> LanguageChanged;
        string                       GetTranslation(string _Key);
        void                         SetLanguage(ELanguage _Language);
        ELanguage                    GetCurrentLanguage();
        void                         AddTextObject(LocalizableTextObjectInfo    _Info);
        void                         RemoveTextObject(LocalizableTextObjectInfo _Info);
    }
}