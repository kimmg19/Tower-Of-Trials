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
    void Start()
    {
        portal = GetComponent<Portal>();
        playerUI = GameObject.Find("PlayerUI");
        pauseMenu.SetActive(false);
        optionMenu.SetActive(false);
        dieImage.SetActive(false);
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
}



