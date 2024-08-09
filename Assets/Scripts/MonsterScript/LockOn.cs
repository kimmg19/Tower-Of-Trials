// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class LockOn : MonoBehaviour
// {
//     PlayerInputs playerInputs;
//     [SerializeField] float lockOnRadius = 10;
//     [SerializeField] LayerMask targetLayer;
//     [SerializeField] GameObject mainCamera;
//     [SerializeField] float minViewAngle = -70;
//     [SerializeField] float maxViewAngle = 70;
//     [SerializeField] List<Enemy> targetEnemy = new List<Enemy>();
//     Enemy currentTarget;
//     Vector3 currentTargetPosition;
//     bool lockOn = false;
//     [SerializeField] float lookAtSmoothing = 5;

//     void Start()
//     {
//         playerInputs = GetComponent<PlayerInputs>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(lockOn)
//         {
//             if(isTargetRange()){
//                 LookAtTarget();
//             }
//             else{
//                 ResetTarget();
//             }
//         }
//         // Q 키가 눌려서 isLockOn이 true로 바뀌었을 때 FindLockOnTarget 호출
//         if (playerInputs.isLockOn)
//         {
//             FindLockOnTarget();
//             playerInputs.isLockOn = false;  // 타겟을 찾고 나서 isLockOn을 false로 다시 설정
//         }
//     }

//     private void FindLockOnTarget(){
//         Collider[] findTarget = Physics.OverlapSphere(transform.position,lockOnRadius, targetLayer);

//         if(findTarget.Length <= 0){
//             ResetTarget();
//             return;
//         }
//         bool targetAdded = false; // 적이 추가되었는지 확인할 플래그

//         for (int i = 0; i < findTarget.Length; i++){
//             Enemy target = findTarget[i].GetComponent<Enemy>();

//             if(target != null){
//                 Vector3 targetDirection = target.transform.position - transform.position;

//                 float viewAngle = Vector3.Angle(targetDirection, mainCamera.transform.forward);

//                 if(viewAngle > minViewAngle && viewAngle < maxViewAngle){
//                     RaycastHit hit;

//                     if(Physics.Linecast(transform.position
//                                         ,target.lockOnTarget.transform.position
//                                         , out hit, targetLayer))
//                     {
//                         targetAdded = true;
//                         targetEnemy.Add(target);
//                         Debug.Log("Adding target: " + target.name);
//                     }
//                 }
//             }
//         }
//         if(targetAdded){
//             LockOnTarget();
//         }
//     }

//     private void LockOnTarget()
//     {
//         if (targetEnemy.Count == 0)
//         {
//             Debug.Log("No targets available!");
//             return;
//         }
//         float shortDistance = Mathf.Infinity;

//         for (int i = 0; i < targetEnemy.Count; i++){
//             if (targetEnemy[i] != null){
//                 float distanceFromTarget = Vector3.Distance(transform.position,
//                                                             targetEnemy[i].transform.position);

//                 if(distanceFromTarget < shortDistance){
//                     shortDistance = distanceFromTarget;
//                     currentTarget = targetEnemy[i];
//                 }
//             }
//             else
//             {
//                 ResetTarget();
//             }
//         }
//         if(currentTarget != null)
//         {
//             lockOn = true;
//             FindTarget();
//         }
//     }

//     private void LookAtTarget()
//     {
//         if(currentTarget == null)
//         {
//             ResetTarget();
//             return;
//         }
//         currentTargetPosition = currentTarget.lockOnTarget.transform.position;

//         Vector3 dir = (currentTargetPosition - transform.position).normalized;
//         dir.y = transform.position.y;

//         transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * lookAtSmoothing);
//     }

//     private void FindTarget()
//     {
//         lockOn = true;
//     }
//     private void ResetTarget()
//     {
//         lockOn = false;
//         targetEnemy.Clear();
//     }

//     private bool isTargetRange()
//     {
//         float distance = (transform.position - currentTargetPosition).magnitude;
//         return distance <= lockOnRadius;  // lockOnRadius 이내에 있을 때 true
//     }
//     private void OnDrawGizmos()
//     {
//         Gizmos.DrawWireSphere(transform.position, lockOnRadius);
//     }
// }
