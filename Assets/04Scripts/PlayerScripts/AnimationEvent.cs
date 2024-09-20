using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerInputs playerInputs;
    [HideInInspector] public bool enableDamaging;
    Animator animator;
    [SerializeField] ParticleSystem attack1;
    [SerializeField] ParticleSystem attack2;
    [SerializeField] ParticleSystem attack3_1;
    [SerializeField] ParticleSystem attack3_2;
    [SerializeField] ParticleSystem attack3_3;
    SkillAttack skillAttack;
    
    [SerializeField] ParticleSystem skillEffect;
    // Sword 스크립트 참조 변수 추가
    private Sword sword;

    void Start()
    {
        skillAttack = GetComponent<SkillAttack>();
        playerInputs = GetComponent<PlayerInputs>();
        playerMovement = GetComponent<PlayerMovement>();        
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
    void SkillEffectPlay()
    {
        skillEffect.Play();
        skillAttack.Skill();
    }
    void Attack01Effect()
    {
        attack1.Play();
        AudioManager.instance.Play("Attack1SFX"); // 1타 공격 효과음 재생
    }
    void Attack02Effect() { 
        attack2.Play();
        AudioManager.instance.Play("Attack2SFX"); // 2타 공격 효과음 재생
    }
    void Attack03Effect1()
    {
        attack3_1.Play();
        AudioManager.instance.Play("Attack3_1SFX"); // 3타 공격 효과음 재생

        
    }
    void Attack03Effect2()
    {
        attack3_2.Play();
        AudioManager.instance.Play("Attack3_2SFX"); // 3타 공격 효과음 재생

    }
    void Attack03Effect3()
    {
        attack3_3.Play();
        AudioManager.instance.Play("Attack3Voice"); // 3타 공격 효과음 재생
    }   

    

    public void OnFinishAttack()
    {
        playerInputs.isAttacking = false;
    }
    void FinishSkillAttack()
    {
        playerInputs.isSkillAttacking=false;
    }
    void EndDodge()
    {
        playerMovement.characterController.center = new Vector3(0, 0.88f, 0);
        playerMovement.characterController.height = 1.6f;
        playerInputs.isDodging = false;        
    }
    void EndJump()
    {        
        playerInputs.isJumping = false;
    }

    public bool IsAttacking()
    {
        return (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2") ||
                    animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3") || playerInputs.isAttacking);
    }
}
