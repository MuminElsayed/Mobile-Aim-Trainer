using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static PlayerInput playerInput;
    public static GameManager instance;
    [SerializeField]
    private TextMeshProUGUI addedScoreText;
    [SerializeField]
    private GameObject postGameCanvas, playingCanvas, pauseMenu, settingsMenu, startButton;
    public int playerScore, timeRemaining, gameTime;
    public static Action spawnEnemy, gameEnd;
    public static Action<int> addScore, updateTimeAction;
    public static Action showAd;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        playerInput = new PlayerInput();
    }

    void Start()
    {
        try
        {
            gameTime = 60;
            showPlayingCanvas();
            Time.timeScale = 0;
        } catch {
            Debug.Log("Error with canvases/ads");
        }
    }

    void Update()
    {
        // playerInput.UI.Back.started += _ => pauseGame();
    }

    public void startGame()
    {
        Time.timeScale = 1;
        startButton.SetActive(false);
        AudioManager.audioManager.sendcrosshair();
        timeRemaining = gameTime;
        InvokeRepeating("callUpdateTime", 1f, 1f);
    }

    void callUpdateTime()
    {
        timeRemaining--;
        if (timeRemaining < 1)
        {
            //End game
            sendPlayerScore();
            gameEnd();
            CancelInvoke();

            showAd();
            print("show add on end");
        }
        updateTime(timeRemaining);
    }

    void updateTime(int time)
    {
        // timeText.text = "Time: " + time.ToString();
        if (updateTimeAction != null)
        {
            updateTimeAction(time);
        }
    }

    void addPlayerScore(int score)
    {
        if (playerScore + score < 0)
        {
            playerScore = 0;
        } else {
            playerScore += score;
        }
        scoreFade(score);
    }

    void scoreFade(int number)
    {
        addedScoreText.gameObject.SetActive(false);
        addedScoreText.gameObject.SetActive(true);
        if (number > 0)
        {
            addedScoreText.text = "+" + number;
            addedScoreText.color = Color.green;
        } else {
            addedScoreText.text = number.ToString();
            addedScoreText.color = Color.red;
        }
    }

    public void showPlayingCanvas()
    {
        Time.timeScale = 1;
        postGameCanvas.SetActive(false);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        playingCanvas.SetActive(true);

        showAd();
    }

    public void pauseGame()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
            playingCanvas.SetActive(false);
            postGameCanvas.SetActive(false);
            pauseMenu.SetActive(true);
        }
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        postGameCanvas.SetActive(false);
        pauseMenu.SetActive(false);
        playingCanvas.SetActive(true);
    }

    public void showPostGame()
    {
        Time.timeScale = 0;
        PostGameCanvas.instance.playerGameTime = gameTime;
        playingCanvas.SetActive(false);
        pauseMenu.SetActive(false);
        postGameCanvas.SetActive(true);
    }

    public void openSettings()
    {
        settingsMenu.SetActive(true);
    }

    void sendPlayerScore()
    {
        PostGameCanvas.instance.playerScore = playerScore;
    }

    void OnEnable()
    {
        playerInput.Enable();
        gameEnd += showPostGame;
        addScore += addPlayerScore;
    }

    void OnDisable()
    {
        playerInput.Disable();
        gameEnd -= showPostGame;
        addScore -= addPlayerScore;
    }
}
