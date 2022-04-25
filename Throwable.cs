using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Throwable : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField]
    private float speed;
    private int enemyLayer;
    public static Action addShot;
    private int difficulty;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    void Start()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 0);
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == enemyLayer)
        {
            collision.gameObject.SetActive(false);
            WeaponController.addScore(250 * (difficulty + 1));
            WeaponController.addAccuracy(true);
        } else {
            WeaponController.addAccuracy(false);
            WeaponController.addScore(-50 * (difficulty + 1));
        }
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        transform.forward = Vector3.zero;
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
    }
}