using UnityEngine;
using UnityEngine.InputSystem;

public class ComebackPortal : MonoBehaviour
{
    // 포탈 관련 UI
    [SerializeField]
    private GameObject AskSelection;

    GameObject obj;
    PlayerInputs playerInputs;

    void Start()
    {
        // Player와 PlayerInputs 설정
        obj = GameObject.Find("Player");
        playerInputs = obj.GetComponent<PlayerInputs>();

        if (AskSelection != null)
        {
            AskSelection.SetActive(false); // 처음에는 비활성화 상태
        }
    }

    void Update()
    {
        // G키를 눌렀고, AskSelection UI가 활성화되어 있을 때
        if (playerInputs.isGPress && AskSelection.activeSelf)
        {
            playerInputs.isInteracting = true;
            // 마을로 돌아가기 로직
            OnClickComeback();
            playerInputs.isInteracting = false;
        }
    }

    // Player가 포탈에 진입할 때
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 상호작용 UI 활성화
            AskSelection.SetActive(true);
        }
    }

    // Player가 포탈에서 나갈 때
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 상호작용 UI 비활성화
            AskSelection.SetActive(false);
            playerInputs.isInteracting = false;
        }
    }

    // 마을로 돌아가는 버튼을 눌렀을 때
    public void OnClickComeback()
    {
        // 마을 장면으로 로드
        LoadingSceneManager.LoadScene(2);
        Time.timeScale = 1f; // 게임 속도 정상화
    }
}
