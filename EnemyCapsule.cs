using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyCapsule : MonoBehaviour
{
    public static Action<GameObject> lastDisabled;
    void OnDisable()
    {
        if (lastDisabled != null)
        {
            lastDisabled(gameObject);   
        }
        if (GameManager.spawnEnemy != null)
        {
            GameManager.spawnEnemy();
        }
    }
}
