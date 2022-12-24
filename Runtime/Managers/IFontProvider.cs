using mazing.common.Runtime.Enums;
using TMPro;

namespace mazing.common.Runtime.Managers
{
    public interface IFontProvider
    {
        TMP_FontAsset GetFont(ETextType _TextType, ELanguage _Language);
    }
}