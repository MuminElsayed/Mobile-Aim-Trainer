using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class mainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainmenuUI, trainingPacksUI, settingsUI, optionsUI, changeNameUI, playerNameButton, SkinsUI, WeaponsUI, buyCoinsMenu, purchaseInfoMenu;
    private string playerName;
    private TextMeshProUGUI playerNameButtonText;
    public static Action gameStart;
    [SerializeField]
    private Camera mainCam, secCam;

    void Start()
    {
        mainCam = Camera.main;
        openMainMenu();
        setDifficulty(0);
        playerNameButtonText = playerNameButton.GetComponentInChildren<TextMeshProUGUI>();
        playerName = PlayerPrefs.GetString("playerName", "default");
        if (string.Equals(playerName, "default")) //No name set
        {
            playerName = "Player" + UnityEngine.Random.Range(0, 10000);
            changePlayerName(playerName);
            print("randm name set");
        } else {
            changePlayerName(playerName);
        }
    }
    void setAudio()
    {
        if (PlayerPrefs.GetInt("MusicVolume", 1) != 0) //Music saved as ON
        {
            AudioManager.audioManager.setMusicVol(1);
        } else { //Music set to OFF
            AudioManager.audioManager.setMusicVol(0);
        }

        if (PlayerPrefs.GetInt("AudioVolume", 1) != 0) //Effects saved as ON
        {
            AudioManager.audioManager.setEffectsVol(1);
        } else {
            AudioManager.audioManager.setEffectsVol(0);
        }
    }
    public void openTrainingPacks()
    {
        trainingPacksUI.SetActive(true);
        optionsUI.SetActive(false);
    }
    public void openSettings()
    {
        settingsUI.SetActive(true);
    }

    public void openMainMenu()
    {
        mainCam.gameObject.SetActive(true);
        playerNameButton.SetActive(true);
        optionsUI.SetActive(true);

        purchaseInfoMenu.SetActive(false);
        buyCoinsMenu.SetActive(false);
        secCam.gameObject.SetActive(false);
        mainmenuUI.SetActive(true);
        settingsUI.SetActive(false);
        changeNameUI.SetActive(false);
        trainingPacksUI.SetActive(false);
        SkinsUI.SetActive(false);
        WeaponsUI.SetActive(false);
    }

    public void setDifficulty(int mode) //0 easy, 1 medium, 2 hard
    {
        PlayerPrefs.SetInt("Difficulty", mode);
    }

    public void openNameChangeUI()
    {
        changeNameUI.SetActive(true);
    }

    public void openSkins()
    {
        secCam.gameObject.SetActive(true);
        mainCam.gameObject.SetActive(false);
        mainmenuUI.SetActive(false);
        playerNameButton.SetActive(false);
        SkinsUI.SetActive(true);
        WeaponsUI.SetActive(true);
    }

    public void buyCoins()
    {
        buyCoinsMenu.SetActive(true);
        purchaseInfoMenu.SetActive(false);
    }

    public void changePlayerName(string newName)
    {
        if (newName.Length <= 20)
        {
            changeNameUI.SetActive(false);
            playerName = newName;
            PlayerPrefs.SetString("playerName", newName);
            playerNameButtonText.text = "Welcome " + playerName + ", tap here to change your name.";
        }
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
