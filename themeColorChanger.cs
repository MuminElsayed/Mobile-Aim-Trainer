using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class themeColorChanger : MonoBehaviour
{
    [SerializeField]
    private Color lightColor;
    private Renderer rend;
    private int theme;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void setThemeColor(int num, Color color, Texture targetTex)
    {
        Material tempRend = rend.material;
        if (num == 0)
        {
            color = lightColor;
            tempRend.SetColor("_EmissionColor", Color.black);
        } else {
            tempRend.SetColor("_EmissionColor", color);
        }
        //Set colors
        tempRend.SetColor("_BaseColor", color);
        tempRend.SetColor("_Color", color);

        //Set textures
        tempRend.SetTexture("_BaseMap", targetTex);
        tempRend.SetTexture("_EmissionMap", targetTex);

        //Set emission
        tempRend.EnableKeyword("_EMISSION");

        rend.material = tempRend;
    }

    void OnEnable()
    {
        WeaponChangerUI.changeTheme += setThemeColor;
        AudioManager.setTheme += setThemeColor;
        AudioManager.audioManager.getThemeSettings();
    }

    void OnDisable()
    {
        WeaponChangerUI.changeTheme -= setThemeColor;
        AudioManager.setTheme -= setThemeColor;
    }
}
