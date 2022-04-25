using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class settingsUI : MonoBehaviour
{
    [SerializeField]
    private Animator musicFrameAnim, audioFrameAnim;
    [SerializeField]
    private GameObject confirmedRestore, playingCanvas, confirmedAdsDisabled;
    [SerializeField]
    private TextMeshProUGUI xText, yText;
    [SerializeField]
    private Color[] colors;
    [SerializeField]
    private Sprite[] crosshairs;
    [SerializeField]
    private Image crosshairVP, colorVP, finalCrosshair;
    private int currentColor = 0, currentCH = 0;
    public static Action<Color, Sprite> setCrosshair;
    public static Action<float, float> playerSens;
    public static Action changeLayoutAction;
    public float xSens, ySens;
    [SerializeField]
    private Slider xSlider, ySlider;


    public void SetSettings()
    {
        if (PlayerPrefs.GetInt("MusicVolume", 0) != 0) //Music saved as ON
        {
            musicON();
        } else { //Music set to OFF
            musicOFF();
        }

        if (PlayerPrefs.GetInt("AudioVolume", 1) != 0) //Effects saved as ON
        {
            audioON();
        } else {
            audioOFF();
        }
        //Get player saved settings
        currentColor = PlayerPrefs.GetInt("CHColor", 0);
        currentCH = PlayerPrefs.GetInt("CH", 0);
        xSens = PlayerPrefs.GetFloat("xSens", 0.6f);
        ySens = PlayerPrefs.GetFloat("ySens", 0.6f);

        //Set player saved settings
        changeColor(currentColor);
        changeCH(currentCH);
        xSlider.value = xSens * 20f;
        ySlider.value = ySens * 20f;
        changeSensitivityX(xSens * 20f);
        changeSensitivityY(ySens * 20f);
        setCrosshair(colors[currentColor], crosshairs[currentCH]);
    }

    //Music toggle
    public void musicOFF()
    {
        PlayerPrefs.SetInt("MusicVolume", 0);
        musicFrameAnim.SetBool("left", false);
        musicFrameAnim.SetBool("right", true);
        AudioManager.audioManager.setMusicVol(0);
    }

    public void musicON()
    {
        PlayerPrefs.SetInt("MusicVolume", 1);
        musicFrameAnim.SetBool("left", true);
        musicFrameAnim.SetBool("right", false);
        AudioManager.audioManager.setMusicVol(1);
    }

    //SFX toggle
    public void audioOFF()
    {
        PlayerPrefs.SetInt("AudioVolume", 0);
        audioFrameAnim.SetBool("left", false);
        audioFrameAnim.SetBool("right", true);
        AudioManager.audioManager.setEffectsVol(0);
    }

    public void audioON()
    {
        PlayerPrefs.SetInt("AudioVolume", 1);
        audioFrameAnim.SetBool("left", true);
        audioFrameAnim.SetBool("right", false);
        AudioManager.audioManager.setEffectsVol(1);
    }

    //Crosshair color changer
    public void nextColor()
    {
        if (currentColor < colors.Length - 1)
        {
            currentColor++;
        } else {
            currentColor = 0;
        }
        changeColor(currentColor);
    }

    public void prevColor()
    {
        if (currentColor == 0)
        {
            currentColor = colors.Length - 1;
        } else {
            currentColor --;
        }
        changeColor(currentColor);
    }

    //Crosshair shape changer
    public void nextCH()
    {
        if (currentCH < crosshairs.Length - 1)
        {
            currentCH++;
        } else {
            currentCH = 0;
        }
        changeCH(currentCH);
    }
    public void prevCH()
    {
        if (currentCH == 0)
        {
            currentCH = crosshairs.Length - 1;
        } else {
            currentCH --;
        }
        changeCH(currentCH);
    }

    //Save crosshair color
    void changeColor(int colorNum)
    {
        colorVP.color = colors[colorNum];
        // crosshairVP.color = colors[colorNum];
        PlayerPrefs.SetInt("CHColor", colorNum);
        finalCrosshair.color = colors[colorNum];
    }

    //Save crosshair shape
    void changeCH(int crosshair)
    {
        crosshairVP.sprite = crosshairs[crosshair];
        PlayerPrefs.SetInt("CH", crosshair);
        finalCrosshair.sprite = crosshairs[crosshair];
    }

    public void closeSettings()
    {
        setSensitivity();
        setSettings();
        gameObject.SetActive(false);
    }

    public void openSettings()
    {
        gameObject.SetActive(true);
    }
    
    //Change aim sensitivity
    public void changeSensitivityX(float value)
    {
        // xSens = Mathf.Round(value * 5.0f) / 5f;
        xSens = value/20f;
        xText.text = (xSens * 2f).ToString("0.00");
    }
    public void changeSensitivityY(float value)
    {
        // ySens = Mathf.Round(value * 5.0f) / 5f;
        ySens = value/20f;
        yText.text = (ySens * 2f).ToString("0.00");
    }

    //Restore purchases
    void confirmRestore()
    {
        confirmedRestore.SetActive(true);
    }

    //Open custom layout panel
    public void changeLayout()
    {
        playingCanvas.SetActive(true);
        changeLayoutAction();
    }

    //Save aim sensitivity
    void setSensitivity()
    {
        PlayerPrefs.SetFloat("xSens", xSens);
        PlayerPrefs.SetFloat("ySens", ySens);
        if (playerSens != null)
        {
            playerSens(xSens, ySens);
        }
    }

    public void removeAds()
    {
        if (savedPlayerData.instance.checkIfPurchased("com.ath.disableads"))
        {
            confirmedAdsDisabled.GetComponentInChildren<TextMeshProUGUI>().text = "Ads already disabled.";
            confirmedAdsDisabled.SetActive(true);
        } else {
            IAPManager.instance.BuyProduct("com.ath.disableads");
        }
    }

    void OnEnable()
    {
        savedPlayerData.confirmRestoreAction += confirmRestore;
        setSettings();
    }
    void OnDisable()
    {
        savedPlayerData.confirmRestoreAction -= confirmRestore;
        setSensitivity();
        setSettings();
        AudioManager.audioManager.sendcrosshair();
        PlayerPrefs.Save();
    }
}

