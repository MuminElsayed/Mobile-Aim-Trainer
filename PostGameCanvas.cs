using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PostGameCanvas : MonoBehaviour
{
    public static PostGameCanvas instance;
    [SerializeField]
    private TextMeshProUGUI totalScoreText, playerStatsText, LBnames, LBranks, LBscore;
    public int playerShotsFired, playerHeadshotCount, playerBodyshotCount, playerShotsMissed, playerGameTime, playerScore;
    public float playerAccuracy;
    [SerializeField]
    public GameObject playerStats, leaderboard, optionsMenu;
    public int leaderboardMaxPages, currentLBpage = 1;
    public List<Score> scores;
    private string playerName;
    [SerializeField]
    private Animator LBanim;
    private bool postedScore;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        postedScore = false;
        openOptionsMenu();
    }

    void Start()
    {
        leaderboardMaxPages = 5;
        currentLBpage = 1;
        changeLBPages(currentLBpage);
        scores = new List<Score>();
        LBanim.SetBool("ShowScores", false);
        playerName = PlayerPrefs.GetString("playerName", "default");
    }

    public void setStats()
    {
        playerStatsText.text = "Shots Fired: " + playerShotsFired.ToString() + "\n" + 
        "Headshots: " + playerHeadshotCount.ToString()  + "\n" + 
        "Bodyshots: " + playerBodyshotCount.ToString() + "\n" + 
        "Shots missed: " + playerShotsMissed.ToString() + "\n" + 
        "Accuracy: " + Mathf.Round(playerAccuracy).ToString() + "%" + "\n" + 
        "Time: " + playerGameTime.ToString() + " seconds";

        totalScoreText.text = playerScore.ToString();
    }

    public void getPlayerStats()
    {
        playerStats.SetActive(true);
        leaderboard.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void openLeaderboard()
    {
        postScore();
        // getScore();
        playerStats.SetActive(false);
        leaderboard.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void restartTraining()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void goToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void openOptionsMenu()
    {
        playerStats.SetActive(false);
        leaderboard.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void nextLeaderboardPage()
    {
        if (currentLBpage >= leaderboardMaxPages)
        {
            currentLBpage = 1;
        } else {
            currentLBpage++;
        }
        changeLBPages(currentLBpage);
    }

    public void prevLeaderboardPage()
    {
        if (currentLBpage <= 1)
        {
            currentLBpage = leaderboardMaxPages;
        } else {
            currentLBpage--;
        }
        changeLBPages(currentLBpage);
    }

    void changeLBPages(int pageNumber)
    {
        LBnames.pageToDisplay = currentLBpage;
        LBranks.pageToDisplay = currentLBpage;
        LBscore.pageToDisplay = currentLBpage;
    }

    public void getScore()
    {
        scores = leaderboardHandler.instance.RetrieveScores();
    }

    public void updateLeaderboard(List<Score> updatedScores)
    {
        scores = updatedScores;
        int counter = 1;
        foreach (Score score in scores)
        {
            if (counter == 1)
            {
                LBranks.text = counter + ".\n";
                LBnames.text = score.name + "\n";
                LBscore.text = score.score + "\n";
            } else {
                LBranks.text += counter + ".\n";
                LBnames.text += score.name + "\n";
                LBscore.text += score.score + "\n";
            }
            counter++;
        }
        leaderboardMaxPages = Mathf.RoundToInt((float)scores.Capacity/10f);
        LBanim.SetBool("ShowScores", true);
    }

    public void postScore()
    {
        if (!postedScore && playerScore != 0)
        {
            leaderboardHandler.instance.PostScores(playerName, playerScore);
            getScore();
            postedScore = true;
        } else {
            getScore();
        }
    }

    void OnEnable()
    {
        GameManager.playerInput.PlayerMain.Disable();
        GameManager.gameEnd += postScore;
    }

    void OnDisable()
    {
        GameManager.playerInput.PlayerMain.Enable();
        GameManager.gameEnd -= postScore;
    }
}