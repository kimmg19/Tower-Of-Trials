using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameCanvas : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    bool isGamePaused = false;
    void Start()
    {
        pauseMenu.SetActive(false);
    }
    public void ClickPuaseButton()
    {
        if (!isGamePaused)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            isGamePaused = true;
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
    }
    public void ClickToMainButton()
    {
        AudioManager.instance.Stop("VillageBgm");
        Time.timeScale = 1;
        SceneManager.LoadScene("GameTitleScene");
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



