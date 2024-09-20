using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHitEffect : MonoBehaviour
{
    public Image hitEffectImage; // UI Image를 참조
    public float fadeDuration = 0.5f; // 페이드 지속 시간

    private void Start()
    {
        if (hitEffectImage != null)
        {
            hitEffectImage.enabled = false; // 초기에는 이미지 비활성화
        }
    }

    // 플레이어가 피격당할 때 이 메서드를 호출
    public void ShowHitEffect()
    {
        if (hitEffectImage != null)
        {
            StartCoroutine(FadeInAndOut());
        }
    }

    private IEnumerator FadeInAndOut()
    {
        hitEffectImage.enabled = true; // 이미지 활성화
        CanvasGroup canvasGroup = hitEffectImage.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            // 이미지에 CanvasGroup이 없으면 추가
            canvasGroup = hitEffectImage.gameObject.AddComponent<CanvasGroup>();
        }

        float elapsedTime = 0f;

        // 알파값을 1로 올리며 페이드 인
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // 짧은 시간 대기

        elapsedTime = 0f;

        // 알파값을 0으로 낮추며 페이드 아웃
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));
            yield return null;
        }

        canvasGroup.alpha = 0; // 마지막에는 완전히 투명하게 설정
        hitEffectImage.enabled = false; // 이미지 비활성화
    }
}
