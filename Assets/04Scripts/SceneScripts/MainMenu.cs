using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] GameObject optionMenu;
    [SerializeField] GameObject infoPanel;
    [SerializeField] GameObject OptionPanel;
    [SerializeField] Slider bgmSlider;

    [SerializeField] GameObject HelpImage1;
    [SerializeField] GameObject HelpImage2;
    [SerializeField] GameObject HelpImage3;

    public RectTransform title; // 타이틀의 RectTransform 추가
    public CanvasGroup titleCanvasGroup; // 타이틀의 CanvasGroup 추가
    public CanvasGroup infoCanvasGroup; // Info 패널의 CanvasGroup 추가
    public float fadeDuration = 2.5f; // 나타나는 시간

    public ButtonEffect buttonEffectScript;

    void Start()
    {
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        GetVolume();

        HelpImage1.SetActive(false);
        HelpImage2.SetActive(false);
        HelpImage3.SetActive(false);

        infoPanel.SetActive(false);
        infoCanvasGroup.alpha = 0; // 패널 초기 상태 투명하게 설정

        StartTitleFadeIn(); // 타이틀 페이드인 애니메이션 시작
    }

    void StartTitleFadeIn()
    {
        titleCanvasGroup.alpha = 0; // 타이틀 초기 상태 투명하게 설정
        LeanTween.alphaCanvas(titleCanvasGroup, 1, fadeDuration)
            .setEaseInOutSine(); // 페이드인 애니메이션 적용
    }

    public void OnClickInfo()
    {
        if (infoPanel.activeSelf)
        {
            HideInfoPanel(); // Info 패널 숨김
        }
        else
        {
            ShowInfoPanel(); // Info 패널 표시
        }
    }

    void ShowInfoPanel()
    {
        infoPanel.SetActive(true); // 패널을 활성화
        infoCanvasGroup.alpha = 0; // 초기 상태 투명하게 설정
        infoPanel.GetComponent<RectTransform>().localScale = Vector3.zero; // 초기 크기 0으로 설정

        // 패널이 부드럽게 나타나고 확대되도록 애니메이션 설정
        LeanTween.alphaCanvas(infoCanvasGroup, 1, fadeDuration)
            .setEaseInOutSine();
        LeanTween.scale(infoPanel, Vector3.one, fadeDuration)
            .setEaseInOutBack(); // 부드럽게 확대
    }

    void HideInfoPanel()
    {
        // 패널이 부드럽게 사라지도록 애니메이션 설정
        LeanTween.alphaCanvas(infoCanvasGroup, 0, fadeDuration)
            .setEaseInOutSine()
            .setOnComplete(() => infoPanel.SetActive(false)); // 애니메이션 완료 후 비활성화
        LeanTween.scale(infoPanel, Vector3.zero, fadeDuration)
            .setEaseInOutBack(); // 부드럽게 축소
    }

    void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }
    
    public void OnClickGameStart()
    {
        LoadingSceneManager.LoadScene(2);
    }

    public void OnClickLoad()
    {
        Debug.Log("Loading Game...");
    }

    public void OnClickOption()
    {
        optionMenu.SetActive(!optionMenu.activeSelf);
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    void GetVolume()
    {
        float bgmVolume;
        audioMixer.GetFloat("BGM", out bgmVolume);   
        bgmSlider.value = Mathf.Pow(10, bgmVolume / 20);
    }

    void OnPause()
    {
        OnClickOption();
    }

    public void OnClickCloseOptionButton()
    {
        if (OptionPanel.activeSelf)
        {
            OptionPanel.SetActive(false);
        }
    }

    public void OnClickHelp()
    {
        if (!HelpImage1.activeSelf)
        {
            HelpImage1.SetActive(true);
        }

        if (buttonEffectScript != null)
        {
            buttonEffectScript.enabled = false;
        }
    }

    public void OnClickHelpClose1()
    {
        if (HelpImage1.activeSelf)
        {
            HelpImage1.SetActive(false);
        }
    }

    public void OnClickHelpClose2()
    {
        if (HelpImage2.activeSelf)
        {
            HelpImage2.SetActive(false);
        }
    }

    public void OnClickHelpClose3()
    {
        if (HelpImage3.activeSelf)
        {
            HelpImage3.SetActive(false);
        }
    }

    public void OnClickHelpNext()
    {
        if (HelpImage1.activeSelf)
        {
            HelpImage1.SetActive(false);
            HelpImage2.SetActive(true);
        }
    }

    public void OnClickHelpNext2()
    {
        if (HelpImage2.activeSelf)
        {
            HelpImage2.SetActive(false);
            HelpImage3.SetActive(true);
        }
    }

    public void OnClickHelpPrev()
    {
        if (HelpImage2.activeSelf)
        {
            HelpImage2.SetActive(false);
            HelpImage1.SetActive(true);
        }
    }

    public void OnClickHelpPrev2()
    {
        if (HelpImage3.activeSelf)
        {
            HelpImage3.SetActive(false);
            HelpImage2.SetActive(true);
        }
    }
}
