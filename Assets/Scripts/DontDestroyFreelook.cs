using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyFreelook : MonoBehaviour
{
    public static DontDestroyFreelook instance; // 변수추가
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
