using System.Collections;
using System.Collections.Generic;
using System;
using Gamee_Hiukka.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Gamee_Hiukka.Control
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        private IStoreController myStoreController;
        private IExtensionProvider myExtensionProvider;

        public bool dontDestroyOnload = true;
        public static IAPManager Instance;

        private Action _actionRemoveAds;
        private Action _actionAddCoinPack1;
        private Action _actionAddCoinPack2;
        private Action _actionUnlockAllSkin;
        private Action _actionCoinX2;
        private Action _actionCombo;

        void OnEnable() 
        {
            if (dontDestroyOnload) DontDestroyOnLoad(gameObject);

            if (Instance == null) Instance = FindObjectOfType<IAPManager>();
        }

        public void Start()
        {
            if (myStoreController == null)
            {
                Initialize();
            }
        }
        public void Initialize() 
        {
            if (IsInitialized())
            {
                return;
            }

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(DataParam.REMOVE_ADS, ProductType.NonConsumable);
            builder.AddProduct(DataParam.ADD_COIN_PACK_1, ProductType.Consumable);
            builder.AddProduct(DataParam.ADD_COIN_PACK_2, ProductType.Consumable);
            builder.AddProduct(DataParam.UNLOCK_ALL_SKIN, ProductType.NonConsumable);
            builder.AddProduct(DataParam.COIN_X2, ProductType.NonConsumable);
            builder.AddProduct(DataParam.COMBO, ProductType.NonConsumable);
            UnityPurchasing.Initialize(this, builder);
        }

        public bool IsInitialized() 
        {
            return myStoreController != null && myStoreController != null;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("Initialized is Complete!");
            myStoreController = controller;
            myExtensionProvider = extensions;
        }

        public void BuyRemoveAds(Action actionRemoveAds) 
        {
            this._actionRemoveAds = actionRemoveAds;
            BuyProductID(DataParam.REMOVE_ADS);
        }

        public void BuyAddCoinPack1(Action actionAddCoinPack1) 
        {
            this._actionAddCoinPack1 = actionAddCoinPack1;
            BuyProductID(DataParam.ADD_COIN_PACK_1);
        }

        public void BuyAddCoinPack2(Action actionAddCoinPack2)
        {
            this._actionAddCoinPack2 = actionAddCoinPack2;
            BuyProductID(DataParam.ADD_COIN_PACK_2);
        }

        public void BuyUnlockAllSkin(Action actionUnlockAllSkin) 
        {
            this._actionUnlockAllSkin = actionUnlockAllSkin;
            BuyProductID(DataParam.UNLOCK_ALL_SKIN);
        }

        public void BuyCoinX2(Action actionCoinX2)
        {
            this._actionCoinX2 = actionCoinX2;
            BuyProductID(DataParam.COIN_X2);
        }

        public void BuyCombo(Action actionCombo)
        {
            this._actionCombo = actionCombo;
            BuyProductID(DataParam.COMBO);
        }
        void BuyProductID(string productId)
        {
            if (IsInitialized())
            {
                Product product = myStoreController.products.WithID(productId);

                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));

                    myStoreController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        public void RestorePurchases()
        {
            if (!IsInitialized())
            {
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                Debug.Log("RestorePurchases started ...");

                var apple = myExtensionProvider.GetExtension<IAppleExtensions>();
                apple.RestoreTransactions((result) => {
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                });
            }
            else
            {
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.Log("Initialized is Fail! " + error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {

        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            if(string.Equals(args.purchasedProduct.definition.id, DataParam.REMOVE_ADS, System.StringComparison.Ordinal))
            {
                _actionRemoveAds?.Invoke();
            }
            else if(string.Equals(args.purchasedProduct.definition.id, DataParam.ADD_COIN_PACK_1, System.StringComparison.Ordinal)) 
            {
                _actionAddCoinPack1?.Invoke();
            }
            else if (string.Equals(args.purchasedProduct.definition.id, DataParam.ADD_COIN_PACK_2, System.StringComparison.Ordinal))
            {
                _actionAddCoinPack2?.Invoke();
            }
            else if (string.Equals(args.purchasedProduct.definition.id, DataParam.UNLOCK_ALL_SKIN, System.StringComparison.Ordinal))
            {
                _actionUnlockAllSkin?.Invoke();
            }
            else if (string.Equals(args.purchasedProduct.definition.id, DataParam.COIN_X2, System.StringComparison.Ordinal))
            {
                _actionCoinX2?.Invoke();
            }
            else if (string.Equals(args.purchasedProduct.definition.id, DataParam.COMBO, System.StringComparison.Ordinal))
            {
                _actionCombo?.Invoke();
            }

            return PurchaseProcessingResult.Complete;
        }
    }
}

