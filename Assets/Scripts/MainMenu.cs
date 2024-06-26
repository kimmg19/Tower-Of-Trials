using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameObject optionMenu;
    [SerializeField] Slider bgmSlider;

    void Start()
    {
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        GetVolume();
    }
    void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);

    }
    public void OnClickGameStart()
    {
        
        LoadingSceneManager.LoadScene(2);
    }

    public void OnClickLoad()
    {
        Debug.Log("Loading Game...");
    }

    public void OnClickOption()
    {
        if (optionMenu.activeSelf)
        {
            optionMenu.SetActive(false);
        } else
        {
            optionMenu.SetActive(true);
        }
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    void GetVolume()
    {
        float bgmVolume;
        audioMixer.GetFloat("BGM", out bgmVolume);   
        bgmSlider.value = Mathf.Pow(10, bgmVolume / 20);
    }


    void OnPause()
    {
        OnClickOption();
    }
}


