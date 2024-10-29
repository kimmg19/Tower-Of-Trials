using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject moveUpPrefab;  
    public float spawnInterval = 5.0f;  
    public Transform spawnPoint;  
    public GameObject objectToPush;  

    private void Start()
    {
        StartCoroutine(SpawnMoveUp());
    }

    IEnumerator SpawnMoveUp()
    {
        while (true)
        {
            GameObject spawnedObject = Instantiate(moveUpPrefab, spawnPoint.position, spawnPoint.rotation);

            moveUpPrefab = spawnedObject;

            MoveUp moveUpScript = spawnedObject.GetComponent<MoveUp>();

            if (moveUpScript != null)
            {
                moveUpScript.objectToPush = objectToPush;  
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
