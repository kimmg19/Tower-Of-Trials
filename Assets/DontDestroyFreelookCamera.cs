using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DontDestroyFreelookCamera : MonoBehaviour
{
    CinemachineFreeLook cinemachineFreeLook;
    Transform player;
    public static DontDestroyFreelookCamera Instance = null;
    private void Awake()
    {
        Transform player=GameObject.Find("Player").transform;
        if (Instance == null)
        {           

            Instance = this;
            
        } else
        {          

            Destroy(this.gameObject);
            GetComponent<CinemachineFreeLook>().LookAt = player;
            GetComponent<CinemachineFreeLook>().Follow = player;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
