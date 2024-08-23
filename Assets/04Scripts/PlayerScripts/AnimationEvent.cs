using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerInputs playerInputs;
    [SerializeField] TrailRenderer trailRenderer;
    [HideInInspector] public bool enableDamaging;
    Animator animator;

    // Sword 스크립트 참조 변수 추가
    private Sword sword;

    void Start()
    {
        playerInputs = GetComponent<PlayerInputs>();
        playerMovement = GetComponent<PlayerMovement>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        trailRenderer.gameObject.SetActive(false);
        animator = GetComponent<Animator>();

        // Sword 스크립트 참조 설정
        sword = GetComponentInChildren<Sword>(); // 플레이어 하위에 있는 Sword 오브젝트를 찾아 참조
        if (sword == null)
        {
            Debug.LogError("Sword script not found in child objects.");
        }
    }

    // 애니메이션 이벤트로 호출될 메서드
    void DamageAble()
    {
        enableDamaging = true;
        sword?.EnableCollider(); // Sword의 콜라이더 활성화
    }

    // 애니메이션 이벤트로 호출될 메서드
    void DamageDisable()
    {
        enableDamaging = false;
        sword?.DisableCollider(); // Sword의 콜라이더 비활성화
    }

    void AttackEffectOn()
    {
        trailRenderer.gameObject.SetActive(true);
        if (playerInputs.isDodging)  
            AtttackEffectOff();
    }

    public void AtttackEffectOff()
    {
        trailRenderer.gameObject.SetActive(false);
    }

    public void OnFinishAttack()
    {
        playerInputs.isAttacking = false;
    }

    void EndDodge()
    {
        playerMovement.characterController.center = new Vector3(0, 0.88f, 0);
        playerMovement.characterController.height = 1.6f;
        playerInputs.isDodging = false;
    }

    public bool IsAttacking()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || playerInputs.isAttacking);
    }
}
