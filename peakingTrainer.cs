using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class peakingTrainer : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    private GameObject lastEnemy;
    [SerializeField]
    private GameObject lasers;
    private int difficulty;
    private float laserSpeed;
    void Start()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        setDifficulty();
        spawnEnemy();
        spawnEnemy();
        spawnEnemy();
    }

    void spawnEnemy()
    {
        if (enemies!=null)
        {
            int randomEnemy = UnityEngine.Random.Range(0, enemies.Length);
            if (!enemies[randomEnemy].activeInHierarchy && enemies[randomEnemy] != lastEnemy) //Same as last spawn
            {
                StartCoroutine(activateObj(enemies[randomEnemy]));
            } else {
                spawnEnemy();
            }
            //Spawns random unique enemy
        }
    }
    void setDifficulty()
    {
        if (difficulty == 0)
        {
            laserSpeed = 1f;
        } else if (difficulty == 1)
        {
            laserSpeed = 1.5f;
        } else {
            laserSpeed = 2.5f;
        }
        lasers.GetComponentInChildren<LaserObstacle>().moveSpeed = laserSpeed;
    }
    
    IEnumerator activateObj(GameObject obj)
    {
        yield return new WaitForEndOfFrame();
        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true);
            // StopAllCoroutines();
        } else {
            spawnEnemy();
        }
    }
    void getLastDisabled(GameObject obj)
    {
        lastEnemy = obj;
    }

    void OnEnable()
    {
        GameManager.spawnEnemy += spawnEnemy;
        EnemyCapsule.lastDisabled += getLastDisabled;
    }

    void OnDisable()
    {
        GameManager.spawnEnemy -= spawnEnemy;
        EnemyCapsule.lastDisabled -= getLastDisabled;
    }
}
