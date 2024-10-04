using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static int nextSceneIndex;
    [SerializeField]
    private Image ProgressBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        SceneManager.LoadScene(1); 
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneIndex);
        op.allowSceneActivation = false;

        float fakeLoadTime = Random.Range(1f, 2f);
        float timer = 0.0f;

        while (timer < fakeLoadTime)
        {
            timer += Time.deltaTime;
            ProgressBar.fillAmount = timer / fakeLoadTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        op.allowSceneActivation = true;
    }
}
