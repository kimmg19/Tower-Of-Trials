using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public static int nextSceneIndex; // ���� ���� �ε����� ����

    [SerializeField]
    Image ProgressBar;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(int sceneIndex)
    {
        nextSceneIndex = sceneIndex;
        SceneManager.LoadScene(1);
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneIndex); // 다음 씬을 로드
        op.allowSceneActivation = false;
        float fakeLoadTime = Random.Range(1f, 2f); // 1초에서 3초 사이의 랜덤한 로딩 시간 생성
        float timer = 0.0f;
        while (timer < fakeLoadTime)
        {
            timer += Time.deltaTime;
            ProgressBar.fillAmount = timer / fakeLoadTime; // 로딩 바가 로딩 진행에 따라 채워짐
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); // 로딩이 완료되도 0.5초 기다리게함
        op.allowSceneActivation = true; // 로딩이 완료되면 다음 씬으로 이동
    }
}
