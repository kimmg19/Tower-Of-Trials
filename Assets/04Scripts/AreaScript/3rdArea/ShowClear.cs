using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class ShowClear : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;  // 타이틀 패널
    [SerializeField] GameObject clearPanel; // 클리어 패널
    [SerializeField] PlayerInputs playerInputs;
    private CanvasGroup titleCanvasGroup;
    private CanvasGroup clearPanelCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        titleCanvasGroup = titlePanel.GetComponent<CanvasGroup>();
        clearPanelCanvasGroup = clearPanel.GetComponent<CanvasGroup>();

        if (clearPanel != null)
        {
            clearPanel.SetActive(false);
        }

        if (titleCanvasGroup != null)
        {
            StartCoroutine(ShowTitlePanel());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("클리어패널 등장");
            if (clearPanel != null)
            {
                StartCoroutine(ShowClearPanel());
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
        AudioManager.instance.Play("VictoryBgm");
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
}
