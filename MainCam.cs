using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [SerializeField]
    private bool canTilt;
    [SerializeField]
    private float tiltOffset;
    private GameObject player;
    private Collider playerCollider;
    private Vector3 offset, originalOffset, playerOriginalOffset;
    private bool tiltingLeft, tiltingRight;
    [SerializeField]
    private float tiltSpeed = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (canTilt)
        {
            playerCollider = GameObject.FindGameObjectWithTag("PlayerTrigger").GetComponent<Collider>();
            playerOriginalOffset = playerCollider.transform.position;
        }
        offset = transform.localPosition;
        originalOffset = offset;
    }
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }

    public void tiltLeft()
    {
        if (!tiltingLeft)
        {
            StopAllCoroutines();
            StartCoroutine(lerpOffset(originalOffset - Vector3.right * tiltOffset, tiltSpeed));
            // offset = originalOffset - Vector3.right * 0.5f;
            // offset = Vector3.Lerp(offset, originalOffset - Vector3.right * 0.5f, Time.deltaTime * tiltSpeed);
            tiltingLeft = true;
            tiltingRight = false;
        }
    }
    public void tiltRight()
    {
        if (!tiltingRight)
        {
            StopAllCoroutines();
            StartCoroutine(lerpOffset(originalOffset + Vector3.right * tiltOffset, tiltSpeed));
            // offset = originalOffset + Vector3.right * 0.5f;
            // offset = Vector3.Lerp(offset, originalOffset + Vector3.right * 0.5f, Time.deltaTime * tiltSpeed);
            tiltingRight = true;
            tiltingLeft = false;
        }
    }

    public void noTilt()
    {
        // offset = originalOffset;
        // offset = Vector3.Lerp(offset, originalOffset, Time.deltaTime * tiltSpeed);
        StopAllCoroutines();
        StartCoroutine(lerpOffset(originalOffset, tiltSpeed));
        tiltingRight = false;
        tiltingLeft = false;
    }

    IEnumerator lerpOffset(Vector3 endPos, float speed)
    {
        while(Mathf.Abs(offset.x - endPos.x) > 0.01f)
        {
            offset = Vector3.Lerp(offset, endPos, Time.deltaTime * speed);
            // playerCollider.transform.position = Vector3.Lerp(playerCollider.transform.position, endPos - originalOffset, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
    }

    void OnEnable()
    {
        if (canTilt)
        {
            GameManager.playerInput.PlayerMain.TiltRight.performed += _ => tiltRight();
            GameManager.playerInput.PlayerMain.TiltLeft.performed += _ => tiltLeft();
            GameManager.playerInput.PlayerMain.TiltRight.canceled += _ => noTilt();
            GameManager.playerInput.PlayerMain.TiltLeft.canceled += _ => noTilt();
        }
    }
}
