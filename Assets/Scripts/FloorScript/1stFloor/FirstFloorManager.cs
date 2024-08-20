using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.UI;

public class FirstFloorManager : MonoBehaviour
{
    public PlayableDirector bossCinematic; // 타임라인 에셋을 재생할 PlayableDirector
    public GameObject slimeGroup;
    public GameObject turtleGroup;
    public GameObject golemDoor;
    public GameObject rewardChest; // 보물상자 오브젝트
    public GameObject titlePanel;  // 타이틀 패널
    public GameObject panel;
    public TextMeshProUGUI killText;

    private CanvasGroup panelCanvasGroup;
    private CanvasGroup titleCanvasGroup;

    private int slimesToKill = 5;
    private int turtlesToKill = 5;
    private int slimesKilled = 0;
    private int turtlesKilled = 0;

    private bool slimesSpawned = false;
    private bool turtlesSpawned = false;

    void Start()
    {
        panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        titleCanvasGroup = titlePanel.GetComponent<CanvasGroup>();

        if (panelCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup component missing on the CountPanel GameObject.");
        }

        if (titleCanvasGroup == null)
        {
            Debug.LogError("CanvasGroup component missing on the TitlePanel GameObject.");
        }

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

        if (panel != null)
        {
            panel.SetActive(false);
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
        if (rewardChest != null)
        {
            rewardChest.SetActive(true); // 보물상자 활성화
            Debug.Log("Reward chest has been activated!");
        }
    }

    public void OnSlimeKilled()
    {
        slimesKilled++;
        UpdateKillText("슬라임", slimesKilled, slimesToKill);

        if (slimesKilled >= slimesToKill)
        {
            CheckFloorCompletion();
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
            CheckFloorCompletion();
            PlayBossCinematic();
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

    private void CheckFloorCompletion()
    {
        if (slimesKilled >= slimesToKill && turtlesKilled >= turtlesToKill)
        {
            HandleFloorCompletion();
        }
    }

    private void HandleFloorCompletion()
    {
        if (golemDoor != null)
        {
            golemDoor.SetActive(false); // 이때 열림
            Debug.Log("Golem door is now open!");
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
            Debug.Log("Slimes spawned!");
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
            Debug.Log("Turtles spawned!");
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

    private IEnumerator ShowPanel(string monsterType, int killed, int toKill)
    {
        panel.SetActive(true);
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
            panel.SetActive(false); // 패널 비활성화
        }
    }

    private void PlayBossCinematic()
    {
        if (bossCinematic != null)
        {
            bossCinematic.Play();
        }
    }
}
