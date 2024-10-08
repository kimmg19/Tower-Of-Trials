using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using System.Collections;


public class AltarInteraction : MonoBehaviour
{
    public GameObject player; // Player 오브젝트를 할당
    public bool isPlayerInRange = false; 
    public string requiredKeyItemName = "Key"; 
    public TextMeshProUGUI interactionText; 
    PlayerInputs playerInputs;

    public GameObject[] pillars; // 5개의 기둥 오브젝트
    public ParticleSystem[] pillarParticles; // 각 기둥에 연결된 파티클 시스템
    private int currentPillarIndex = 0; // 현재 활성화할 기둥 인덱스
    private bool isInteracting = false; // 상호작용 중인지 확인하는 플래그

    [Header("Cinematic Settings")]
    public PlayableDirector secondAreaClearDirector; // 2nd Area Clear 타임라인
    public SecondAreaManager secondAreaManager; // 2구역 매니저

    private void Start()
    {
        playerInputs = player.GetComponent<PlayerInputs>();
        interactionText.gameObject.SetActive(false); 

        // 타임라인이 끝났을 때 호출되는 이벤트 추가
        if (secondAreaClearDirector != null)
        {
            secondAreaClearDirector.stopped += OnSecondAreaClearCinematicEnd;
        }
    }

    private void Update()
    {
        if (isPlayerInRange && playerInputs.isGPress && !isInteracting)
        {
            isInteracting = true;
            AttemptInteraction();
        }

        if (!playerInputs.isGPress)
        {
            isInteracting = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true; 
            ShowInteractionText(); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false; 
            interactionText.gameObject.SetActive(false); 
        }
    }

    void AttemptInteraction()
    {
        Inventory playerInventory = Inventory.instance;

        Item keyItem = playerInventory.items.Find(item => item.itemName == requiredKeyItemName);
        if (keyItem != null && currentPillarIndex < pillars.Length)
        {
            Debug.Log("Key item found! Interaction with altar successful.");
            TriggerAltarEvent();

            playerInventory.RemoveItem(playerInventory.items.IndexOf(keyItem));
            interactionText.gameObject.SetActive(false);
        }
        
        UpdateInteractionText();
    }

    void TriggerAltarEvent()
    {
        if (currentPillarIndex < pillarParticles.Length)
        {
            if (!pillarParticles[currentPillarIndex].gameObject.activeSelf)
            {
                pillarParticles[currentPillarIndex].gameObject.SetActive(true); 
            }
            AudioManager.instance.Play("2ndAreaFireOn");
            pillarParticles[currentPillarIndex].Play();
            currentPillarIndex++;

            // 기둥이 5개 모두 활성화되면 타임라인 실행
            if (currentPillarIndex == pillarParticles.Length)
            {
                Invoke("PlaySecondAreaClearScene", 2f); // 2초 후에 타임라인 실행
            }
        }

        Debug.Log("Altar event triggered!");
    }

    void PlaySecondAreaClearScene()
    {
        if (secondAreaClearDirector != null)
        {
            AudioManager.instance.Stop("2ndAreaBgm");
            secondAreaClearDirector.Play(); // 2nd Area Clear 타임라인 실행
            Debug.Log("2nd Area Clear Scene played!");
        }
        else
        {
            Debug.LogWarning("secondAreaClearDirector is not assigned!");
        }
    }

    // 타임라인이 끝나면 호출되는 함수
    void OnSecondAreaClearCinematicEnd(PlayableDirector director)
    {
        if (director == secondAreaClearDirector)
        {
            // 타임라인이 끝났을 때 2구역 매니저의 클리어 패널 처리 로직을 1초 후에 실행
            StartCoroutine(DelayedClearPanelFade(1f)); // 1초 후에 실행
        }
    }

    // 1초 후에 클리어 패널
    private IEnumerator DelayedClearPanelFade(float delay)
    {
        yield return new WaitForSeconds(delay); // 지정한 시간만큼 대기
        secondAreaManager.StartClearPanelFade(); // 클리어 패널 처리 로직 실행
    }

    void ShowInteractionText()
    {
        UpdateInteractionText();
    }

    void UpdateInteractionText()
        {
            Inventory playerInventory = Inventory.instance;

            // 남은 열쇠가 있는지 확인
            Item keyItem = playerInventory.items.Find(item => item.itemName == requiredKeyItemName);
            if (keyItem != null)
            {
                interactionText.text = "'G'키를 눌러 열쇠 사용";
                interactionText.gameObject.SetActive(true);
            }
            else
            {
                // 열쇠가 없으면 텍스트 숨김
                interactionText.gameObject.SetActive(false);
            }
        }
}
