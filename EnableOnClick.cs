using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnClick : MonoBehaviour
{  
    public void Enable(GameObject obj)
    {
        obj.SetActive(true);
    }
}