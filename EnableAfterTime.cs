using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAfterTime : MonoBehaviour
{
    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private float time;
    void Start()
    {
        Invoke("enable", time);
    }

    void enable()
    {
        obj.SetActive(true);
    }
}
