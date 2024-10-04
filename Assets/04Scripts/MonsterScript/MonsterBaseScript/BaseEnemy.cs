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
        DropItem();
        Invoke("DestroyEnemy", 2f);
    }

    protected virtual void DestroyEnemy()
    {
        gameObject.SetActive(false);
    }
    float delayTime = 0.01f;
    protected virtual void OnTriggerEnter(Collider other)
    {
        // 방패 충돌 먼저 처리
        if (other.CompareTag("Shield"))
        {
            ProcessTrigger(other.gameObject);
        }
        
        else if (other.CompareTag("Player") && playerStatus.playerAlive)
        {            
                StartCoroutine(DelayedTrigger(other.gameObject, delayTime));
        }
    }

    IEnumerator DelayedTrigger(GameObject obj, float delay)
    {
        
        yield return new WaitForSeconds(delay);
        //Debug.LogError("3");

        // 방패와의 충돌 여부를 다시 확인
        if (isShieldTriggered)
        {
            Debug.LogError("Shield was triggered. Ignoring Player collision.");
            TriggerReset();
            yield break;  // 코루틴 중지
        }

        // 무적 상태를 체크하여 데미지를 무시
        if (playerStatus.isDamageIgnored)
        {
            Debug.Log($"{gameObject.name}'s attack was ignored due to player invincibility.");
        }

        if (enableDamaging && !playerStatus.isParried)
        {
            if (!playerInputs.isAttacking)
            {
                Debug.Log($"{gameObject.name} is attempting to damage the player with enableDamaging: {enableDamaging}");
                playerStatus.TakeDamage(damageAmount);
                enableDamaging = false;
            }
        } else
        {
            Debug.Log($"{gameObject.name}'s attack was parried or enemy is in slow motion. No damage to player.");
            enableDamaging = false;
        }
        //Debug.LogError("4");
    }

    public bool isShieldTriggered = false;

    void ProcessTrigger(GameObject obj)
    {
        isShieldTriggered = true; // 방패와 충돌했음을 표시
        Shield shield = obj.GetComponent<Shield>();
        //Debug.LogError("1");

        if (shield.isParryWindowActive && shield.canParry)
        {
            shield.HandleParrySuccess(this);
        } else if (shield.isBlocking)
        {
            int reducedDamage = Mathf.RoundToInt(GetDamageAmount() * (1 - shield.damageReductionPercentage / 100));
            playerStatus.TakeDamage(reducedDamage);
            Debug.Log($"Blocked! Damage reduced to {reducedDamage}.");
        } else
        {
            playerStatus.TakeDamage(GetDamageAmount());
        }
        //Debug.LogError("2");
        
    }

    // 아이템 드랍 메서드 (상속받는 클래스에서 구현할 수 있음)
    protected virtual void DropItem()
    {
        // 기본적으로 아무것도 드랍하지 않음
        // 상속받는 클래스에서 오버라이딩하여 특정 아이템을 드랍하도록 구현
    }

    void TriggerReset()
    {
        //Debug.LogError("555");
        StopCoroutine("DelayedTrigger");
        isShieldTriggered = false;
    }


}
