using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class BaseEnemy : MonoBehaviour
{
    protected int HP;
    protected int damageAmount;
    public Slider healthBar;
    public Animator animator;
    public bool enableDamaging = false;
    public bool isParried = false; // 패링 상태를 추적하는 변수

    protected PlayerStats playerStats;
    protected PlayerStatus playerStatus;
    protected PlayerInputs playerInputs;

    public bool isAttacking = false; // 공격 상태를 추적하는 변수

    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            playerStatus = player.GetComponent<PlayerStatus>();
            playerInputs = player.GetComponent<PlayerInputs>();
        }

        InitializeStats(); // 하위 클래스에서 고유의 값 설정

        // 슬라임의 자식 오브젝트에서 슬라이더 컴포넌트를 찾아 할당
        healthBar = GetComponentInChildren<Slider>();

        if (healthBar == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing a health bar!");
        }
        else if (!healthBar.gameObject.activeInHierarchy)
        {
            healthBar.gameObject.SetActive(true); // 헬스바 활성화
        }
    }

    protected virtual void Update()
    {
        // 애니메이터 상태 디버깅 로그 추가
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("AttackState"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                enableDamaging = true; // 공격 애니메이션이 시작될 때 enableDamaging을 켭니다.
            }
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                enableDamaging = false; // 공격 애니메이션이 끝나면 enableDamaging을 끕니다.
            }
        }

        // 헬스바를 개별적으로 업데이트
        if (healthBar != null)
        {
            healthBar.value = HP;
        }
    }

    protected abstract void InitializeStats(); // 파생 클래스에서 값을 초기화하도록 강제

    public virtual void TakeDamage(int damageAmount, bool parried = false)
    {
        HP -= damageAmount;
        AudioManager.instance.Play("MonsterHit");
        isParried = parried; // 패링 상태 추적
        Debug.Log($"{gameObject.name} took {damageAmount} damage. Current HP: {HP}, Parried: {isParried}");

        // 헬스바 업데이트
        if (healthBar != null)
        {
            healthBar.value = (float)HP / healthBar.maxValue;
        }

        if (HP <= 0)
        {
            Die();
        }
        else if (isParried)
        {
            // 패링 성공 시에만 슬로우 모션 애니메이션 재생
            StartCoroutine(PlaySlowHitAnimation());
        }
    }

    public int GetDamageAmount()
    {
        Debug.Log($"Damage amount is {damageAmount} for {gameObject.name}");
        return damageAmount;
    }

    private IEnumerator PlaySlowHitAnimation()
    {
        animator.Play("getHit", -1, 0f);

        // 애니메이션 속도를 느리게 설정
        float originalSpeed = animator.speed;
        animator.speed = 0.5f; // 2배 느리게

        // 적의 모든 콜라이더 중 공격 전용 콜라이더를 비활성화
        Collider[] colliders = GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            // 공격용 콜라이더는 레이어를 통해 식별
            if (collider.gameObject.layer == LayerMask.NameToLayer("EnemyAttackCollider"))
            {
                collider.enabled = false;
                Debug.Log($"{collider.gameObject.name} attack collider disabled during slow motion.");
            }
        }

        // 느린 히트 애니메이션이 진행되는 동안 대기 (실제 시간 기준)
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length / animator.speed);

        // 원래 속도로 복구
        animator.speed = originalSpeed;

        // 패링 후 적의 공격 콜라이더를 다시 활성화
        foreach (var collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("EnemyAttackCollider"))
            {
                collider.enabled = true;
                Debug.Log($"{collider.gameObject.name} attack collider re-enabled after slow motion.");
            }
        }

        // 패링 후 다시 공격할 수 있도록 설정
        enableDamaging = true;
        isParried = false; // 패링 상태 해제
    }

    protected virtual void Die()
    {
        animator.SetTrigger("die");
        AudioManager.instance.Play("MonsterDie");
        GetComponent<Collider>().enabled = false;
        Invoke("DestroyEnemy", 3f);
    }

    protected virtual void DestroyEnemy()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerStatus.playerAlive)
        {
            // 무적 상태를 체크하여 데미지를 무시
            if (playerStatus.isDamageIgnored) 
            {
                Debug.Log($"{gameObject.name}'s attack was ignored due to player invincibility.");
                return;
            }

            if (enableDamaging && !playerStatus.isParried)
            {
                // 플레이어가 공격할 때만 데미지를 입힐 수 있도록 조건을 추가
                if (!playerInputs.isAttacking)
                {
                    Debug.Log($"{gameObject.name} is attempting to damage the player with enableDamaging: {enableDamaging}");
                    playerStatus.TakeDamage(damageAmount);
                    enableDamaging = false; // 공격 발생 후 바로 공격 가능 상태를 끔
                }
            }
            else
            {
                Debug.Log($"{gameObject.name}'s attack was parried or enemy is in slow motion. No damage to player.");
                enableDamaging = false; // 패링 성공 후에도 공격 가능 상태를 끔
            }
        }
    }
}
