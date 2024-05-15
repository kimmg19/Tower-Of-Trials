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

    GameObject obj;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("Player");
        playerMovement = obj.GetComponent<PlayerMovement>();
   
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            //Randombuff.SetActive(false);
            // 랜덤하게 1 또는 2를 생성
            int randomNumber = Random.Range(1, 3);
            if(randomNumber == 1)
            {
                print("버프 ㅊㅊ");
                buff.SetActive(true);
                buffyicon.SetActive(true);
                playerMovement.Buffspeed();
                //playerMovement.SetSpeed(playerMovement.speed + 1.5f);
                StartCoroutine(OnBuff());
            }
            else
            {
                print("디버프 ㅋㅋ");
                debuff.SetActive(true);
                debuffyicon.SetActive(true);
                playerMovement.Debuffspeed();
                //playerMovement.SetSpeed(playerMovement.speed - 1.5f);
                StartCoroutine(OnDebuff());
            }
        }
    }
    IEnumerator OnBuff()
    {
        // 1초 대기
        yield return new WaitForSeconds(5.0f);

        // 1초 후에 buff를 비활성화
        buff.SetActive(false);
        Randombuff.SetActive(false);
        buffyicon.SetActive(false);
        playerMovement.Debuffspeed();
       // playerMovement.SetSpeed(playerMovement.speed - 1.5f);
        print("버프 코루틴 작동");
    }
    IEnumerator OnDebuff()
    {
        // 1초 대기
        yield return new WaitForSeconds(5.0f);

        // 1초 후에 buff를 비활성화
        debuff.SetActive(false);
        Randombuff.SetActive(false);
        debuffyicon.SetActive(false);
        playerMovement.Buffspeed();
        //playerMovement.SetSpeed(playerMovement.speed + 1.5f);
        print("디버프 코루틴 작동");
    }
}
