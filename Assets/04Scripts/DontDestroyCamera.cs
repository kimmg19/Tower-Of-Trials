using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCamera : MonoBehaviour
{
    public static DontDestroyCamera instance; // 변수추가
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
}
