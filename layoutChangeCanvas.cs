using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class layoutChangeCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject[] layoutChangeObjects;
    [SerializeField]
    private List<GameObject> activeObjects;
    public static Action resetLayoutAction, saveLayoutAction;
    [SerializeField]
    private GameObject layoutOptions;

    void layoutChangeView()
    {
        foreach (Transform obj in transform)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                activeObjects.Add(obj.gameObject);
            }
            obj.gameObject.SetActive(false);
        }
        foreach (GameObject obj in layoutChangeObjects)
        {
            obj.SetActive(true);
        }

        layoutOptions.SetActive(true);
    }

    void gameView()
    {
        foreach (Transform obj in transform)
        {
            obj.gameObject.SetActive(false);
        }

        foreach (GameObject obj in activeObjects)
        {
            obj.SetActive(true);
        }

        layoutOptions.SetActive(false);
    }

    public void resetLayoutPositions()
    {
        resetLayoutAction();
    }

    public void saveLayoutUI()
    {
        saveLayoutAction();
        gameView();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        settingsUI.changeLayoutAction += layoutChangeView;
    }

    void OnDisable()
    {
        settingsUI.changeLayoutAction -= layoutChangeView;
    }
}
