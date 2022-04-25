using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class updateTime : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void time(int num)
    {
        text.text = "Time: " + num.ToString();
    }

    void OnEnable()
    {
        GameManager.updateTimeAction += time;
    }

    void OnDisable()
    {
        GameManager.updateTimeAction -= time;
    }
}
