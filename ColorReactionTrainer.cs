using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Colors
{
    public Color color;
    public GameObject button;
}
public class ColorReactionTrainer : MonoBehaviour
{
    [SerializeField]
    private Image panel;
    [SerializeField]
    private Colors[] colors;
    private int correctColor, lastColor, difficulty;
    public float timeTaken, score, changeTime;
    void Start()
    {
        panel.color = Color.white;
        difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        setDifficulty();
        setRandomColor();
    }

    void changeColor()
    {
        CancelInvoke();
        setRandomColor();
        Invoke("changeColor", changeTime);
        timeTaken = 0;
    }

    void setRandomColor()
    {
        while(correctColor == lastColor)
        {
            correctColor = UnityEngine.Random.Range(0, colors.Length);
        }
        panel.color = colors[correctColor].color;
        lastColor = correctColor;
    }

    void Update()
    {
        score = Mathf.RoundToInt(100f - timeTaken * 20f);
        timeTaken += Time.deltaTime;
    }

    public void submitColor(int number)
    {
        if (number == correctColor)
        {
            //Correct answer
            GameManager.addScore(Mathf.RoundToInt(100f - timeTaken * 20f) * (difficulty + 1));
            changeColor();
        } else {
            //Wrong answer
            GameManager.addScore(-50 * (difficulty + 1));
            changeColor();
        }
    }

    void setDifficulty()
    {
        if (difficulty == 0)
        {
            for (int i = 4; i < colors.Length; i++)
            {
                colors[i].button.SetActive(false);
            }
            changeTime = 5f;
            System.Array.Resize<Colors>(ref colors, 4);
        } else if (difficulty == 1)
        {
            for (int i = 6; i < colors.Length; i++)
            {
                colors[i].button.SetActive(false);
            }
            changeTime = 3f;
            System.Array.Resize<Colors>(ref colors, 6);
        } else {
            for (int i = 8; i < colors.Length; i++)
            {
                colors[i].button.SetActive(false);
            }
            changeTime = 1.5f;
            System.Array.Resize<Colors>(ref colors, 8);
        }
    }
}