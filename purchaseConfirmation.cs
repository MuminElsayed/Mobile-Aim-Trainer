using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class purchaseConfirmation : MonoBehaviour
{
    public static purchaseConfirmation instance;

    [SerializeField]
    private GameObject descriptionObj, titleObj, loadingObj;
    private TextMeshProUGUI descriptionText, titleText;
    public string defaultTitle, defaultDesc;

    void Awake()
    {
        instance = this;
        descriptionText = descriptionObj.GetComponent<TextMeshProUGUI>();
        titleText = titleObj.GetComponent<TextMeshProUGUI>();
        defaultTitle = titleText.text;
        defaultDesc = descriptionText.text;
    }

    public void updateDesc(string desc)
    {
        loadingObj.SetActive(false);
        descriptionObj.SetActive(true);
        descriptionText.text = desc;
    }

    public void updateTitle(string text)
    {
        loadingObj.SetActive(false);
        titleText.text = text;
    }

    void OnDisable()
    {
        titleText.text = defaultTitle;
        descriptionText.text = defaultDesc;
        loadingObj.SetActive(true);
        descriptionObj.SetActive(false);
    }
}
