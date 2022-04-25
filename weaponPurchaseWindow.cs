using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class weaponPurchaseWindow : MonoBehaviour
{
    public static weaponPurchaseWindow instance;
    [SerializeField]
    private GameObject confirmButton, buyCredsButton, descObj;
    private TextMeshProUGUI boxText;

    void Awake()
    {
        instance = this;
        boxText = descObj.GetComponent<TextMeshProUGUI>();
    }

    public void notEnoughCreds()
    {
        boxText.text = "Not enough credits.";
        buyCredsButton.SetActive(true);
        confirmButton.SetActive(false);
    }   

    public void purchaseSuccess()
    {
        boxText.text = "Purchase Successful!";
        buyCredsButton.SetActive(false);
        confirmButton.SetActive(true);
    }
}
