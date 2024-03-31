using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadSettings : MonoBehaviour
{
    public List<string> audioNames;
    public AudioMixer mainAudio;
    void Start()
    {
        //ResetAudioSettings();
        LoadAudioSettings();     
    }

    void LoadAudioSettings()
    {
        foreach (string name in audioNames)
        {
            SetAudioVolume(name);
        }
    }

    void SetAudioVolume(string audioName)
    {
        float volume = -80;
        if (!PlayerPrefs.HasKey(audioName))
        {
            volume = Mathf.Log10(0.35f * 20);           
            PlayerPrefs.SetFloat(audioName, 0.35f);
        }
        else
        {
            volume = Mathf.Log10(PlayerPrefs.GetFloat(audioName)) * 20;
        }
        mainAudio.SetFloat(audioName, volume);
    }

    void ResetAudioSettings()
    {
        foreach (string name in audioNames)
        {
            PlayerPrefs.DeleteKey(name);
        }
    }
}
