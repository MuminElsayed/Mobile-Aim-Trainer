using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BotTraining : MonoBehaviour
{
    [SerializeField]
    private int minSpawnRange = 10, maxSpawnRange = 20;
    [SerializeField]
    private float spawnWidth = 3, spawnRate = 1f, spawnLifetime = 2f;
    private Vector3 lastSpawnPos, spawnPos;
    [SerializeField]
    private GameObject botPrefab;
    private List<GameObject> bots;
    private int maxBots, difficulty;
    

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        setDifficulty();
        bots = new List<GameObject>();
        maxBots = Mathf.RoundToInt(spawnLifetime/spawnRate) + 1;
        // spawnPos = Vector3.forward * minSpawnRange; //1st bot spawn is always at minrange
        botPrefab.GetComponent<DisableAfterTime>().timeOut = spawnLifetime;
        //Pools 3 bots (spawnLifetime/spawnRate = amount of bots active consequently)
        for (int i = 0; i < maxBots; i++)
        {
            bots.Add(Instantiate(botPrefab, Vector3.zero, botPrefab.transform.rotation)); //Adds new bots to list
            // bots[i].SetActive(false);
        }
        putRandomBot();
        Invoke("putRandomBot", spawnRate);
        Invoke("putRandomBot", spawnRate * 2f);
    }

    void putRandomBot()
    {
        while(spawnPos == lastSpawnPos)
        {
            spawnPos = new Vector3(UnityEngine.Random.Range(-spawnWidth, spawnWidth + 1), -3, UnityEngine.Random.Range(minSpawnRange, maxSpawnRange)); //Gets random spawn pos
        }

        //get disabled bot
        GameObject spawnedBot = bots[0];
        foreach (GameObject bot in bots)
        {
            if (bot.activeSelf == false) //Gets an inactive bot
            {
                spawnedBot = bot;
                StartCoroutine(activateObj(spawnedBot, spawnPos));
                break; 
            }
        }
    }

    IEnumerator activateObj(GameObject obj, Vector3 position)
    {
        yield return new WaitForEndOfFrame();
        if (!obj.activeInHierarchy)
        {
            obj.transform.position = position;
            obj.SetActive(true);
            lastSpawnPos = position; //Saves last spawn pos
        } else {
            putRandomBot();
        }
    }

    void EndGame()
    {
        CancelInvoke();
    }

    void setDifficulty()
    {
        if (difficulty == 0)
        {
            spawnRate = 10f;
            spawnLifetime = 5f;
        } else if (difficulty == 1)
        {
            spawnRate = 5f;
            spawnLifetime = 3f;
        } else {
            spawnRate = 2.5f;
            spawnLifetime = 2f;
        }
    }

    void OnEnable()
    {
        GameManager.spawnEnemy += putRandomBot;
        GameManager.gameEnd += EndGame;
    }

    void OnDisable()
    {
        GameManager.spawnEnemy -= putRandomBot;
        GameManager.gameEnd -= EndGame;
    }
}
