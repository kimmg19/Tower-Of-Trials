using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    // 관련된 UI 캔버스
    [SerializeField]
    private GameObject floorSelection;
    [SerializeField]
    private GameObject AskSelection;

    private bool floorSelectionActive = false; // floorSelection의 현재 활성화 상태를 저장하는 변수

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && AskSelection.activeSelf)
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
        LoadingSceneManager.LoadScene("1stFloor");
    }
}
