#if UNITY_IOS
using mazing.common.Runtime.Managers;
using mazing.common.Runtime.Managers.IAP;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Common.Managers.IAP
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedType.Global
    public class AppleUnityIAPShopManager : UnityIapShopManagerBase
    {
        private IAppleExtensions   m_AppleExtensions;
        
        public AppleUnityIAPShopManager(ILocalizationManager _LocalizationManager)
            : base(_LocalizationManager) { }

        public override void OnInitialized(IStoreController _Controller, IExtensionProvider _Extensions)
        {
            base.OnInitialized(_Controller, _Extensions);
            m_AppleExtensions = _Extensions.GetExtension<IAppleExtensions>();
#if DEVELOPMENT_BUILD
            m_AppleExtensions.simulateAskToBuy = true;
#endif
            m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferredPurchase);
        }

        public override bool RateGame()
        {
            if (!base.RateGame())
                return false;
            // SA.iOS.StoreKit.ISN_SKStoreReviewController.RequestReview();
            Application.OpenURL("itms-apps://itunes.apple.com/app/id1601083190");
            return true;
        }
    }
}
#endif
