using System;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products
    [SerializeField]
    private List<string> allPurchases;
    // public static Action updateProductStatus;


    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        // builder.AddProduct(removeAds, ProductType.NonConsumable);
        foreach (string item in allPurchases)
        {
            builder.AddProduct(item, ProductType.NonConsumable);
        }


        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    // public void transferIAPs()
    // {
    //     foreach (string product in allPurchases)
    //     {
    //         savedPlayerData.instance.addthepurchaseids(product);
    //     }
    // }


    //Step 3 Create methods
    public void BuyProduct(string productID) //Pass in product ID
    {
        BuyProductID(productID);
    }



    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, args.purchasedProduct.definition.id, StringComparison.Ordinal))
        {
            // Debug.Log("Purchase Successful"); //Purchase succesful
            // savedPlayerData.instance.addToPurchases(args.purchasedProduct.definition.id); //Add to player purchase data
            savedPlayerData.instance.confirmCurrencyPurchase(args.purchasedProduct.definition.id);
            purchaseConfirmation.instance.updateTitle("Purchase Successful!");
            purchaseConfirmation.instance.updateDesc("Credits bought successfully.");
            savedPlayerData.instance.updateCurrency();
            print("confirming purchase id: " + args.purchasedProduct.definition.id);
            // Debug.Log(args.purchasedProduct.definition.id + "added to player purchases"); //Purchase succesful
            // if (updateProductStatus != null)
            // {
            //     updateProductStatus();
            // }
        }
        else
        {
            Debug.Log("Purchase Failed");
            purchaseConfirmation.instance.updateTitle("Purchase Failed");
        }
        return PurchaseProcessingResult.Complete;
    }










    //**************************** Initialization methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                purchaseConfirmation.instance.updateTitle("Purchase Failed.");
                purchaseConfirmation.instance.updateDesc(productId + " is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
            purchaseConfirmation.instance.updateTitle("Purchase Failed.");
            purchaseConfirmation.instance.updateDesc(productId + " is not initialized.");
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

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
            savedPlayerData.instance.RestorePurchases();
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        purchaseConfirmation.instance.updateTitle("Purchase Failed.");
        purchaseConfirmation.instance.updateDesc("Purchase Canceled.");
    }

    //Quick add of the list of all products
    // void OnDisable()
    // {
    //     PlayerPrefs.SetString("json test", JsonUtility.ToJson(this, true));
    //     PlayerPrefs.Save();
    // }
}