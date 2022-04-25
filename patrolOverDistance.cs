using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrolOverDistance : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;
    [SerializeField]
    private float speed, distance;
    private Vector3 startPos;

    void Awake()
    {
        startPos = transform.position;
    }
    void Update()
    {
        transform.position = startPos + direction * Mathf.Sin(Time.time * speed) * distance;
    }
}
