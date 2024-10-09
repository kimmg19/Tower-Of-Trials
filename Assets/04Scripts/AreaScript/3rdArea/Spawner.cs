using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject moveUpPrefab;  // MoveUp ������
    public float spawnInterval = 5.0f;  // ������Ʈ�� ������ �ֱ� (��)
    public Transform spawnPoint;  // ������Ʈ�� ������ ��ġ
    public GameObject objectToPush;  // �з��� �� ������Ʈ (�����ʿ��� ������ ������Ʈ)

    private void Start()
    {
        // �ֱ������� MoveUp ������Ʈ�� ����
        StartCoroutine(SpawnMoveUp());
    }

    IEnumerator SpawnMoveUp()
    {
        while (true)
        {
            // MoveUp �������� �ν��Ͻ� ����
            GameObject spawnedObject = Instantiate(moveUpPrefab, spawnPoint.position, spawnPoint.rotation);

            // ������ ������Ʈ�� MoveUpPrefab���� ���� (������ ���� �����ϱ� ����)
            moveUpPrefab = spawnedObject;

            // ������ ������Ʈ�� MoveUp ��ũ��Ʈ�� ������
            MoveUp moveUpScript = spawnedObject.GetComponent<MoveUp>();

            // objectToPush�� ������ ������Ʈ�� �Ҵ�
            if (moveUpScript != null)
            {
                moveUpScript.objectToPush = objectToPush;  // objectToPush �Ҵ�
            }

            // ������ �ð���ŭ ��� �� �ٽ� ����
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
