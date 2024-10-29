using UnityEngine;
using TMPro;
using System.Collections;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textComponent; // TextMeshPro 컴포넌트 참조
    public float fadeDuration = 1f; // 페이드 지속 시간
    public float waitDuration = 3f; // 다음 줄 전 대기 시간
    public string[] textLines; // 여러 줄의 텍스트를 저장할 배열
    private CanvasGroup canvasGroup; // CanvasGroup 컴포넌트 참조

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>(); // CanvasGroup 컴포넌트 가져오기
        if (textLines.Length > 0)
        {
            StartCoroutine(TypeText()); // 텍스트 애니메이션 시작
        }
    }

    IEnumerator TypeText()
    {
        foreach (string line in textLines)
        {
            textComponent.text = line; // 현재 줄 텍스트 설정
            canvasGroup.alpha = 0; // 초기 알파 값을 0으로 설정

            // 페이드인 애니메이션
            float elapsedTime = 0;
            while (elapsedTime < fadeDuration)
            {
                canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration); // 알파 값 조정
                elapsedTime += Time.deltaTime;
                yield return null; // 다음 프레임 대기
            }
            canvasGroup.alpha = 1; // 알파 값을 1로 설정

            yield return new WaitForSeconds(waitDuration); // 다음 줄 전 대기 시간
        }
    }
}
