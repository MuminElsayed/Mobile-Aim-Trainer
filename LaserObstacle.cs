using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObstacle : MonoBehaviour
{
    private Vector3 startPos;
    public float moveSpeed;
    private Rigidbody rb;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + Vector3.forward * -0.25f * moveSpeed);
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("LaserReset"))
        {
            transform.position = startPos;
        } else if (collider.gameObject.CompareTag("PlayerTrigger"))
        {
            WeaponController.addScore(-200);
            // print(collider.gameObject.name);
        }
    }
}
