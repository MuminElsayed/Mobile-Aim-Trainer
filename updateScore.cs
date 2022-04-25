using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class updateScore : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void score(int num)
    {
        text.text = "Score: " + num.ToString();
    }

    void OnEnable()
    {
        WeaponController.updateScoreAction += score;
    }

    void OnDisable()
    {
        WeaponController.updateScoreAction -= score;
    }
}
