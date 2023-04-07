using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mazing.common.Runtime.Utils;
using Unity.Services.Core;
using UnityEngine.Events;
using UnityEngine.Purchasing;

namespace mazing.common.Runtime.Managers.IAP
{
    public enum EShopProductResult
    {
        Pending,
        Success,
        Fail
    }
    
    // ReSharper disable once InconsistentNaming
    public class IAP_ProductInfo : ProductInfo
    {
        public decimal                  LocalizedPrice       { get; set; }
        public string                   LocalizedPriceString { get; set; }
        public string                   Currency             { get; set; }
        public bool                     HasReceipt           { get; set; }
        public Func<EShopProductResult> Result               { get; set; }

        public IAP_ProductInfo() { }
    }
    
    public abstract class UnityIapShopManagerBase : ShopManagerBase, IStoreListener
    {
        #region nonpublic members

        private IStoreController   m_StoreController;
        private IExtensionProvider m_StoreExtensionProvider;

        private readonly Dictionary<int, List<UnityAction>> m_OnPurchaseActionsDictDict =
            new          Dictionary<int, List<UnityAction>>();
        private readonly Dictionary<int, List<UnityAction>> m_OnDeferredActionsDictDict =
            new Dictionary<int, List<UnityAction>>();

        #endregion

        #region inject

        private ILocalizationManager LocalizationManager { get; }

        protected UnityIapShopManagerBase(ILocalizationManager _LocalizationManager)
        {
            LocalizationManager = _LocalizationManager;
        }

        #endregion

        #region api

        public override void Init()
        {
            if (Initialized)
                return;
            var task = InitializeGamingServices(
                InitializePurchasing,
                InitializePurchasing);
            Task.Run(() => task);
        }
        
        public virtual void OnInitialized(IStoreController _Controller, IExtensionProvider _Extensions)
        {
            m_StoreController = _Controller;
            m_StoreExtensionProvider = _Extensions;
            Dbg.Log("Shop Manager Initialized Successfully!");
            base.Init();
        }
        
        public void OnInitializeFailed(InitializationFailureReason _Error)
        {
            string fullMessage = "Shop Manager Initialization Failed!"
                                 + "\n" + "Error: " + _Error;
            Dbg.LogWarning(fullMessage);
        }

        public void OnInitializeFailed(InitializationFailureReason _Error, string? _Message)
        {
            string fullMessage = "Shop Manager Initialization Failed!"
                                 + "\n" + "Error: " + _Error
                                 + "\n" + "Message: " + _Message;
            Dbg.LogWarning(fullMessage);
        }

        public void OnPurchaseFailed(Product _Product, PurchaseFailureReason _FailureReason)
        {
            string fullMessage = "Product Purchase Failed!"
                                 + "\n" + "Id: " + _Product.definition.id
                                 + "\n" + "Type: " + _Product.definition.type
                                 + "\n" + "Title: " + _Product.metadata.localizedTitle
                                 + "\n" + "Price: " + _Product.metadata.localizedPriceString
                                 + "\n" + "Failure Reason: " + _FailureReason;
            Dbg.LogWarning(fullMessage);
        }

        public override void Purchase(int _Key)
        {
            if (!NetworkUtils.IsInternetConnectionAvailable())
            {
                string oopsText = LocalizationManager.GetTranslation("oops");
                string noIntConnText = LocalizationManager.GetTranslation("no_internet_connection");
                ShowAlertDialogAction?.Invoke(oopsText, noIntConnText);
                return;
            }
            Dbg.Log(nameof(Purchase) + ": " + _Key);
            string id = GetProductId(_Key);
            BuyProductID(id);
        }

        public override bool RateGame()
        {
            if (NetworkUtils.IsInternetConnectionAvailable())
                return true;
            string oopsText = LocalizationManager.GetTranslation("oops");
            string noIntConnText = LocalizationManager.GetTranslation("no_internet_connection");
            ShowAlertDialogAction?.Invoke(oopsText, noIntConnText);
            Dbg.LogWarning("Failed to rate game: No internet connection.");
            return false;
        }

        public override IAP_ProductInfo GetItemInfo(int _Key)
        {
            var res = new IAP_ProductInfo();
            GetProductItemInfo(_Key, ref res);
            return res;
        }

        public override void AddPurchaseAction(int _ProductKey, UnityAction _Action)
        {
            AddTransactionAction(_ProductKey, _Action, false);
        }

        public override void AddDeferredAction(int _ProductKey, UnityAction _Action)
        {
            AddTransactionAction(_ProductKey, _Action, true);
        }

        public override void RestorePurchases()
        {
            if (!NetworkUtils.IsInternetConnectionAvailable())
            {
                string oopsText = LocalizationManager.GetTranslation("oops");
                string noIntConnText = LocalizationManager.GetTranslation("no_internet_connection");
                ShowAlertDialogAction?.Invoke(oopsText, noIntConnText);
                Dbg.LogWarning("Failed to restore purchases: No internet connection.");
                return;
            }
            if (!Initialized)
            {
                string oopsText = LocalizationManager.GetTranslation("oops");
                string failToRestoreText = LocalizationManager.GetTranslation("service_connection_error");
                ShowAlertDialogAction?.Invoke(oopsText, failToRestoreText);
                Dbg.LogWarning("RestorePurchases FAIL. Not initialized.");
                return;
            }
            foreach (var product in Products.Where(_P => _P.Type == ProductType.NonConsumable))
            {
                var info = GetItemInfo(product.PurchaseKey);
                Cor.Run(Cor.WaitWhile(
                    () => info.Result() == EShopProductResult.Pending,
                    () =>
                    {
                        if (info.Result() == EShopProductResult.Fail)
                            return;
                        if (!info.HasReceipt)
                            return;
                        if (!m_OnPurchaseActionsDictDict.ContainsKey(product.PurchaseKey))
                            return;
                        var actionsList = m_OnPurchaseActionsDictDict[product.PurchaseKey];
                        foreach (var action in actionsList)
                            action?.Invoke();
                    }));
            }
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs _Args)
        {
            string id = _Args.purchasedProduct.definition.id;
            var info = Products
                .FirstOrDefault(_P => _P.Id == _Args.purchasedProduct.definition.id);
            if (info == null)
            {
                Dbg.LogError($"Product with id {id} does not have product");
                return PurchaseProcessingResult.Complete;
            }
            if (!m_OnPurchaseActionsDictDict.ContainsKey(info.PurchaseKey))
            {
                Dbg.LogWarning($"Product with id {id} does not have purchase action");
                return PurchaseProcessingResult.Complete;
            }
            var actionsList = m_OnPurchaseActionsDictDict[info.PurchaseKey];
            foreach (var action in actionsList)
                action?.Invoke();
            Dbg.Log($"ProcessPurchase: PASS. Product: '{_Args.purchasedProduct.definition.id}'");
            return PurchaseProcessingResult.Complete;
        }

        #endregion
        
        #region nonpublic methods

        private async Task InitializeGamingServices(
            UnityAction _OnSucceedToInitialize,
            UnityAction _OnFailedToInitialize)
        {
            try
            {
                var options = new InitializationOptions()
                    .SetOption("environment", "production");
                await UnityServices.InitializeAsync(options);
                _OnSucceedToInitialize?.Invoke();
            }
            catch (Exception exception) {
                Dbg.LogError(exception);
                _OnFailedToInitialize?.Invoke();
            }
        }
        
        private void InitializePurchasing()
        {
            var builder = GetBuilder();
            foreach (var kvp in Products)
                builder.AddProduct(kvp.Id, kvp.Type);
            UnityPurchasing.Initialize(this, builder);
        }
        
        private void AddTransactionAction(int _ProductKey, UnityAction _Action, bool _Deferred)
        {
            var actionsDictDict = _Deferred ?
                m_OnDeferredActionsDictDict : m_OnPurchaseActionsDictDict;
            if (!actionsDictDict.ContainsKey(_ProductKey))
                actionsDictDict.Add(_ProductKey, new List<UnityAction>());
            var actionsList = actionsDictDict[_ProductKey];
            actionsList.Add(_Action);
        }
        


        protected virtual ConfigurationBuilder GetBuilder()
        {
            var module = StandardPurchasingModule.Instance();
            var builder = ConfigurationBuilder.Instance(module);
            return builder;
        }

        private void BuyProductID(string _ProductId)
        {
            if (!Initialized)
            {
                string oopsText = LocalizationManager.GetTranslation("oops");
                string failToBuyProduct = LocalizationManager.GetTranslation("service_connection_error");
                ShowAlertDialogAction?.Invoke(oopsText, failToBuyProduct);
                Dbg.LogWarning("BuyProductID FAIL. Not initialized.");
                return;
            }
            var product = m_StoreController.products.WithID(_ProductId);
            if (product == null || !product.availableToPurchase)
            {
                Dbg.LogWarning("BuyProductID: FAIL. Not purchasing product, " +
                               "either is not found or is not available for purchase");
                return;
            }
            Dbg.Log($"Purchasing product asynchronously: '{product.definition.id}'");
            m_StoreController.InitiatePurchase(product);
        }

        private void GetProductItemInfo(int _Key, ref IAP_ProductInfo _Args)
        {
            _Args.PurchaseKey = _Key;
            _Args.Result = () => EShopProductResult.Pending;
            if (!Initialized)
            {
                Dbg.LogWarning($"{nameof(UnityIapShopManagerBase)}: Get Product Item Info Failed. Not initialized.");
                _Args.Result = () => EShopProductResult.Fail;
                return;
            }
            string id = GetProductId(_Key);
            var product = m_StoreController.products.set.FirstOrDefault(_P => _P.definition.id == id);
            if (product == null)
            {
                Dbg.LogWarning($"{nameof(UnityIapShopManagerBase)}: " +
                               $"Get Product Item Info Failed. Product with id {id} does not exist");
                _Args.Result = () => EShopProductResult.Fail;
                return;
            }
            _Args.HasReceipt           = product.hasReceipt;
            _Args.Currency             = product.metadata.isoCurrencyCode;
            _Args.LocalizedPrice       = product.metadata.localizedPrice;
            _Args.LocalizedPriceString = product.metadata.localizedPriceString;
            _Args.Result               = () => EShopProductResult.Success;
        }
        
        protected void OnDeferredPurchase(Product _Product)
        {
            string id = _Product.definition.id;
            var info = Products.FirstOrDefault(_P => _P.Id == _Product.definition.id);
            if (info == null)
            {
                Dbg.LogError($"Product with id {id} does not have product");
                return;
            }
            if (!m_OnPurchaseActionsDictDict.ContainsKey(info.PurchaseKey))
            {
                Dbg.LogWarning($"Product with id {id} does not have purchase action");
                return;
            }
            var actionsList = m_OnPurchaseActionsDictDict[info.PurchaseKey];
            foreach (var action in actionsList)
            {
                action?.Invoke();
                if (action != null)
                {
                    Dbg.Log($"Purchase of {_Product.definition.id} is deferred");
                    action();

                }
                else
                {
                    Dbg.LogWarning("Deferred action was not set of " +
                                   $"product with id {_Product.definition.id} was not set" +
                                   ", but it's okay, let this product be free");
                }
            }
        }

        #endregion
    }
}