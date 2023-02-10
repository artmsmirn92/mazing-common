using mazing.common.Runtime.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace mazing.common.Runtime.UI
{
    public class SimpleUiToggleView : SimpleUiItem
    {
        [SerializeField] private Toggle toggle;

        private bool m_IsToggleNotNull;
        
        protected override void CheckIfSerializedItemsNotNull()
        {
            base.CheckIfSerializedItemsNotNull();
            m_IsToggleNotNull = toggle.IsNotNull();
        }
    }
}