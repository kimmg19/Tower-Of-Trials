using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject moveUpPrefab;  // MoveUp 프리팹
    public float spawnInterval = 5.0f;  // 오브젝트가 생성될 주기 (초)
    public Transform spawnPoint;  // 오브젝트가 생성될 위치
    public GameObject objectToPush;  // 밀려야 할 오브젝트 (스포너에서 참조할 오브젝트)

    private void Start()
    {
        // 주기적으로 MoveUp 오브젝트를 생성
        StartCoroutine(SpawnMoveUp());
    }

    IEnumerator SpawnMoveUp()
    {
        while (true)
        {
            // MoveUp 프리팹의 인스턴스 생성
            GameObject spawnedObject = Instantiate(moveUpPrefab, spawnPoint.position, spawnPoint.rotation);

            // 스폰된 오브젝트를 MoveUpPrefab으로 설정 (스폰된 것을 참조하기 위함)
            moveUpPrefab = spawnedObject;

            // 스폰된 오브젝트의 MoveUp 스크립트를 가져옴
            MoveUp moveUpScript = spawnedObject.GetComponent<MoveUp>();

            // objectToPush를 설정된 오브젝트로 할당
            if (moveUpScript != null)
            {
                moveUpScript.objectToPush = objectToPush;  // objectToPush 할당
            }

            // 지정된 시간만큼 대기 후 다시 생성
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
