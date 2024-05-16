using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [SerializeField]
    private GameObject buff;
    [SerializeField]
    private GameObject debuff;
    [SerializeField]
    private GameObject Randombuff;
    [SerializeField] public GameObject Buff_icon;
    [SerializeField] public GameObject Debuff_icon;

    GameObject obj;
    PlayerUI ui;
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("Player");
        playerMovement = obj.GetComponent<PlayerMovement>();
        ui = obj.GetComponent<PlayerUI>();
   
        if (buff != null)
        {
            buff.SetActive(false);
        }

        if (debuff != null)
        {
            debuff.SetActive(false);
        }

        if (Buff_icon != null)
        {
            Buff_icon.SetActive(false);
        }

        if (Debuff_icon != null)
        {
            Debuff_icon.SetActive(false);
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
                Buff_icon.SetActive(true);
                //playerMovement.speed += 0.5f;
                StartCoroutine(OnBuff());
                Buff_icon.SetActive(false);
                //playerMovement.speed -= 0.5f;
            }
            else
            {
                print("����� ����");
                debuff.SetActive(true);
                Debuff_icon.SetActive(true);
                //playerMovement.speed -= 0.5f;
                StartCoroutine(OnBuff());
                Debuff_icon.SetActive(false);
                //playerMovement.speed += 0.5f;
            }
        }
    }
    IEnumerator OnBuff()
    {
        // 1�� ���
        yield return new WaitForSeconds(5.0f);

        // 1�� �Ŀ� buff�� ��Ȱ��ȭ
        buff.SetActive(false);
        debuff.SetActive(false);
        Randombuff.SetActive(false);
        print("�ڷ�ƾ�۵�");
    }
}
