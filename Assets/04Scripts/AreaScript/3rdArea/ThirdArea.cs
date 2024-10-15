using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class ThirdArea : MonoBehaviour
{
    PlayerStatus playerstatus;
    GameObject player;
    bool hasDescentTriggered = false; // �̹� HP ���ҿ� ��ġ �̵��� �Ǿ����� ���θ� Ȯ��
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] PlayableDirector ThirdAreaCinematic;
    [SerializeField] GameObject playermesh;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject ingameCanvas;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerstatus = player.GetComponent<PlayerStatus>();

        StartCoroutine("Play3rdAreaCinematic");
        //playermesh.SetActive(false);
        //playerUI.SetActive(false);
        //ingameCanvas.SetActive(false);
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasDescentTriggered)
        {
            Debug.Log("�浹!");
            playerstatus.Descent(); // HP ���� �� ���̺� ����Ʈ�� �̵�
            //hasDescentTriggered = true; // �� ���� ȣ��ǵ��� ����
        }
    }
    */
    public void ResetDescentTrigger()
    {
        hasDescentTriggered = false;
    }

    IEnumerator Play3rdAreaCinematic()
    {
        playerInputs.isInteracting = true;

        yield return new WaitForSeconds(1f);
        if (ThirdAreaCinematic != null)
        {
            ThirdAreaCinematic.Play();
        }
        yield return new WaitForSeconds((float)ThirdAreaCinematic.duration);  //ThirdAreaCinematic.duration�� �ó׸�ƽ�� ��ü ���̸� �� ������ ��Ÿ��
        playerInputs.isInteracting = false;
        Debug.Log("�ƾ���");
        playermesh.SetActive(true);
        playerUI.SetActive(true);
        ingameCanvas.SetActive(true);
    }
}
