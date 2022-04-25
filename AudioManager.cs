using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    private AudioSource[] audioSources;
    private float[] defaultVolumes;
    public Color chColor;
    public Sprite chSprite;
    public static Action<Color, Sprite> setCrosshair;
    private int themeNum = 0;
    private Color themeColor = Color.white;
    private Texture themeTex;
    public static Action <int, Color, Texture> setTheme;

    void Awake()
    {
        if (audioManager == null)
        {
            audioManager = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        audioSources = GetComponents<AudioSource>();
        defaultVolumes = new float[3];
        int counter = 0;
        foreach (var audioSrc in audioSources)
        {
            defaultVolumes[counter] = audioSrc.volume;
            counter++;
        }
        Application.targetFrameRate = 60;
    }

    public void playClip(AudioClip clip, int source)
    {
        audioSources[source].clip = clip;
        audioSources[source].Play();
    }

    public void setMusicVol(int volume)
    {
        if (volume > 0)
        {
            if (audioSources[0].volume == 0)
            {
                audioSources[0].Play();
            }
            audioSources[0].volume = defaultVolumes[0];
        } else {
            audioSources[0].volume = 0;
        }
    }

    public void setEffectsVol(int volume)
    {
        if (volume > 0)
        {
            audioSources[1].volume = defaultVolumes[1];
            audioSources[2].volume = defaultVolumes[2];
            audioSources[1].Play();
        } else {
            audioSources[1].volume = 0;
            audioSources[2].volume = 0;
        }
    }

    void saveCH(Color color, Sprite sprite)
    {
        chColor = color;
        chSprite = sprite;
    }

    public void sendcrosshair()
    {
        if (setCrosshair != null)
        {
            setCrosshair(chColor, chSprite);
        }
    }

    void saveThemeSettings(int num, Color color, Texture texture)
    {
        themeNum = num;
        themeColor = color;
        themeTex = texture;
    }

    public void getThemeSettings()
    {
        if (setTheme != null)
        {
            setTheme(themeNum, themeColor, themeTex);
        }
    }

    void OnEnable()
    {
        settingsUI.setCrosshair += saveCH;
        WeaponChangerUI.saveTheme += saveThemeSettings;
    }

    void OnDisable()
    {
        settingsUI.setCrosshair -= saveCH;
        WeaponChangerUI.saveTheme -= saveThemeSettings;
    }
}