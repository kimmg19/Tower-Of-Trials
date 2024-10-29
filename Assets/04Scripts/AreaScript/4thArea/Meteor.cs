using UnityEngine;
using UnityEngine.VFX;

public class Meteor : MonoBehaviour
{
    public Transform player;  // 플레이어 Transform
    public float hitRadius = 5f;  // 피격 반경
    public VisualEffect meteorEffect; // 메테오 VFX 이펙트

    private bool hasHitGround = false;  // 메테오가 지면에 닿았는지 여부를 추적

    private void Update()
    {
        // 메테오 이펙트가 땅에 도달했을 때 충돌 처리
        if (!hasHitGround && CheckEffectHitGround())  // 이펙트가 땅에 닿았는지 확인
        {
            hasHitGround = true;

            // 메테오의 실제 위치를 이펙트 위치로 설정 (이펙트가 끝난 위치)
            transform.position = meteorEffect.transform.position;

            // 플레이어와의 거리 계산
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= hitRadius)
            {
                Debug.Log("플레이어가 메테오에 맞았습니다!");
                player.GetComponent<PlayerStatus>().TakeDamage(10);  // 플레이어에게 데미지
            }

            Destroy(gameObject);  // 메테오 오브젝트 파괴
        }
    }

    // 이펙트가 땅에 닿았는지 감지하는 메서드
    private bool CheckEffectHitGround()
    {
        // VFX 이펙트의 aliveParticleCount가 0이 되면 땅에 닿았다고 간주
        return meteorEffect.aliveParticleCount == 0;
    }
}
