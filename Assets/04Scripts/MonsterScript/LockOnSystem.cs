using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockOnSystem : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;  // Target Group
    public Transform target;  // 현재 Lock On 중인 타겟
    public bool isLockOn = false;  // Lock On 상태를 나타내는 플래그
    public float lockOnRadius = 10f;  // Lock On 범위
    public LayerMask lockOnLayerMask;  // Lock On 가능한 레이어
    Animator animator;
    private List<Transform> availableTargets;  // Lock On 가능한 모든 타겟 리스트
    private int currentTargetIndex = 0;  // 현재 타겟의 인덱스
    private Transform playerTransform;
    public RuntimeAnimatorController playerAnimator;
    public RuntimeAnimatorController playerAnimator_LockOn;

    void Start()
    {
        availableTargets = new List<Transform>();
        playerTransform = this.transform;  // 플레이어의 Transform 참조
        animator = GetComponent<Animator>();

        // 초기에는 플레이어만 Target Group에 추가
        targetGroup.AddMember(playerTransform, 1f, 0f);
    }

    void Update()
    {
        // 락온 중일 때만 타겟을 찾고 체크합니다.
        if (isLockOn)
        {
            // 현재 타겟이 유효하지 않으면 ResetLockOn 호출--lockonRadius거리 이상 넘어가면 자동으로 풀림 
            if (target == null || !target.gameObject.activeInHierarchy 
                || Vector3.Distance(playerTransform.position, target.position) > lockOnRadius)
            {
                ResetLockOn();
            } else
            {
                LockOnToTarget();  // 유효하면 타겟 유지
            }
        }
    }

    void LockOnToTarget()
    {
        // 플레이어가 타겟을 향하도록 회전
        Vector3 directionToTarget = target.position - playerTransform.position;
        directionToTarget.y = 0;  // 수직 축의 변화는 무시
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * 5.0f);

        // Target Group에 타겟을 추가 (중복 추가 방지)
        if (targetGroup.FindMember(target) < 0)
        {
            targetGroup.AddMember(target, 1f, 0f);
        }
    }

    public void ToggleLockOn()
    {
        FindTargets();  // 타겟 리스트를 업데이트합니다.

        if (availableTargets.Count > 0)
        {
            if (isLockOn)
            {
                ResetLockOn();
                return;
            }
            isLockOn = true;

            currentTargetIndex = 0;  // 첫 번째 타겟으로 설정
            target = availableTargets[currentTargetIndex];
            LockOnToTarget();
            ChangeAnimatorController(playerAnimator_LockOn);

        } else
        {
            ResetLockOn();
        }

        Debug.Log($"LockOn state: {isLockOn}");
    }

    void ResetLockOn()
    {
        // Lock On 해제 시 타겟과 리스트 초기화
        target = null;
        availableTargets.Clear();

        // Target Group에서 타겟 제거 (플레이어는 유지)
        for (int i = targetGroup.m_Targets.Length - 1; i >= 0; i--)
        {
            if (targetGroup.m_Targets[i].target != playerTransform)
            {
                targetGroup.RemoveMember(targetGroup.m_Targets[i].target);
            }
        }

        ChangeAnimatorController(playerAnimator);
        isLockOn = false;
    }

    void ChangeAnimatorController(RuntimeAnimatorController newController)
    {
        animator.runtimeAnimatorController = newController;
    }

    void FindTargets()
    {
        // 플레이어 주변에 있는 모든 잠금 가능한 타겟 검색
        Collider[] targets = Physics.OverlapSphere(playerTransform.position, lockOnRadius, lockOnLayerMask);

        // 타겟 리스트 초기화
        availableTargets.Clear();

        foreach (Collider potentialTarget in targets)
        {
            availableTargets.Add(potentialTarget.transform);
        }

        if (availableTargets.Count > 0)
        {
            // 현재 타겟을 가장 가까운 타겟으로 설정
            availableTargets.Sort((a, b) => Vector3.Distance(playerTransform.position, a.position)
                .CompareTo(Vector3.Distance(playerTransform.position, b.position)));
        }
    }

    public void SwitchTarget()
    {
        if (!isLockOn || availableTargets.Count <= 1) return;

        // 현재 타겟을 Target Group에서 제거
        targetGroup.RemoveMember(target);

        // 다음 타겟으로 순환
        currentTargetIndex = (currentTargetIndex + 1) % availableTargets.Count;
        target = availableTargets[currentTargetIndex];

        // 새로운 타겟을 Target Group에 추가
        LockOnToTarget();

        Debug.Log($"Switched target to: {target.name}");
    }
}
