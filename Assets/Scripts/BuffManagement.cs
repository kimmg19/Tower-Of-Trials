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
            // �����ϰ� 1 �Ǵ� 2�� ����
            int randomNumber = Random.Range(1, 3);
            if(randomNumber == 1)
            {
                print("���� ����");
                buff.SetActive(true);
                buffyicon.SetActive(true);
                playerMovement.Buffspeed();
                //playerMovement.SetSpeed(playerMovement.speed + 1.5f);
                StartCoroutine(OnBuff());
            }
            else
            {
                print("����� ����");
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
        // 1�� ���
        yield return new WaitForSeconds(5.0f);

        // 1�� �Ŀ� buff�� ��Ȱ��ȭ
        buff.SetActive(false);
        Randombuff.SetActive(false);
        buffyicon.SetActive(false);
        playerMovement.Debuffspeed();
       // playerMovement.SetSpeed(playerMovement.speed - 1.5f);
        print("���� �ڷ�ƾ �۵�");
    }
    IEnumerator OnDebuff()
    {
        // 1�� ���
        yield return new WaitForSeconds(5.0f);

        // 1�� �Ŀ� buff�� ��Ȱ��ȭ
        debuff.SetActive(false);
        Randombuff.SetActive(false);
        debuffyicon.SetActive(false);
        playerMovement.Buffspeed();
        //playerMovement.SetSpeed(playerMovement.speed + 1.5f);
        print("����� �ڷ�ƾ �۵�");
    }
}
