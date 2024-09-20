using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Playables;

public class FirstAreaManager : MonoBehaviour
{
    // 타임라인 에셋을 재생할 PlayableDirector
    [SerializeField] PlayableDirector bossCinematic; 
    [SerializeField] PlayableDirector firstAreaCinematic; 

    [SerializeField] GameObject slimeGroup;
    [SerializeField] GameObject turtleGroup;
    [SerializeField] GameObject golemDoor;
    [SerializeField] GameObject rewardChest; // 보물상자 오브젝트
    [SerializeField] GameObject titlePanel;  // 타이틀 패널
    [SerializeField] GameObject clearPanel; // 클리어 패널
    [SerializeField] GameObject countPanel; // 카운트 패널
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] GameObject bossHealthBarCanvas; // 보스 피통 캔버스

    [SerializeField] PlayerMovement playerMovement;
    private CanvasGroup panelCanvasGroup;
    private CanvasGroup titleCanvasGroup;
    private CanvasGroup clearPanelCanvasGroup;
    private CanvasGroup bossHealthBarCanvasGroup;

    private int slimesToKill = 5;
    private int turtlesToKill = 5;
    private int slimesKilled = 0;
    private int turtlesKilled = 0;

    private bool slimesSpawned = false;
    private bool turtlesSpawned = false;
    public float health = 100f; // 보스의 체력
    float slowMotionInitialScale = 0.5f; // 초기 슬로우 모션 배율
    float slowMotionMidScale = 0.5f; // 중간 슬로우 모션 배율
    float slowMotionFinalScale = 1f; // 최종 슬로우 모션 배율 (원래 속도)
    float slowMotionDuration = 0.1f; // 슬로우 모션 지속 시간
    float transitionDuration = 1f; // 슬로우 모션의 느려지는 전환 시간

    void Start()
    {
        StartCoroutine("Play1stAreaCinematic");
        panelCanvasGroup = countPanel.GetComponent<CanvasGroup>();
        titleCanvasGroup = titlePanel.GetComponent<CanvasGroup>();
        clearPanelCanvasGroup = clearPanel.GetComponent<CanvasGroup>();
        bossHealthBarCanvasGroup = bossHealthBarCanvas.GetComponent<CanvasGroup>();


        // if (clearPanelCanvasGroup == null)
        // {
        //     Debug.LogError("CanvasGroup component missing on the ClearPanel GameObject.");
        // }
        // if (panelCanvasGroup == null)
        // {
        //     Debug.LogError("CanvasGroup component missing on the CountPanel GameObject.");
        // }

        // if (titleCanvasGroup == null)
        // {
        //     Debug.LogError("CanvasGroup component missing on the TitlePanel GameObject.");
        // }

        if (golemDoor != null)
        {
            golemDoor.SetActive(true); // 문은 처음에 열려있고
        }

        if (rewardChest != null)
        {
            rewardChest.SetActive(false); // 보물상자는 처음에 비활성화 상태
        }

        if (slimeGroup != null)
        {
            slimeGroup.SetActive(false);
        }

        if (turtleGroup != null)
        {
            turtleGroup.SetActive(false);
        }

        if (countPanel != null)
        {
            countPanel.SetActive(false);
        }
        // 클리어 패널을 처음에 비활성화
        if (clearPanel != null)
        {
            clearPanel.SetActive(false);
        }

        // 타이틀 패널을 처음에만 보이게 하고, 그 후에는 숨김
        if (titleCanvasGroup != null)
        {
            StartCoroutine(ShowTitlePanel());
        }

    }

    // 골렘이 죽었을 때 호출되는 메서드
    public void OnGolemKilled()
    {
        StartCoroutine(SlowMotionEffect());

        if (rewardChest != null)
        {
            rewardChest.SetActive(true); // 보물상자 활성화
        }

    }

    public void OnSlimeKilled()
    {
        slimesKilled++;
        UpdateKillText("슬라임", slimesKilled, slimesToKill);

        if (slimesKilled >= slimesToKill)
        {
            CheckAreaCompletion();
            if (panelCanvasGroup != null)
            {
                StartCoroutine(HidePanel());  // 패널을 숨기는 코루틴 시작
            }
        }
    }

    public void OnTurtleKilled()
    {
        turtlesKilled++;
        UpdateKillText("거북이", turtlesKilled, turtlesToKill);

        if (turtlesKilled >= turtlesToKill)
        {
            CheckAreaCompletion();
            if (panelCanvasGroup != null)
            {
                StartCoroutine(HidePanel());  // 패널을 숨기는 코루틴 시작
            }
        }
    }

    private void UpdateKillText(string monsterType, int killed, int toKill)
    {
        if (killText != null)
        {
            killText.text = $"{monsterType} 처치 {killed}/{toKill}";
        }
    }

    private void CheckAreaCompletion()
    {
        if (slimesKilled >= slimesToKill && turtlesKilled >= turtlesToKill)
        {
            HandleAreaCompletion();
        }
    }

    private void HandleAreaCompletion()
    {
        if (golemDoor != null)
        {
            golemDoor.SetActive(false); // 이때 열림
            StartCoroutine("PlayBossCinematic");
        }
    }

    public void SpawnSlimes()
    {
        if (!slimesSpawned && slimeGroup != null)
        {
            slimesSpawned = true;
            slimeGroup.SetActive(true);
            if (panelCanvasGroup != null)
            {
                StartCoroutine(ShowPanel("슬라임", slimesKilled, slimesToKill)); // 패널을 보여주는 코루틴 시작
            }
        }
    }

    public void SpawnTurtles()
    {
        if (!turtlesSpawned && turtleGroup != null)
        {
            turtlesSpawned = true;
            turtleGroup.SetActive(true);
            if (panelCanvasGroup != null)
            {
                StartCoroutine(ShowPanel("거북이", turtlesKilled, turtlesToKill)); // 패널을 보여주는 코루틴 시작
            }
        }
    }

    private IEnumerator ShowTitlePanel()
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
            yield return new WaitForSeconds(3f);

            while (titleCanvasGroup.alpha > 0f)
            {
                titleCanvasGroup.alpha -= Time.deltaTime * 1; // 페이드 아웃 속도 조절
                yield return null;
            }
            titleCanvasGroup.alpha = 0f; // 마지막에는 완전히 투명
            titlePanel.SetActive(false); // 타이틀 비활성화
        }
    }
    private IEnumerator ShowClearPanel()
    {
        clearPanel.SetActive(true);
        clearPanelCanvasGroup.alpha = 0f; // 초기에는 완전히 투명
        while (clearPanelCanvasGroup.alpha < 1f)
        {
            clearPanelCanvasGroup.alpha += Time.deltaTime * 1; // 페이드 인 속도 조절
            yield return null;
        }
        clearPanelCanvasGroup.alpha = 1f; // 마지막에는 완전히 불투명

        // 일정 시간 후에 페이드 아웃
        yield return new WaitForSeconds(7f);

        while (clearPanelCanvasGroup.alpha > 0f)
        {
            clearPanelCanvasGroup.alpha -= Time.deltaTime * 1; // 페이드 아웃 속도 조절
            yield return null;
        }
        clearPanelCanvasGroup.alpha = 0f; // 마지막에는 완전히 투명
        clearPanel.SetActive(false); // 클리어 패널 비활성화
    }

    private IEnumerator ShowPanel(string monsterType, int killed, int toKill)
    {
        countPanel.SetActive(true);
        killText.text = $"{monsterType} 처치 {killed}/{toKill}";

        if (panelCanvasGroup != null)
        {
            panelCanvasGroup.alpha = 0f; // 초기에는 완전히 투명
            while (panelCanvasGroup.alpha < 1f)
            {
                panelCanvasGroup.alpha += Time.deltaTime * 1; // 페이드 인 속도 조절
                yield return null;
            }
            panelCanvasGroup.alpha = 1f; // 마지막에는 완전히 불투명
        }
    }

    private IEnumerator HidePanel()
    {
        if (panelCanvasGroup != null)
        {
            while (panelCanvasGroup.alpha > 0f)
            {
                panelCanvasGroup.alpha -= Time.deltaTime * 1; // 페이드 아웃 속도 조절
                yield return null;
            }
            panelCanvasGroup.alpha = 0f; // 마지막에는 완전히 투명
            countPanel.SetActive(false); // 패널 비활성화
        }
    }

    IEnumerator PlayBossCinematic()
    {
        playerMovement.GetComponent<PlayerMovement>().enabled = false;
        yield return new WaitForSeconds(1f);
        if (bossCinematic != null)
        {
            bossCinematic.Play();

            // 기존 1층 BGM을 멈추고 새로운 보스 BGM 재생
            if (AudioManager.instance != null)
            {
                AudioManager.instance.Stop("1stAreaBgm");
                AudioManager.instance.Play("1stAreaBossBattleBgm");
            }

            // 시네마틱이 끝나고 체력바를 활성화
            yield return new WaitForSeconds((float)bossCinematic.duration);  // bossCinematic.duration은 시네마틱의 전체 길이를 초 단위로 나타냄
            
            // 체력바 캔버스를 아예 활성화하는 대신, 투명도를 조정하는 방식으로 전환
            if (bossHealthBarCanvas != null)
            {
                CanvasGroup bossHealthBarCanvasGroup = bossHealthBarCanvas.GetComponent<CanvasGroup>();
                if (bossHealthBarCanvasGroup != null)
                {
                    bossHealthBarCanvasGroup.alpha = 1f;  // 체력바 보이기
                    bossHealthBarCanvasGroup.interactable = true;
                    bossHealthBarCanvasGroup.blocksRaycasts = true;
                }
                else
                {
                    bossHealthBarCanvas.SetActive(true); // 캔버스 그룹이 없으면 기존 방식 사용
                }
            }

            playerMovement.GetComponent<PlayerMovement>().enabled = true;
        }
    }

    IEnumerator Play1stAreaCinematic()
    {
        playerMovement.GetComponent<PlayerMovement>().enabled = false;
        yield return new WaitForSeconds(1f);
        if (firstAreaCinematic != null)
        {
            firstAreaCinematic.Play();
        }
        yield return new WaitForSeconds((float)firstAreaCinematic.duration);  //firstAreaCinematic.duration은 시네마틱의 전체 길이를 초 단위로 나타냄
        playerMovement.GetComponent<PlayerMovement>().enabled = true;
    }

    IEnumerator SlowMotionEffect()
    {
        // 초기 슬로우 모션 배율로 시작
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            Time.timeScale = Mathf.Lerp(slowMotionInitialScale, slowMotionMidScale, t);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 중간 슬로우 모션 배율로 설정
        Time.timeScale = slowMotionMidScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // 지정한 시간 동안 대기
        yield return new WaitForSecondsRealtime(slowMotionDuration);

        // 다시 원래 속도로 복구
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            Time.timeScale = Mathf.Lerp(slowMotionMidScale, slowMotionFinalScale, t);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 원래 속도로 설정
        Time.timeScale = slowMotionFinalScale;
        Time.fixedDeltaTime = 0.02f;

        // 슬로우 모션이 끝난 후 승리 BGM 재생
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Stop("1stAreaBossBattleBgm"); // 보스 전투 BGM 멈추기
            AudioManager.instance.Play("VictoryBgm");     // 승리 BGM 재생
        }

        // 클리어 패널 보여주기
        if (clearPanel != null)
        {
            StartCoroutine(ShowClearPanel());
        }
    }
}
