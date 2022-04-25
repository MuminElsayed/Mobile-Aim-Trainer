using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class packDetails
{
    public Sprite sprite;
    public string name, description, sceneName;
    public bool hasDifficulty;
}

public class trainingPacksPicker : MonoBehaviour
{
    [SerializeField]
    private packDetails[] trainingPacks;
    [SerializeField]
    private Image packIMG;
    [SerializeField]
    private TextMeshProUGUI packTitle, packDesc;
    [SerializeField]
    private GameObject difficultyOptions;
    private int packCounter = 0, numberOfPacks;

    void Start()
    {
        setPackDetails(trainingPacks[packCounter].sprite, trainingPacks[packCounter].name, trainingPacks[packCounter].description);
        numberOfPacks = trainingPacks.Length;
    }

    void setPackDetails(Sprite sprite, string title, string description)
    {
        packIMG.sprite = sprite;
        packTitle.text = title;
        packDesc.text = description;
        checkDifficulty();
    }
    public void nextPack()
    {
        if (packCounter == numberOfPacks - 1)
        {
            packCounter = 0;
        } else {
            packCounter++;
        }
        setPackDetails(trainingPacks[packCounter].sprite, trainingPacks[packCounter].name, trainingPacks[packCounter].description);
    }
    
    public void previousPack()
    {
        if (packCounter == 0)
        {
            packCounter = numberOfPacks - 1;
        } else {
            packCounter--;
        }
        setPackDetails(trainingPacks[packCounter].sprite, trainingPacks[packCounter].name, trainingPacks[packCounter].description);
    }

    void checkDifficulty()
    {
        if (trainingPacks[packCounter].hasDifficulty)
        {
            difficultyOptions.SetActive(true);
        } else {
            difficultyOptions.SetActive(false);
        }
    }

    public void startGame()
    {
        SceneManager.LoadScene(trainingPacks[packCounter].sceneName);
    }
}