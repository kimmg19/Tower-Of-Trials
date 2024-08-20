using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;

    void Awake()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        GetVolume();

        void SetSFXVolume(float volume)
        {
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        }

        void SetBGMVolume(float volume)
        {
            audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);

        }

        void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);

        }
    }

    void GetVolume()
    {
        float masterVolume;
        float sfxVolume;
        float bgmVolume;
        audioMixer.GetFloat("Master", out masterVolume);
        audioMixer.GetFloat("SFX", out sfxVolume);
        audioMixer.GetFloat("BGM", out bgmVolume);

        masterSlider.value = Mathf.Pow(10, masterVolume / 20);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
        bgmSlider.value = Mathf.Pow(10, bgmVolume / 20);

    }
}
