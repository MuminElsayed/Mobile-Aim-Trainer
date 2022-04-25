using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class savedPlayerData : MonoBehaviour
{
    public static savedPlayerData instance;

    public int currentCurrency = 0;
    [SerializeField]
    private List<string> playerPurchases = new List<string>{"com.athaim.ath.handgundefault", "com.athaim.ath.defaultthemetheme"};
    [SerializeField]
    private List<string> allPurchaseIDs;
    [SerializeField]
    private int[] allPurchasePrices;
    public static Action confirmRestoreAction, updateProductStatus, enablePurchaseDetails;
    public static Action<int> updateCurrencyAction;
    private string currency1000id = "com.ath.coins1000", currency2500id = "com.ath.coins2500", currency5000id = "com.ath.coins5000", currency10000id = "com.ath.coins10000";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public int getProductPrice(string productID)
    {
        if (allPurchaseIDs.Contains(productID))
        {
            // print("price: " + productID + allPurchasePrices[allPurchaseIDs.IndexOf(productID)]);
            return allPurchasePrices[allPurchaseIDs.IndexOf(productID)];
        } else {
            print("product not found, id: " + productID);
            return 999999;
        }
    }

    // public void addthepurchaseids(string id)
    // {
    //     allPurchaseIDs.Add(id);
    // }

    public void addToPurchases(string purchaseName)
    {
        playerPurchases.Add(purchaseName);
    }

    public bool checkIfPurchased(string productID)
    {
        //Return purchase status of an item
        if (playerPurchases.Contains(productID))
        {
            return true;
        } else {
            return false;
        }
    }

    public void RestorePurchases()
    {
        playerPurchases.RemoveRange(2, playerPurchases.Count - 2);
        resetCurrency();
        PlayerPrefs.SetInt("PlayerWeapon", 0);
        PlayerPrefs.SetInt("Theme", 0);
        PlayerPrefs.SetInt("WeaponSkin", 0);
        confirmRestoreAction();
    }

    //In game Currency
    public void addCurrency(int number)
    {
        currentCurrency += number;
    }

    public void buyProduct(string productID, int cost)
    {
        if (cost <= currentCurrency)
        {
            //Buy success
            savedPlayerData.instance.addToPurchases(productID);
            currentCurrency -= cost;
            updateCurrency();
            enablePurchaseDetails();
            weaponPurchaseWindow.instance.purchaseSuccess();
            if (updateProductStatus != null)
                {
                    updateProductStatus();
                }
        } else {
            //Fail
            enablePurchaseDetails();
            weaponPurchaseWindow.instance.notEnoughCreds();
            print("not enough currency");
            // return "Buy failed: not enough credits.";
        }
    }

    public void buy1000()
    {
        IAPManager.instance.BuyProduct(currency1000id);
    }
    public void buy2500()
    {
        IAPManager.instance.BuyProduct(currency2500id);
    }
    public void buy5000()
    {
        IAPManager.instance.BuyProduct(currency5000id);
    }
    public void buy10000()
    {
        IAPManager.instance.BuyProduct(currency10000id);
    }

    public void confirmCurrencyPurchase(string currencyid)
    {
        if (string.Equals(currencyid, currency1000id))
        {
            addCurrency(1000);
        } else if (string.Equals(currencyid, currency2500id))
        {
            addCurrency(2500);
        } else if (string.Equals(currencyid, currency5000id))
        {
            addCurrency(5000);
        } else if (string.Equals(currencyid, currency10000id))
        {
            addCurrency(10000);
        }
    }

    private void resetCurrency()
    {
        currentCurrency = 0;
    }

    public int getCurrentCurrency()
    {
        return currentCurrency;
    }

    public void updateCurrency()
    {
        PlayerPrefs.SetInt("currency", currentCurrency);
        if (updateCurrencyAction != null)
            updateCurrencyAction(currentCurrency);
    }

    void OnEnable()
    {
        updateCurrency();
        savedPlayerData.updateProductStatus += updateCurrency;
        //Load player data
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("savedPlayerData"), this);
    }

    void OnDisable()
    {
        savedPlayerData.updateProductStatus -= updateCurrency;
        //Save player data
        string jsonData = JsonUtility.ToJson(this, false);
        PlayerPrefs.SetString("savedPlayerData", jsonData);
        PlayerPrefs.Save();
    }
}
