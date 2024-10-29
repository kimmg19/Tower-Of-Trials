using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class ShowClear : MonoBehaviour
{
    [SerializeField] GameObject titlePanel;  // Ÿ��Ʋ �г�
    [SerializeField] GameObject clearPanel; // Ŭ���� �г�
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
            titleCanvasGroup.alpha = 0f; // �ʱ⿡�� ������ ����
            while (titleCanvasGroup.alpha < 1f)
            {
                titleCanvasGroup.alpha += Time.deltaTime * 1; // ���̵� �� �ӵ� ����
                yield return null;
            }
            titleCanvasGroup.alpha = 1f; // ���������� ������ ������

            // Ÿ��Ʋ�� ���� �ð� �Ŀ� ���������
            yield return new WaitForSeconds(3f);

            while (titleCanvasGroup.alpha > 0f)
            {
                titleCanvasGroup.alpha -= Time.deltaTime * 1; // ���̵� �ƿ� �ӵ� ����
                yield return null;
            }
            titleCanvasGroup.alpha = 0f; // ���������� ������ ����
            titlePanel.SetActive(false); // Ÿ��Ʋ ��Ȱ��ȭ
        }
    }

    private IEnumerator ShowClearPanel()
    {
        AudioManager.instance.Play("VictoryBgm");
        clearPanel.SetActive(true);
        clearPanelCanvasGroup.alpha = 0f; // �ʱ⿡�� ������ ����
        while (clearPanelCanvasGroup.alpha < 1f)
        {
            clearPanelCanvasGroup.alpha += Time.deltaTime * 1; // ���̵� �� �ӵ� ����
            yield return null;
        }
        clearPanelCanvasGroup.alpha = 1f; // ���������� ������ ������

        // ���� �ð� �Ŀ� ���̵� �ƿ�
        yield return new WaitForSeconds(7f);

        while (clearPanelCanvasGroup.alpha > 0f)
        {
            clearPanelCanvasGroup.alpha -= Time.deltaTime * 1; // ���̵� �ƿ� �ӵ� ����
            yield return null;
        }
        clearPanelCanvasGroup.alpha = 0f; // ���������� ������ ����
        clearPanel.SetActive(false); // Ŭ���� �г� ��Ȱ��ȭ
    }
}
