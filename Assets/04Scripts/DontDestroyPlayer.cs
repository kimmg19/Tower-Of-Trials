using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlayer : MonoBehaviour
{
    public static DontDestroyPlayer instance; // 변수추가
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
           
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    public void MoveToNewPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}
