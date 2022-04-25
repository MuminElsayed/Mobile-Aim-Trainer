using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skin
{
    public Texture texture;
    public Color color = Color.white;
    public float emissionIntensity = 1f;
}

[System.Serializable]
public class weaponParts
{
    public Skin[] skins;
    public Renderer renderer;
}
public class WeaponSkinChanger : MonoBehaviour
{
    public int currentSkin;
    [SerializeField]
    private weaponParts[] weaponParts;

    void OnEnable()
    {
        currentSkin = PlayerPrefs.GetInt("WeaponSkin", 0);

        changeSkin(currentSkin);
        
        WeaponChangerUI.changeSkin += changeSkin;
    }

    public void changeSkin(int skinNum)
    {
        foreach (weaponParts part in weaponParts)
        {
            Material tempRend = part.renderer.material;
            //Set colors
            tempRend.SetColor("_BaseColor", part.skins[skinNum].color);
            tempRend.SetColor("_Color", part.skins[skinNum].color);

            // //Set textures
            tempRend.SetTexture("_BaseMap", part.skins[skinNum].texture);
            tempRend.SetTexture("_EmissionMap", part.skins[skinNum].texture);

            //Set emission
            tempRend.SetColor("_EmissionColor", part.skins[skinNum].color * part.skins[skinNum].emissionIntensity);

            part.renderer.material = tempRend;
        }
    }

    void OnDisable()
    {
        WeaponChangerUI.changeSkin -= changeSkin;
    }
}
