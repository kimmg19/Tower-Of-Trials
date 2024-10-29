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
    private CanvasGroup titleCanvasGroup; // 타이틀 페이드 인,아웃을 위한 캔버스 그룹

    [Header("Clear Panel Settings")] 
    [SerializeField] GameObject clearPanel; // 클리어 패널
    private CanvasGroup clearCanvasGroup;   // 클리어 패널의 페이드 인,아웃을 위한 캔버스 그룹

    [Header("Reward Settings")]
    [SerializeField] GameObject rewardChest; // 보물상자 오브젝트

    [Header("Other Settings")]
    [SerializeField] PlayerInputs playerInputs;

    void Start()
    {
        PlayEnterCinematic(); // 게임 시작 시 시네마틱 재생
        titleCanvasGroup = titlePanel.GetComponent<CanvasGroup>();
        clearCanvasGroup = clearPanel.GetComponent<CanvasGroup>();

        // 타이틀 패널을 처음에만 보이게 하고, 그 후에는 숨김
        if (titleCanvasGroup != null)
        {
            StartCoroutine(ShowTitlePanel());
        }

        // 클리어 패널은 처음엔 비활성화 상태로 시작
        if (clearCanvasGroup != null)
        {
            clearPanel.SetActive(false); // 시작 시 클리어 패널을 비활성화
            clearCanvasGroup.alpha = 0f; // 알파값을 0으로 설정 (투명)
        }

        // 보물상자는 처음에 비활성화 상태로 시작
        if (rewardChest != null)
        {
            rewardChest.SetActive(false);
        }
    }

    void PlayEnterCinematic()
    {
        if (cinematicDirector != null)
        {
            playerInputs.enabled = false; // 시네마틱 시작 시 입력 비활성화
            // playerInputs.isInteracting = true;
            cinematicDirector.Play(); // 시네마틱 재생
            cinematicDirector.playableGraph.GetRootPlayable(0).SetSpeed(cinematicSpeed); // 시네마틱 속도 설정

            // 시네마틱 종료 시 입력 활성화
            cinematicDirector.stopped += OnCinematicEnded;
        }
    }

    void OnCinematicEnded(PlayableDirector director)
    {
        playerInputs.enabled = true; // 시네마틱 종료 시 입력 활성화
        // playerInputs.isInteracting = false;
        cinematicDirector.stopped -= OnCinematicEnded; // 이벤트 해제 (중복 방지)
    }

    public void StartClearPanelFade()
    {
        if (clearCanvasGroup != null)
        {
            AudioManager.instance.Play("VictoryBgm");
            rewardChest.SetActive(true);
            StartCoroutine(ShowClearPanel()); // 클리어 패널 페이드 인/아웃 시작
        }
    }

    private IEnumerator ShowTitlePanel() // 타이틀 페이드 인/아웃
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
            titleCanvasGroup.alpha = 1f; // 완전히 불투명

            // 타이틀이 일정 시간 후에 사라지도록
            yield return new WaitForSeconds(7f);

            while (titleCanvasGroup.alpha > 0f)
            {
                titleCanvasGroup.alpha -= Time.deltaTime * 1; // 페이드 아웃 속도 조절
                yield return null;
            }
            titleCanvasGroup.alpha = 0f; // 완전히 투명
            titlePanel.SetActive(false); // 타이틀 비활성화
        }
    }

    private IEnumerator ShowClearPanel() // 클리어 패널 페이드 인/아웃
    {
        clearPanel.SetActive(true);
        clearCanvasGroup.alpha = 0f; // 초기에는 투명

        // 페이드 인 (투명 -> 불투명)
        while (clearCanvasGroup.alpha < 1f)
        {
            clearCanvasGroup.alpha += Time.deltaTime * 1; // 페이드 인 속도 조절
            yield return null;
        }
        clearCanvasGroup.alpha = 1f; // 완전히 불투명

        // 일정 시간 대기
        yield return new WaitForSeconds(5f); // 클리어 패널이 5초 동안 유지됨

        // 페이드 아웃 (불투명 -> 투명)
        while (clearCanvasGroup.alpha > 0f)
        {
            clearCanvasGroup.alpha -= Time.deltaTime * 1; // 페이드 아웃 속도 조절
            yield return null;
        }
        clearCanvasGroup.alpha = 0f; // 완전히 투명
        clearPanel.SetActive(false); // 클리어 패널 비활성화
    }
}
