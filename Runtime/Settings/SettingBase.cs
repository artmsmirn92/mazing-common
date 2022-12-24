using System.Collections.Generic;
using mazing.common.Runtime.Entities;
using mazing.common.Runtime.Utils;
using UnityEngine.Events;

namespace mazing.common.Runtime.Settings
{
    public abstract class SettingBase<T> : ISetting<T>
    {
        public event UnityAction<T>      ValueSet;
        public abstract SaveKey<T>       Key          { get; }
        public abstract string           TitleKey     { get; }
        public abstract ESettingLocation Location     { get; }
        public abstract ESettingType     Type         { get; }
        public virtual  List<T>          Values       => null;
        public virtual  object           Min          => null;
        public virtual  object           Max          => null;
        public virtual  string           SpriteOnKey  => null;
        public virtual  string           SpriteOffKey => null;
        public virtual  T                Get()        => SaveUtils.GetValue(Key);
        
        public virtual  void             Put(T _Value)
        {
            SaveUtils.PutValue(Key, _Value);
            RaiseValueSetEvent(_Value);
        }

        protected void RaiseValueSetEvent(T _Value)
        {
            ValueSet?.Invoke(_Value);
        }
    }
}