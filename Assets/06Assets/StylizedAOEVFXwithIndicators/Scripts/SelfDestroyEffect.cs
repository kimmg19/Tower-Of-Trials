using UnityEngine;
using UnityEngine.VFX;

public class SelfDestroyEffect : MonoBehaviour
{
    private VisualEffect effect;
    private bool effectPlayed = false;
    public Transform player;  // 플레이어 Transform
    public Collider meteorCollider; // 메테오의 콜라이더

    void Start()
    {
        effect = gameObject.GetComponent<VisualEffect>();
        meteorCollider = gameObject.GetComponent<Collider>(); // 메테오의 콜라이더 가져오기
        effect.Play();

        // 플레이어 참조 디버깅
        if (player != null)
        {
            Debug.Log("플레이어가 성공적으로 할당되었습니다.");
        }
        else
        {
            Debug.LogWarning("플레이어가 할당되지 않았습니다!");
        }

        // 초기에는 콜라이더 비활성화
        meteorCollider.enabled = false;
    }

    void Update()
    {
        // 이펙트가 처음 플레이되고 있으면 충돌 확인
        if (effect.aliveParticleCount > 0 && !effectPlayed)
        {
            effectPlayed = true;
            Debug.Log("이펙트가 처음 재생되었습니다.");
            Invoke("ActivateCollider", 1.3f);  // 1.3초 후에 콜라이더 활성화
        }

        // 이펙트가 끝났으면 오브젝트 파괴
        if (effect.aliveParticleCount == 0 && effectPlayed)
        {
            Debug.Log("이펙트가 종료되었습니다. 메테오를 파괴합니다.");
            Destroy(gameObject);
        }
    }

    // 1.3초 후에 콜라이더를 활성화하는 메서드
    private void ActivateCollider()
    {
        meteorCollider.enabled = true;
        Debug.Log("메테오의 콜라이더가 활성화되었습니다.");
    }

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어와 충돌 시 데미지 처리
        if (other.CompareTag("Player")) // 플레이어 태그를 확인
        {
            Debug.Log("플레이어가 메테오에 맞았습니다!");
            other.GetComponent<PlayerStatus>().TakeDamage(10);  // 플레이어에게 데미지 입힘
            meteorCollider.enabled = false; // 한번 맞았을 때 콜라이더 비활성화
        }
    }
}
