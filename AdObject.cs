using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdObject : MonoBehaviour
{
    [SerializeField]
    private string AppleAdID, PlayStoreAdID, testAdID;
    [SerializeField]
    private bool testAds;
    private string AdID;
    private InterstitialAd fullscreenAd;


    public void showAd()
    {
        if (fullscreenAd.IsLoaded() && !savedPlayerData.instance.checkIfPurchased("com.ath.disableads")) //Ads loaded and Ads are enabled
        {
            fullscreenAd.Show();
            // print("showing ad");
        }
    }
    private void OnEnable()
    {
        #if UNITY_ANDROID
            AdID = PlayStoreAdID;
        #elif UNITY_IPHONE
            AdID = AppleAdID;
        #else
            AdID = null;
        #endif

        //Init ad
        if (!testAds)
        {
            fullscreenAd = new InterstitialAd(AdID);
            // Debug.Log("set real ad");
        } else {
            fullscreenAd = new InterstitialAd(testAdID);
            // Debug.Log("set test ad");
        }

        //Create ad request
        AdRequest request = new AdRequest.Builder().Build();
        //Load ad request
        fullscreenAd.LoadAd(request);

        // Called when the ad is closed.
        this.fullscreenAd.OnAdClosed += HandleOnAdClosed;

        GameManager.showAd += showAd;
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //Reinstantiates this ad
        OnDisable();
        OnEnable();
    }

    void OnDisable()
    {
        fullscreenAd.Destroy();

        GameManager.showAd -= showAd;
    }
}
