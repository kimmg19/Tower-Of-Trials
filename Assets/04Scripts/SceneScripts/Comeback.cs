using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Comeback : MonoBehaviour
{
    public void OnClickComeback()
    {
        LoadingSceneManager.LoadScene(2);
    }

}
