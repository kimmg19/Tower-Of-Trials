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
    GameObject obj;
    PlayerInputs playerInputs;
    [SerializeField]GameObject playerUI;


    void Start()
    {
        obj = GameObject.Find("Player");
        playerInputs = obj.GetComponent<PlayerInputs>();
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
            playerInputs.isInteracting = true;
            floorSelection.SetActive(true);
            AskSelection.SetActive(false);
            playerUI.SetActive(false);
            playerInputs.isInteracting = true;
        }
    }

    // Player가 포탈에 진입할 때
    void OnTriggerEnter(Collider other)
    {

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
            playerInputs.isInteracting = false;

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
        print("눌렀음");
        if (floorSelection != null)
        {
            floorSelection.SetActive(false);
        }
        else
        {
            Debug.LogWarning("floorSelection is null!");
        }
        playerUI.SetActive(true);
        playerInputs.isGPress = false;
        //AskSelection.SetActive(true);
        playerInputs.isInteracting = false;

    }
}
