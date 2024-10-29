using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class ThirdArea : MonoBehaviour
{
    PlayerStatus playerstatus;
    GameObject player;

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
        playermesh.SetActive(true);
        playerUI.SetActive(true);
        ingameCanvas.SetActive(true);
    }
}
