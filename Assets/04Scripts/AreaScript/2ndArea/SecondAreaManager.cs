using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Playables;

public class SecondAreaManager : MonoBehaviour
{
    [Header("Cinematic Settings")]
    [SerializeField] PlayableDirector cinematicDirector; // 시네마틱 재생을 위한 PlayableDirector
    [SerializeField] float cinematicSpeed = 1.3f; // 시네마틱 속도 (1.0은 기본 속도)

    [Header("Area Settings")]
    [SerializeField] GameObject titlePanel;  // 타이틀 패널
    private CanvasGroup titleCanvasGroup; // 페이드 인,아웃을 위한 타이틀 캔버스 그룹


    void Start()
    {
        PlayEnterCinematic(); // 게임 시작 시 시네마틱 재생
        titleCanvasGroup = titlePanel.GetComponent<CanvasGroup>();

        // 타이틀 패널을 처음에만 보이게 하고, 그 후에는 숨김
        if (titleCanvasGroup != null)
        {
            StartCoroutine(ShowTitlePanel());
        }
    }

    void PlayEnterCinematic()
    {
        if (cinematicDirector != null)
        {
            cinematicDirector.playableGraph.GetRootPlayable(0).SetSpeed(cinematicSpeed); // 시네마틱 속도 설정
            cinematicDirector.Play(); // 시네마틱 재생
        }
    }

    private IEnumerator ShowTitlePanel() // 타이틀 페이드 인,아웃
    {
        titlePanel.SetActive(true);
        if (titleCanvasGroup != null)
        {
            titleCanvasGroup.alpha = 0f; // 초기에는 완전히 투명
            while (titleCanvasGroup.alpha < 1f)
            {
                titleCanvasGroup.alpha += Time.deltaTime * 1; // 페이드 인 속도 조절
                yield return null;
            }
            titleCanvasGroup.alpha = 1f; // 마지막에는 완전히 불투명

            // 타이틀이 일정 시간 후에 사라지도록
            yield return new WaitForSeconds(7f);

            while (titleCanvasGroup.alpha > 0f)
            {
                titleCanvasGroup.alpha -= Time.deltaTime * 1; // 페이드 아웃 속도 조절
                yield return null;
            }
            titleCanvasGroup.alpha = 0f; // 마지막에는 완전히 투명
            titlePanel.SetActive(false); // 타이틀 비활성화
        }
    }
}
