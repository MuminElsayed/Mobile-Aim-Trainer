using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float timeOut;
    [SerializeField]
    private bool hideMeshOnly;
    private MeshRenderer meshRenderer;
    
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        try  {
            GetComponentInChildren<shrinkingZone>().shrinkTime = timeOut;
        } catch {}
    }
    void setLifetime(float time)
    {
        timeOut = time;
    }

    void OnEnable()
    {
        if (hideMeshOnly)
        {
            meshRenderer.enabled = true;
            Invoke("hide", timeOut);
        } else {
            Invoke("disable", timeOut);
        }
    }

    void hide()
    {
        meshRenderer.enabled = false;
    }

    void disable()
    {
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (GameManager.spawnEnemy != null)
        {
            GameManager.spawnEnemy();
        }
        CancelInvoke();
        StopAllCoroutines();
    }
}
