using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public GameObject particleEffect; // 파티클 이펙트를 참조할 변수

    void OnTriggerEnter(Collider other)
    {
        // 플레이어가 키 아이템에 닿았을 때 실행
        if (other.CompareTag("Player"))
        {
            AudioManager.instance.Play("PickUpItem");
            // 파티클 효과가 있으면 삭제
            if (particleEffect != null)
            {
                Destroy(particleEffect);
            }
        }
    }
}
