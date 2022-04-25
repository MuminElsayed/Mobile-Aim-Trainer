using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    void Awake()
    {
        //Initialize google ads SDK
        MobileAds.Initialize(initStatus => { });
    }
}
