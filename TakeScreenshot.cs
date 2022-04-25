using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TakeScreenshot : MonoBehaviour
{

    void Update()
    {
        GameManager.playerInput.UI.Screenshot.performed += _ => takeScreenshot();
    }

    void takeScreenshot()
    {
        ScreenCapture.CaptureScreenshot(Application.dataPath + "/screenshots/" + SceneManager.GetActiveScene().name + Time.time + ".png");
        print ("Screenshot saved at: " + Application.dataPath + "/screenshots/" + SceneManager.GetActiveScene().name + Time.time + ".png");
    }
}