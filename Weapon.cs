using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int maxAmmo, currentAmmo;
    public float reloadTime, fireRate;
    public AudioClip bulletAudio, reloadAudio;
    public bool automatic, throwable;
}
