using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class ThirdArea : MonoBehaviour
{
    PlayerStatus playerstatus;
    GameObject player;
    bool hasDescentTriggered = false; // 이미 HP 감소와 위치 이동이 되었는지 여부를 확인
    [SerializeField] PlayerInputs playerInputs;
    [SerializeField] PlayableDirector ThirdAreaCinematic;
    [SerializeField] GameObject playermesh;
    [SerializeField] GameObject playerUI;
    [SerializeField] GameObject ingameCanvas;

    void Start()
    {
        playermesh.SetActive(false);
        playerUI.SetActive(false);
        ingameCanvas.SetActive(false);
        player = GameObject.FindWithTag("Player");
        playerstatus = player.GetComponent<PlayerStatus>();
        StartCoroutine("Play3rdAreaCinematic");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasDescentTriggered)
        {
            Debug.Log("충돌!");
            playerstatus.Descent(); // HP 감소 및 세이브 포인트로 이동
            //hasDescentTriggered = true; // 한 번만 호출되도록 설정
        }
    }

    public void ResetDescentTrigger()
    {
        hasDescentTriggered = false;
    }

    IEnumerator Play3rdAreaCinematic()
    {
        playerInputs.isInteracting = true;

        yield return new WaitForSeconds(20f);
        if (ThirdAreaCinematic != null)
        {
            ThirdAreaCinematic.Play();
        }
        yield return new WaitForSeconds((float)ThirdAreaCinematic.duration);  //ThirdAreaCinematic.duration은 시네마틱의 전체 길이를 초 단위로 나타냄
        playerInputs.isInteracting = false;
        playermesh.SetActive(true);
        playerUI.SetActive(true);
        ingameCanvas.SetActive(true);
    }
}
