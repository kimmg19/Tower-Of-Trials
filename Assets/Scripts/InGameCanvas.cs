using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameCanvas : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    bool isGamePaused = false;
    public GameObject playerUI;
    public GameObject optionMenu;
    void Start()
    {
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
    }
    public void ClickPuaseButton()
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
}



