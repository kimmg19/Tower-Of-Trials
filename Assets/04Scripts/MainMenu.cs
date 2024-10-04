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
    [SerializeField] GameObject HelpImage;
    [SerializeField] Slider bgmSlider;

    [SerializeField] GameObject HelpImage1;
    [SerializeField] GameObject HelpImage2;
    [SerializeField] GameObject HelpImage3;

    void Start()
    {
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        GetVolume();

        HelpImage1.SetActive(false);
        HelpImage2.SetActive(false);
        HelpImage3.SetActive(false);
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
        }
        else
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

    public void OnClickHelp()
    {
        if (!HelpImage1.activeSelf)
        {
            HelpImage1.SetActive(true);
        }
    }

    public void OnClickHelpClose1()
    {
        if (HelpImage1.activeSelf)
        {
            HelpImage1.SetActive(false);
        }
    }

    public void OnClickHelpClose2()
    {
        if (HelpImage2.activeSelf)
        {
            HelpImage2.SetActive(false);
        }
    }

    public void OnClickHelpClose3()
    {
        if (HelpImage3.activeSelf)
        {
            HelpImage3.SetActive(false);
        }
    }

    public void OnClickHelpNext()
    {
        if (HelpImage1.activeSelf)
        {
            HelpImage1.SetActive(false);
            HelpImage2.SetActive(true);
        }
    }

    public void OnClickHelpNext2()
    {
        if (HelpImage2.activeSelf)
        {
            HelpImage2.SetActive(false);
            HelpImage3.SetActive(true);
        }
    }

    public void OnClickHelpPrev()
    {
        if (HelpImage2.activeSelf)
        {
            HelpImage2.SetActive(false);
            HelpImage1.SetActive(true);
        }
    }

    public void OnClickHelpPrev2()
    {
        if (HelpImage3.activeSelf)
        {
            HelpImage3.SetActive(false);
            HelpImage2.SetActive(true);
        }
    }
}