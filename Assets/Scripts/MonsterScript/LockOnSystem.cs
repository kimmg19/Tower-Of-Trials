using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockOnSystem : MonoBehaviour
{
    public CinemachineFreeLook lockonCamera;  // FreeLook 카메라
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
        
        // 초기에는 LockonCamera를 비활성화
        if (lockonCamera != null)
        {
            lockonCamera.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isLockOn && target != null)
        {
            LockOnToTarget();
        }
    }

    void LockOnToTarget()
    {
        // 플레이어가 타겟을 향하도록 회전
        Vector3 directionToTarget = target.position - playerTransform.position;
        directionToTarget.y = 0;  // 수직 축의 변화는 무시
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * 5.0f);

        // FreeLook 카메라 설정: 타겟을 바라보도록 설정
        lockonCamera.LookAt = target;
        lockonCamera.Follow = playerTransform;
    }

    public void ToggleLockOn()
    {
        isLockOn = !isLockOn;

        if (isLockOn)
        {
            // Lock On 상태라면 타겟을 찾습니다.
            FindTargets();
            if (availableTargets.Count > 0)
            {
                currentTargetIndex = 0;  // 가장 첫 번째 타겟으로 설정
                target = availableTargets[currentTargetIndex];
                LockOnToTarget();  // 타겟을 설정한 후 카메라 설정

                // LockonCamera 활성화
                lockonCamera.gameObject.SetActive(true);
                ChangeAnimatorController(playerAnimator_LockOn);
            }
            else
            {
                // 타겟이 없을 경우 Lock On 해제
                isLockOn = false;
                lockonCamera.gameObject.SetActive(false);
                ChangeAnimatorController(playerAnimator);

            }
        }
        else
        {
            // Lock On 해제 시 타겟을 null로 설정
            target = null;
            availableTargets.Clear();

            // LockonCamera 비활성화
            lockonCamera.gameObject.SetActive(false);
            ChangeAnimatorController(playerAnimator);

        }

        Debug.Log($"LockOn state: {isLockOn}");
    }

    void ChangeAnimatorController(RuntimeAnimatorController newController)//락온 on/off때마다 애니메이터 바꾸기
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

        // 다음 타겟으로 순환
        currentTargetIndex = (currentTargetIndex + 1) % availableTargets.Count;
        target = availableTargets[currentTargetIndex];
        LockOnToTarget();  // 타겟을 변경한 후 카메라 설정
    }
}
