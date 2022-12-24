#if UNITY_ANDROID
using System.Collections;
using mazing.common.Runtime.Utils;
using UnityEngine;
using UnityEngine.Purchasing;

namespace mazing.common.Runtime.Managers.IAP
{
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once UnusedType.Global
    public class AndroidUnityIAPShopManager : UnityIapShopManagerBase
    {
        public AndroidUnityIAPShopManager(ILocalizationManager _LocalizationManager) 
            : base(_LocalizationManager) { }

        protected override ConfigurationBuilder GetBuilder()
        {
            var builder = base.GetBuilder();
            builder.Configure<IGooglePlayConfiguration>().SetDeferredPurchaseListener(OnDeferredPurchase);
            return builder;
        }

        public override bool RateGame()
        {
            if (!base.RateGame())
                return false;
            Cor.Run(RateGameAndroid());
            return true;
        }

        private static IEnumerator RateGameAndroid()
        {
            Application.OpenURL("market://details?id=" + Application.identifier);
            SaveUtils.PutValue(SaveKeysCommon.GameWasRated, true);
            yield return null;
        }
    }
}
#endif
