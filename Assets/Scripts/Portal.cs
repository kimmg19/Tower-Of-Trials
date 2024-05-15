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
    GameObject obj;
    PlayerMovement playerMovement;
    void Start()
    {
        obj = GameObject.Find("Player");
        playerMovement = obj.GetComponent<PlayerMovement>();
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
            floorSelection.SetActive(true);
            AskSelection.SetActive(false);

        }
        else playerMovement.isGPress = false;
    }

    // Player가 포탈에 진입할 때
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 캔버스 활성화
            if (AskSelection != null)
            {
                AskSelection.SetActive(true);
            }
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
    }

    public void OnClick1stFloor()
    {
        LoadingSceneManager.LoadScene(3);
        Time.timeScale = 1f;
    }

    public void OnClick2ndFloor()
    {
        LoadingSceneManager.LoadScene(4);
        Time.timeScale = 1f;
    }

    public void OnClickCloseButton()
    {
        AskSelection.SetActive(true);
        floorSelection.SetActive(false);
    }
}