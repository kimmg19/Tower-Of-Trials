using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.Play("Main");
    }


    public void OnClickGameStart()
    {
        Debug.Log("GameStart");
        Destroy(AudioManager.instance.gameObject);
        LoadingSceneManager.LoadScene("InGameScene");
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

}
