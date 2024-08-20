using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class PatrolState : BaseState
{
    private float timer;
    private float chaseRange = 8f;
    private List<Transform> wayPoints = new List<Transform>();

protected override void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    GameObject[] waypointsArray = GameObject.FindGameObjectsWithTag("WayPoints");
    wayPoints.Clear(); // 초기화하여 이전의 웨이포인트를 제거합니다.
    foreach (GameObject go in waypointsArray)
    {
        wayPoints.Add(go.transform);
    }

    if (wayPoints.Count > 0)
    {
        Debug.Log($"Found {wayPoints.Count} waypoints.");
        agent.speed = 1.2f; // 속도 설정
        timer = 0;
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position); // 무작위 웨이포인트로 이동
    }
    else
    {
        Debug.LogWarning("No waypoints found for patrol.");
    }
}

protected override void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
{
    if (wayPoints.Count > 0 && agent.remainingDistance <= agent.stoppingDistance)
    {
        Debug.Log("Arrived at waypoint, selecting next waypoint.");
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position); // 다음 웨이포인트로 이동
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
        agent.SetDestination(agent.transform.position); // 이동 정지
    }
}
