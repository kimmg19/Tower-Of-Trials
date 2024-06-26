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
    GameObject player;
    PlayerInputs playerInputs;
    void Start()
    {
        player = GameObject.Find("Player");
        playerInputs = player.GetComponent<PlayerInputs>();
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
        if (playerInputs.isGPress && AskSelection.activeSelf)
        {
            floorSelection.SetActive(true);
            AskSelection.SetActive(false);
        }
        else playerInputs.isGPress = false;
    }

    // Player가 포탈에 진입할 때
    void OnTriggerEnter(Collider other)
    {
        playerInputs.isGPress = false;

        if (other.CompareTag("Player"))
        {
            // 캔버스 활성화
            AskSelection.SetActive(true);
        }
    }

    // Player가 포탈에서 나갈 때
    void OnTriggerExit(Collider other)
    {
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
        playerInputs.isGPress = false;
    }

    public void OnClick1stFloor()
    {
        MovePlayerToScene(3, new Vector3(-6, 0, 20)); // 1층으로 이동, 원하는 위치 설정
    }

    public void OnClick2ndFloor()
    {
        MovePlayerToScene(4, new Vector3(25, 10, 40)); // 2층으로 이동, 원하는 위치 설정
    }

    public void OnClickCloseButton()
    {
        AskSelection.SetActive(true);
        floorSelection.SetActive(false);
    }

    public void MovePlayerToScene(int sceneIndex, Vector3 newPosition)
    {
        LoadingSceneManager.LoadScene(sceneIndex);
        player.GetComponent<DontDestroyPlayer>().MoveToNewPosition(newPosition);
        Time.timeScale = 1f;
    }
}