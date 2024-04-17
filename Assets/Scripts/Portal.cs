using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Portal : MonoBehaviour
{
    // 관련된 UI 캔버스
    [SerializeField]
    private GameObject floorSelection;
    [SerializeField]
    private GameObject AskSelection;
    [SerializeField]
    PlayerMovement playerMovement;
    void Start()
    {
        if (floorSelection != null)
        {
            floorSelection.SetActive(false);
        }

        if (AskSelection != null)
        {
            AskSelection.SetActive(false);
        }
    }
    

    void Update()
    {
        if (playerMovement.isGPress && AskSelection.activeSelf)
        {
            AskSelection.SetActive(false);
            floorSelection.SetActive(true);
        }
    }

    // 충돌체가 이 포탈에 진입할 때 호출됨
    void OnTriggerEnter(Collider other)
    {
        // 충돌체가 "Player" 태그인 경우
        if (other.CompareTag("Player"))
        {
            // 캔버스 활성화
            if (AskSelection != null)
            {
                AskSelection.SetActive(true);
                print("Player entered the portal.");
            }
        }
    }

    // 충돌체가 이 포탈에서 나갈 때 호출됨
    void OnTriggerExit(Collider other)
    {
        // 충돌체가 "Player" 태그인 경우
        if (other.CompareTag("Player"))
        {
            // 캔버스 비활성화
            if (AskSelection != null)
            {
                AskSelection.SetActive(false);
            }

            if (floorSelection != null)
            {
                floorSelection.SetActive(false);
            }
        }
    }

    public void OnClick1stFloor()
    {
        Destroy(AudioManager.instance.gameObject); // 층 넘어가면 bgm x
        LoadingSceneManager.LoadScene("1stFloor");
    }

    public void OnClickCloseButton()
    {
        floorSelection.SetActive(false);
    }
}
