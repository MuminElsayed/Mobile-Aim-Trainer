using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class layoutOptions : MonoBehaviour
{
    private RectTransform rect, targetRect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void setParent(RectTransform parent)
    {
        targetRect = parent;
        transform.SetParent(parent);
        rect.anchoredPosition = Vector3.zero;
        rect.localScale = 1/targetRect.localScale.x * 2f * Vector3.one;
    }

    public void HideTarget()
    {
        transform.SetParent(transform.root);
        rect.localScale = Vector3.zero;
        targetRect.gameObject.SetActive(false);
    }

    public void SizeIncrease()
    {
        if (targetRect.localScale.x < 2)
        {
            targetRect.localScale += Vector3.one * 0.1f;
        }
        rect.localScale = 1/targetRect.localScale.x * 2f * Vector3.one;
    }

    public void SizeDecrease()
    {
        if (targetRect.localScale.x > 0.5f)
        {
            targetRect.localScale -= Vector3.one * 0.1f;
        }
        rect.localScale = 1/targetRect.localScale.x * 2f * Vector3.one;
    }
    
    void OnEnable()
    {
        layoutChanger.gettingDragged += setParent;
    }

    void OnDisable()
    {
        layoutChanger.gettingDragged -= setParent;
    }
}
