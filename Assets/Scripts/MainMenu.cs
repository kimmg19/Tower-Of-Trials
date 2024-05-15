using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
