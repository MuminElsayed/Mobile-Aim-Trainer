using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


[System.Serializable]
public class weaponPreview
{
    public GameObject gameObject;
    public string name;
}
[System.Serializable]
public class colorThemesPreview
{
    public string name;
    public Color color;
    public Texture texture;
}
public class WeaponChangerUI : MonoBehaviour
{
    [SerializeField]
    private weaponPreview[] weapons;
    [SerializeField]
    private colorThemesPreview[] colorThemes;
    [SerializeField]
    Texture lightTex, darkTex;
    [SerializeField]
    private TextMeshProUGUI weaponName, equipBuyButton, equipBuyThemeButton;
    [SerializeField]
    private Image equipBuyButtonFrame, equipBuyButtonFrameThemes;
    [SerializeField]
    private GameObject[] skinsObjects, themesObjects;
    [SerializeField]
    private GameObject purchaseDetails;
    private int currentWep = 0, equippedWeapon, weaponSkinNum, previewThemeNum, equippedTheme;
    public string weaponPurchaseID, themePurchaseID, skinName = "Default", equippedWeaponID;
    public bool canEquip, canEquipTheme;
    public static Action<int, Color, Texture> changeTheme;
    public static Action<int, Color, Texture> saveTheme;
    public static Action<int> changeSkin, updateProductPrice;

    void Awake()
    {
        equippedWeaponID = PlayerPrefs.GetString("EquippedWeapon", "default"); //Gets weapon & skin
        equippedWeapon = PlayerPrefs.GetInt("PlayerWeapon", 0);
        equippedTheme = PlayerPrefs.GetInt("Theme", 0);
        previewThemeNum = 0;
        skinName = "Default";
    }

    void updateProductIDWeapon()
    {
        // weaponPurchaseID = "com.athaim.ath." + weapons[currentWep].name.Replace(" ", "").Replace("-", "").ToLower() + skinName.Replace(" ", "").ToLower();
        weaponPurchaseID = "com.athaim.ath." + weaponName.text.Replace(" ", "").Replace("-", "").ToLower();
        if (!string.Equals(equippedWeaponID, weaponPurchaseID) || string.Equals(equippedWeaponID, "Default")) //Already equipped or starter weapon
        {
            if (checkPurchase(weaponPurchaseID)) //Weapon already purchased, set to can equip it
            {
                setButtonToEquip(equipBuyButton, equipBuyButtonFrame);
                canEquip = true;
                //Add equip mechanic
            } else { //Weapon not yet purchases, open buying window
                setButtonToBuy(equipBuyButton, equipBuyButtonFrame);
                canEquip = false;
            }
        } else {
            setButtonToEquipped(equipBuyButton, equipBuyButtonFrame);
        }
        if (updateProductPrice != null)
            updateProductPrice(savedPlayerData.instance.getProductPrice(weaponPurchaseID));
    }
    
    void updateProductIDTheme()
    { //Updates current productID to the selected theme
        themePurchaseID = "com.athaim.ath." + colorThemes[previewThemeNum].name.Replace(" ", "").ToLower() + "theme";
        if (previewThemeNum != equippedTheme) //Check if default theme is equipped
        {
            if (checkPurchase(themePurchaseID)) //True if theme already purchased
            {
                setButtonToEquip(equipBuyThemeButton, equipBuyButtonFrameThemes);
                canEquipTheme = true;
            } else { //Set to buy
                setButtonToBuy(equipBuyThemeButton, equipBuyButtonFrameThemes);
                canEquipTheme = false;
            }
        } else {
            setButtonToEquipped(equipBuyThemeButton, equipBuyButtonFrameThemes);
        }
        if (updateProductPrice != null)
            updateProductPrice(savedPlayerData.instance.getProductPrice(themePurchaseID));
    }

    public void EquipPurchaseProduct()
    {
        if (canEquip)
        {
            setButtonToEquipped(equipBuyButton, equipBuyButtonFrame);
            equippedWeaponID = weaponPurchaseID;
            equippedWeapon = currentWep;
            PlayerPrefs.SetString("EquippedWeapon", weaponPurchaseID);
            PlayerPrefs.SetInt("WeaponSkin", weaponSkinNum); //Save equipped skin number
        } else {
            // IAPManager.instance.BuyProduct(weaponPurchaseID);
            savedPlayerData.instance.buyProduct(weaponPurchaseID, savedPlayerData.instance.getProductPrice(weaponPurchaseID)); //ID & Product Price
        }
    }
    
    public void buyEquipTheme()
    {
        if (canEquipTheme)
        {
            PlayerPrefs.SetInt("Theme", previewThemeNum);
            equippedTheme = previewThemeNum;
            setButtonToEquipped(equipBuyThemeButton, equipBuyButtonFrameThemes);
            if (previewThemeNum == 0)
            {
                saveTheme(previewThemeNum, colorThemes[previewThemeNum].color, lightTex); //Light mode
            } else {
                saveTheme(previewThemeNum, colorThemes[previewThemeNum].color, darkTex); //Dark mode
            }
        } else {
            // IAPManager.instance.BuyProduct(themePurchaseID);
            savedPlayerData.instance.buyProduct(themePurchaseID, savedPlayerData.instance.getProductPrice(weaponPurchaseID)); //ID & Product Price
        }
    }

    void setButtonToEquip(TextMeshProUGUI button, Image buttonFrame)
    {
        button.text = "Equip";
        button.color = Color.white;
        buttonFrame.color = Color.white;
    }

    void setButtonToEquipped(TextMeshProUGUI button, Image buttonFrame)
    {
        button.text = "Equipped!";
        button.color = Color.green;
        buttonFrame.color = Color.green;
    }

    void setButtonToBuy(TextMeshProUGUI button, Image buttonFrame)
    {
        button.text = "Buy";
        button.color = Color.yellow;
        buttonFrame.color = Color.yellow;
    }

    void changeWeapon(int num)
    {
        foreach (weaponPreview item in weapons)
        {
            item.gameObject.SetActive(false);
        }
        weapons[num].gameObject.SetActive(true);
        weaponName.text = weapons[num].name + " " + skinName;
        updateProductIDWeapon();
    }

    public void changeSkinName(string name)
    {
        skinName = name;
        weaponName.text = weapons[currentWep].name + " " + skinName;
        updateProductIDWeapon();
    }

    public void changeSkinNum(int num)
    {
        weaponSkinNum = num;
    }

    public void changePreviewTheme(int themeNum)
    {
        weaponName.text = colorThemes[themeNum].name;
        previewThemeNum = themeNum;
        if (themeNum == 0)
        {
            changeTheme(themeNum, colorThemes[themeNum].color, lightTex); //Light mode
        } else {
            changeTheme(themeNum, colorThemes[themeNum].color, darkTex); //Dark mode
        }
        updateProductIDTheme();
    }

    public bool checkPurchase(string productID)
    {
        return savedPlayerData.instance.checkIfPurchased(productID);
    }

    public void nextWeapon()
    {
        if (currentWep < weapons.Length - 1)
        {
            currentWep++;
        } else {
            currentWep = 0;
        }
        changeWeapon(currentWep);
    }

    public void prevWeapon()
    {
        if (currentWep == 0)
        {
            currentWep = weapons.Length - 1;
        } else {
            currentWep--;
        }
        changeWeapon(currentWep);
    }

    public void changeToThemes()
    {
        foreach (GameObject item in skinsObjects)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in themesObjects)
        {
            item.SetActive(true);
        }
        updateProductPrice(0);
    }

    public void changeToSkins()
    {
        foreach (GameObject item in skinsObjects)
        {
            item.SetActive(true);
        }
        foreach (GameObject item in themesObjects)
        {
            item.SetActive(false);
        }
        if (updateProductPrice != null)
                updateProductPrice(savedPlayerData.instance.getProductPrice(weaponPurchaseID));
    }

    public void enablePurchaseDetails()
    {
        purchaseDetails.SetActive(true);
    }

    void OnEnable()
    {
        savedPlayerData.updateProductStatus += updateProductIDWeapon;
        savedPlayerData.updateProductStatus += updateProductIDTheme;
        savedPlayerData.enablePurchaseDetails += enablePurchaseDetails;
        changeToSkins();
        changeSkinName("Default");
        changeWeapon(0);
        changePreviewTheme(0);
        changeSkin(0);
        purchaseDetails.SetActive(false);
    }

    void OnDisable()
    {
        savedPlayerData.updateProductStatus -= updateProductIDWeapon;
        savedPlayerData.updateProductStatus -= updateProductIDTheme;
        savedPlayerData.enablePurchaseDetails -= enablePurchaseDetails;
        changeSkinName("Default");
        currentWep = 0;
        previewThemeNum = 0;
        changeWeapon(0);
        PlayerPrefs.SetInt("PlayerWeapon", equippedWeapon);
        changePreviewTheme(equippedTheme);
        if (equippedTheme == 0)
        {
            saveTheme(equippedTheme, colorThemes[equippedTheme].color, lightTex); //Light mode
        } else {
            saveTheme(equippedTheme, colorThemes[equippedTheme].color, darkTex); //Dark mode
        }
    }
}
