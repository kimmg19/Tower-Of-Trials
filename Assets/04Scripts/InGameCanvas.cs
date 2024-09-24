using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameCanvas : MonoBehaviour
{
    public GameObject dieImage;
    public GameObject pauseMenu;
    public GameObject pauseButton;
    bool isGamePaused = false;
    GameObject playerUI;
    public GameObject optionMenu;
    Portal portal;
    [SerializeField] GameObject HelpImage1;
    [SerializeField] GameObject HelpImage2;
    [SerializeField] GameObject HelpImage3;
    void Start()
    {
        portal = GetComponent<Portal>();
        playerUI = GameObject.Find("PlayerUI");
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
        dieImage.SetActive(false);
        HelpImage1.SetActive(false);
        HelpImage2.SetActive(false);
        HelpImage3.SetActive(false);
    }
    public void ClickPauseButton()
    {
        if (!isGamePaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            isGamePaused = true;
            playerUI.SetActive(false);
        } else
        {
            ClickResumeButton();
        }

    }
    public void ClickResumeButton()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
        playerUI.SetActive(true);
        optionMenu.SetActive(false);
    }
    public void ClickOptionButton()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
    public void ClickToMainButton()
    {
        
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void ClickQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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