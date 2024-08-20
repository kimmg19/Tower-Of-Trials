using UnityEngine;

public class FirstFloorTrigger : MonoBehaviour
{
    public FirstFloorManager floorManager;
    public bool isSlimeTrigger;  // 슬라임 트리거인지 여부
    public bool isTurtleTrigger; // 거북이 트리거인지 여부

    private bool hasTriggered = false; // 트리거가 한 번만 작동하도록

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            if (isSlimeTrigger)
            {
                floorManager.SpawnSlimes();
            }

            if (isTurtleTrigger)
            {
                floorManager.SpawnTurtles();
            }

            // 트리거는 한 번 작동한 후 비활성화되도록 설정
            gameObject.SetActive(false);
        }
    }
}
