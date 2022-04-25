using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shrinkingZone : MonoBehaviour
{
    [SerializeField]
    private Color green, orange, red;
    private Image image;
    private Vector2 originalSize;
    public float shrinkTime;
    void Awake()
    {
        image = GetComponentInChildren<Image>();
        originalSize = image.rectTransform.sizeDelta;
    }

    void reset()
    {
        image.rectTransform.sizeDelta = originalSize;
        StartCoroutine(shrinkOverTime(shrinkTime));
    }

    void setShrinkTime(float time)
    {
        shrinkTime = time;
    }

    void OnEnable()
    {
        reset();
    }

    IEnumerator shrinkOverTime(float totalDuration)
    {
        float startTime = Time.time;
        float percentDone = 0;
        while (Time.time < startTime + totalDuration)
        {
            percentDone = (Time.time - startTime)/totalDuration;
            image.rectTransform.sizeDelta = originalSize * (1 - percentDone);
            if (percentDone > 0.33f)
            {
                image.color = orange;
                if (percentDone > 0.66f)
                {
                    image.color = red;
                }
            } else {
                image.color = green;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
