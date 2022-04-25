using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{
    private RectTransform rect;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void setParent(GameObject gameObject)
    {
        try
        {
            transform.SetParent(gameObject.transform);
            rect.anchoredPosition = Vector3.zero;
        } catch {
            Debug.Log("couldn't set parent");
        }
    }
}
