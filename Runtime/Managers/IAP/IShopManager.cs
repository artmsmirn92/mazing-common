using System.Collections.Generic;
using UnityEngine.Events;

namespace mazing.common.Runtime.Managers.IAP
{
    public interface IShopManager : IInit, IShowAlertDialogAction
    {
        void         RegisterProductInfos(List<ProductInfo> _Products);
        void         RestorePurchases();
        void         Purchase(int _Key);
        bool         RateGame();
        ShopItemArgs GetItemInfo(int _Key);
        void         AddPurchaseAction(int _ProductKey, UnityAction _Action);
        void         AddDeferredAction(int _ProductKey, UnityAction _Action);
    }
}