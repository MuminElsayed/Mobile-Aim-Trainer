using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnClick : MonoBehaviour
{    
    public void Disable(GameObject obj)
    {
        obj.SetActive(false);
    }
}