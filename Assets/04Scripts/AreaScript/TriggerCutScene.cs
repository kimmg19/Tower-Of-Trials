using UnityEngine;
using UnityEngine.Playables;

public class TriggerCutscene : MonoBehaviour
{
    public PlayableDirector storyCutscene; // Reference to the Timeline (StoryCutScene)
    public string playerTag = "Player";    // The tag of the player

    private bool hasPlayed = false; // To prevent re-triggering

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
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
            AudioManager.instance.Stop("MainAreaBgm"); // 메인 BGM 끄기
            storyCutscene.stopped += OnCutsceneStopped; // 타임라인 종료 시 호출할 메서드 등록
            storyCutscene.Play();
        }
        else
        {
            Debug.LogWarning("StoryCutScene not assigned in the inspector.");
        }
    }

    void OnCutsceneStopped(PlayableDirector director)
    {
        AudioManager.instance.Play("MainAreaBgm"); // 타임라인이 끝난 후 메인 BGM 켜기
        director.stopped -= OnCutsceneStopped; // 이벤트 핸들러 등록 해제
    }
}
