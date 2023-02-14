using System;
using mazing.common.Runtime.Enums;
using UnityEngine;

namespace mazing.common.Runtime.Entities
{
    public class LocTextInfo : ICloneable
    {
        public Component             TextObject           { get; }
        public string                LocalizationKey      { get; set; }
        public ETextType             TextType             { get; set; }
        public Func<string, string>  TextFormula          { get; set; }
        public ETextLocalizationType TextLocalizationType { get; } 

        public LocTextInfo(
            Component             _TextObject,
            ETextType             _TextType,
            string                _LocalizationKey      = null,
            Func<string, string>  _TextFormula          = null,
            ETextLocalizationType _TextLocalizationType = ETextLocalizationType.TextAndFont)
        {
            TextObject           = _TextObject;
            TextType             = _TextType;
            LocalizationKey      = _LocalizationKey;
            TextFormula          = _TextFormula;
            TextLocalizationType = _TextLocalizationType;
        }

        public object Clone()
        {
            return new LocTextInfo(TextObject, TextType, LocalizationKey, TextFormula, TextLocalizationType);
        }
    }
}