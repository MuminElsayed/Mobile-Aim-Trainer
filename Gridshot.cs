using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gridshot : MonoBehaviour
{
    [SerializeField]
    private GameObject[] targets;
    private GameObject lastSpawn;

    void Start()
    {
        putRandomTarget();
        putRandomTarget();
        putRandomTarget();
    }

    void putRandomTarget()
    {
        //Gets unique target then call spawn
        int randomSpawn = UnityEngine.Random.Range(0, targets.Length);
        if (!targets[randomSpawn].activeInHierarchy && targets[randomSpawn] != lastSpawn) //Inactive
        {
            StartCoroutine(activateObj(targets[randomSpawn]));         
        } else {
            putRandomTarget();
        }
    }
    IEnumerator activateObj(GameObject obj)
    {
        yield return new WaitForEndOfFrame();
        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true);
        } else {
            putRandomTarget();
        }
    }

    void getLastDisabled(GameObject obj)
    {
        lastSpawn = obj;
    }

    void OnEnable()
    {
        GameManager.spawnEnemy += putRandomTarget;
        EnemyCapsule.lastDisabled += getLastDisabled;
    }

    void OnDisable()
    {
        GameManager.spawnEnemy -= putRandomTarget;
        EnemyCapsule.lastDisabled -= getLastDisabled;
    }
}
