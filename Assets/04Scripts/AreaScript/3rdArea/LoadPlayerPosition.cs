using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{
    public GameObject player; 
    public PlayerStatus playerstatus;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); 
        playerstatus = player.GetComponent<PlayerStatus>(); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerstatus = player.GetComponent<PlayerStatus>();
            if (playerstatus != null) // null üũ
            {
                playerstatus.Descent(); 
            }
           
        }
    }
}
