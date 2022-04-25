using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class layoutChanger : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    public Vector3 changedRectPos, defaultPos;
    private bool changingLayout;
    public float scale = 1;
    public static Action<RectTransform> gettingDragged;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    public void OnPointerDown(PointerEventData pointer)
    {
        //send action
        if (gettingDragged != null)
        {
            gettingDragged(rectTransform);
        }
    }

    public void OnEndDrag(PointerEventData pointer)
    {
        if (changingLayout)
        {
            changedRectPos = rectTransform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (changingLayout)
        {
            rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
        }
    }

    public void resetToDefault()
    {
        rectTransform.position = defaultPos;
        rectTransform.localScale = Vector3.one;
        scale = 1;
        changedRectPos = Vector3.zero;
        saveLayout();
        changingLayout = true;
    }

    public void saveLayout()
    {
        changingLayout = false;
        scale = rectTransform.localScale.x;
        //Save player data
        string jsonData = JsonUtility.ToJson(this, false);
        PlayerPrefs.SetString(gameObject.name, jsonData);
        PlayerPrefs.Save();
    }

    public void changeLayout()
    {
        changingLayout = true;
    }

    void OnEnable()
    {
        //Load player data
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(gameObject.name), this);

        if (changedRectPos != Vector3.zero) //Changed default value
        {
            rectTransform.position = changedRectPos;
        } else {
            //Saves default pos
            defaultPos = rectTransform.position;
        }
        rectTransform.localScale = new Vector3(scale, scale, 1);

        layoutChangeCanvas.resetLayoutAction += resetToDefault;
        layoutChangeCanvas.saveLayoutAction += saveLayout;
        settingsUI.changeLayoutAction += changeLayout;
    }

    void OnDisable()
    {
        layoutChangeCanvas.resetLayoutAction -= resetToDefault;
        layoutChangeCanvas.saveLayoutAction -= saveLayout;
        settingsUI.changeLayoutAction -= changeLayout;
    }
}
