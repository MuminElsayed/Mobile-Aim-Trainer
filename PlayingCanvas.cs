using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCanvas : MonoBehaviour
{
    [SerializeField]
    private GameObject headshotText;


    void headShot()
    {
        headshotText.SetActive(true);
    }
    
    void OnEnable()
    {
        WeaponController.headshot += headShot;
    }

    void OnDisable()
    {
        WeaponController.headshot -= headShot;
    }
}
