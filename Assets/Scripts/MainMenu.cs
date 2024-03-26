using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject gameStartSelection;
    // Start is called before the first frame update
    void Start()
    {
        gameStartSelection.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickNewGame()
    {
        Debug.Log("New Game");
        gameStartSelection.SetActive(true);
    }

    public void OnClickLoad()
    {
        Debug.Log("Loading Game...");
    }

    public void OnClickOption()
    {
        Debug.Log("Option");
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickChangeScene()
    {
        SceneManager.LoadScene("InGameScene");
    }

    public void OnClickClose()
    {
        gameStartSelection.SetActive(false);
    }
}
