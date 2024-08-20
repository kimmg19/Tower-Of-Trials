using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManagement : MonoBehaviour
{
    [SerializeField]
    private GameObject buff;
    [SerializeField]
    private GameObject debuff;
    [SerializeField]
    private GameObject Randombuff;
    [SerializeField]
    private GameObject buffyicon;
    [SerializeField]
    private GameObject debuffyicon;
    //private bool hasCollided = false;

    GameObject obj;
    PlayerMovement playerMovement;
    PlayerStats playerstats;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("Player");
        playerMovement = obj.GetComponent<PlayerMovement>();
        playerstats = obj.GetComponent<PlayerStats>();
   
        if (buff != null)
        {
            buff.SetActive(false);
        }

        if (debuff != null)
        {
            debuff.SetActive(false);
        }

        if (buffyicon != null)
        {
           buffyicon.SetActive(false);
        }

        if (debuffyicon != null)
        {
            debuffyicon.SetActive(false);
        }

        if (Randombuff == null)
        {
            Debug.LogError("Randombuff가 할당되지 않았습니다.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 랜덤하게 1 또는 2를 생성
            int randomNumber = Random.Range(1, 3);
            if(randomNumber == 1)
            {
                print("버프 ㅊㅊ");
                buff.SetActive(true);
                buffyicon.SetActive(true);
                Randombuff.SetActive(false);
                playerMovement.Buffspeed();

                Invoke("BuffBlinkoff", 3.0f);
                Invoke("BuffBlinkon", 3.5f);
                Invoke("BuffBlinkoff", 4.0f);
                Invoke("BuffBlinkon", 4.5f);
                Invoke("BuffBlinkoff", 5.0f);
                Invoke("ResetSpeed", 5.0f);

            }
            else
            {
                print("디버프 ㅋㅋ");
                debuff.SetActive(true);
                debuffyicon.SetActive(true);
                Randombuff.SetActive(false);
                playerMovement.Debuffspeed();

                Invoke("DeBuffBlinkoff", 3.0f);
                Invoke("DeBuffBlinkon", 3.5f);
                Invoke("DeBuffBlinkoff", 4.0f);
                Invoke("DeBuffBlinkon", 4.5f);
                Invoke("DeBuffBlinkoff", 5.0f);
                Invoke("ResetSpeed", 5.0f);

            }
        }
    }

    void ResetSpeed()
    {
        playerMovement.speed = 1.0f;
        playerstats.sprintSpeed = 1.5f;
    }

    void BuffBlinkoff()
    {
        buffyicon.SetActive(false);
    }
    void BuffBlinkon()
    {
        buffyicon.SetActive(true);
    }
    void DeBuffBlinkoff()
    {
        debuffyicon.SetActive(false);
    }
    void DeBuffBlinkon()
    {
        debuffyicon.SetActive(true);
    }

}
