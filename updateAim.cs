using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class updateAim : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void aim(float aim)
    {
        text.text = Mathf.Round(aim).ToString() + "%";
    }

    void OnEnable()
    {
        WeaponController.updateAimAction += aim;
    }

    void OnDisable()
    {
        WeaponController.updateAimAction -= aim;
    }
}
