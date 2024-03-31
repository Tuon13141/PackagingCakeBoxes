using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    public Slider audioSlider;
    public AudioMixer mainAudio;

    public string audioName;

    private void Start()
    {
        audioSlider.value = PlayerPrefs.GetFloat(audioName);
        ChangeAudioVolume();
    }
    public void ChangeAudioVolume()
    {
        float volume = 0;
        if (audioSlider.value == 0)
        {
            volume = -80;
        }
        else
        {
            volume = Mathf.Log10(audioSlider.value) * 20;
            
        }
        mainAudio.SetFloat(audioName, volume);
        PlayerPrefs.SetFloat(audioName, audioSlider.value);
    }
}
