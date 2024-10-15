using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    public GameObject player;
    public PlayableDirector storyCutscene; // 타임라인(StoryCutScene) 참조
    public string playerTag = "Player";    // 플레이어의 태그
    private string cutscenePlayedKey = "CutscenePlayed"; // 컷씬 재생 상태를 저장할 PlayerPrefs 키

    PlayerInputs playerInputs;

    void Start()
    {
        playerInputs = player.GetComponent<PlayerInputs>();

        // 이전에 컷씬이 재생된 적이 있는지 확인
        if (PlayerPrefs.GetInt(cutscenePlayedKey, 0) == 1)
        {
            // 이미 재생된 경우 더 이상 실행되지 않도록 설정
            hasPlayed = true;
        }
    }

    private bool hasPlayed = false; // 재트리거 방지용 변수

    void OnTriggerEnter(Collider other)
    {
        // 트리거에 들어온 객체가 플레이어인지 확인
        if (other.CompareTag(playerTag) && !hasPlayed)
        {
            hasPlayed = true;
            PlayCutscene();
        }
    }

    void PlayCutscene()
    {
        if (storyCutscene != null)
        {
            playerInputs.enabled = false;  // 플레이어 입력 비활성화

            AudioManager.instance.Stop("MainAreaBgm"); // 메인 BGM 끄기
            storyCutscene.stopped += OnCutsceneStopped; // 타임라인 종료 시 호출할 메서드 등록
            storyCutscene.Play();

            // 컷씬이 재생되었음을 PlayerPrefs에 저장
            PlayerPrefs.SetInt(cutscenePlayedKey, 1);
            PlayerPrefs.Save(); // PlayerPrefs 즉시 저장
        }
        else
        {
            Debug.LogWarning("StoryCutScene이 인스펙터에 할당되지 않았습니다.");
        }
    }

    void OnCutsceneStopped(PlayableDirector director)
    {
        AudioManager.instance.Play("MainAreaBgm"); // 타임라인이 끝난 후 메인 BGM 재생
        playerInputs.enabled = true;
        director.stopped -= OnCutsceneStopped; // 이벤트 핸들러 해제
    }
}
