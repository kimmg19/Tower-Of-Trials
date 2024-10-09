using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class PatrolState : BaseState
{
    private float timer;
    private float chaseRange = 8f;
    private List<Transform> wayPoints = new List<Transform>();
    private static List<Transform> occupiedWayPoints = new List<Transform>(); // 이미 사용 중인 웨이포인트 추적
    private float wanderRadius = 5f; // 랜덤 이동 반경

    protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject[] waypointsArray = GameObject.FindGameObjectsWithTag("WayPoints");
        wayPoints.Clear();
        foreach (GameObject go in waypointsArray)
        {
            wayPoints.Add(go.transform);
        }

        if (wayPoints.Count > 0)
        {
            agent.speed = 1.2f;
            timer = 0;

            // 가까운 웨이포인트 중 사용 중이 아닌 웨이포인트로 이동
            Transform targetWaypoint = GetNearestAvailableWaypoint();
            if (targetWaypoint != null)
            {
                agent.SetDestination(targetWaypoint.position);
                occupiedWayPoints.Add(targetWaypoint); // 해당 웨이포인트를 사용 중으로 표시
            }
            else
            {
                // 모든 웨이포인트가 사용 중일 때 랜덤한 방향으로 이동
                WanderRandomly();
            }
        }
    }

    protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wayPoints.Count > 0 && agent.remainingDistance <= agent.stoppingDistance)
        {
            // 다음 가까운 웨이포인트 중 사용 중이 아닌 웨이포인트로 이동
            Transform targetWaypoint = GetNearestAvailableWaypoint();
            if (targetWaypoint != null)
            {
                agent.SetDestination(targetWaypoint.position);
                occupiedWayPoints.Add(targetWaypoint); // 새로운 웨이포인트 사용 중으로 표시
            }
            else
            {
                // 모든 웨이포인트가 사용 중일 때 랜덤한 방향으로 이동
                WanderRandomly();
            }
        }

        timer += Time.deltaTime;

        if (timer > 10f)
        {
            animator.SetBool("isPatrolling", false);
        }

        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange && playerStatus.playerAlive)
        {
            animator.SetBool("isChasing", true);
        }
    }

    protected override void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 해당 웨이포인트를 더 이상 사용하지 않도록 목록에서 제거
        Transform currentWaypoint = GetNearestWaypoint();
        if (occupiedWayPoints.Contains(currentWaypoint))
        {
            occupiedWayPoints.Remove(currentWaypoint);
        }
        agent.SetDestination(agent.transform.position); // 이동 정지
    }

    // 가장 가까우면서 사용 중이 아닌 웨이포인트 찾기
    private Transform GetNearestAvailableWaypoint()
    {
        Transform nearestWaypoint = null;
        float minDistance = float.MaxValue;

        foreach (Transform waypoint in wayPoints)
        {
            if (!occupiedWayPoints.Contains(waypoint)) // 사용 중이지 않은 웨이포인트만 선택
            {
                float distance = Vector3.Distance(agent.transform.position, waypoint.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestWaypoint = waypoint;
                }
            }
        }

        return nearestWaypoint;
    }

    // 모든 웨이포인트들 중 가장 가까운 웨이포인트 찾기
    private Transform GetNearestWaypoint()
    {
        Transform nearestWaypoint = wayPoints[0];
        float minDistance = Vector3.Distance(agent.transform.position, nearestWaypoint.position);

        foreach (Transform waypoint in wayPoints)
        {
            float distance = Vector3.Distance(agent.transform.position, waypoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWaypoint = waypoint;
            }
        }

        return nearestWaypoint;
    }

    // 랜덤한 위치로 이동하는 메서드
    private void WanderRandomly()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius; // 반경 내 무작위 방향
        randomDirection += agent.transform.position;

        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(navHit.position); // 무작위로 찾은 위치로 이동
            Debug.Log("모든 웨이포인트가 사용 중이므로 랜덤하게 이동합니다.");
        }
    }
}
