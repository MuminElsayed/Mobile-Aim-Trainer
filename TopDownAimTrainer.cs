using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TopDownAimTrainer : MonoBehaviour
{
    private LayerMask noSpawnLayerMask;
    [SerializeField]
    private Transform minSpawnRange, maxSpawnRange;
    [SerializeField]
    private GameObject botPrefab;
    [SerializeField]
    private float spawnLifetime, spawnRate;
    private List<GameObject> botsList;
    private Vector3 spawnOffset;
    private int difficulty;

    void Start()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        setDifficulty();
        noSpawnLayerMask = LayerMask.GetMask("NoSpawn", "Enemy");
        int maxBots = Mathf.RoundToInt(spawnLifetime/spawnRate) + 2;
        botsList = new List<GameObject>();
        botPrefab.GetComponent<DisableAfterTime>().timeOut = spawnLifetime;
        spawnOffset = botPrefab.transform.position;

        for (int i = 0; i < maxBots; i++) //Create bots
        {
            botsList.Add(Instantiate(botPrefab, Vector3.zero, Quaternion.identity));
            // botsList[i].SetActive(false);
        }

        // InvokeRepeating("spawnBot", 1f, spawnRate);
        Invoke("spawnBot", 1);
    }

    void spawnBot()
    {
        Vector3 randomPos = new Vector3(UnityEngine.Random.Range(minSpawnRange.position.x, maxSpawnRange.position.x), 0, UnityEngine.Random.Range(minSpawnRange.position.z, maxSpawnRange.position.z)) + spawnOffset;

        while (Physics.Raycast(randomPos + Vector3.up, Vector3.down, 5f, noSpawnLayerMask)) //Hit a no spawn area or another enemy
        {
            //Get new randomPos
            randomPos = new Vector3(UnityEngine.Random.Range(minSpawnRange.position.x, maxSpawnRange.position.x), 0, UnityEngine.Random.Range(minSpawnRange.position.z, maxSpawnRange.position.z)) + spawnOffset;
        }

        GameObject spawnedBot = null;
        foreach (GameObject bot in botsList)
        {
            if (bot.activeSelf == false) //Gets an inactive bot
            {
                spawnedBot = bot;
                break; 
            }
        }

        spawnedBot.transform.position = randomPos;
        StartCoroutine(activateBot(spawnedBot));
    }

    IEnumerator activateBot(GameObject bot)
    {
        yield return new WaitForEndOfFrame();
        bot.SetActive(true);
    }

    void setDifficulty()
    {
        if (difficulty == 0)
        {
            spawnRate = 10f;
            spawnLifetime = 10f;
        } else if (difficulty == 1)
        {
            spawnRate = 5f;
            spawnLifetime = 5f;
        } else {
            spawnRate = 3f;
            spawnLifetime = 3f;
        }
    }

    void endGame()
    {
        CancelInvoke();
        Time.timeScale = 0;
    }

    void OnEnable()
    {
        GameManager.spawnEnemy += spawnBot;
        GameManager.gameEnd += endGame;
    }

    void OnDisable()
    {
        GameManager.spawnEnemy -= spawnBot;
        GameManager.gameEnd -= endGame;
    }
}
