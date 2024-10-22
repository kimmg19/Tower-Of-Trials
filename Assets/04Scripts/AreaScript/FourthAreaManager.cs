using System.Collections;
using UnityEngine;

public class FourthAreaManager : MonoBehaviour
{
    [Header("Dragon Settings")]
    public GameObject dragons;
    public GameObject dragon;

    [Header("Meteor Gimmick Settings")]
    public GameObject meteorPrefab;    // 메테오 프리팹
    public int maxMeteors = 7;         // 메테오 최대 생성 개수
    public float meteorSpawnInterval = 1f; // 메테오 생성 간격 (초 단위)
    public Vector3 spawnAreaCenter = new Vector3(25f, 0f, 25f); // 중심 좌표
    public float spawnAreaRadius = 20f;  // 메테오가 떨어질 범위

    [Header("Player Settings")]
    public Transform player;           // 플레이어 위치 참조

    //"Other Settings"
    private float time;
    private bool isDrakarisFlyAttackActive = false;  // FlyAttack 중복 방지 플래그
    private bool isDrakarisAttackActive = false;     // Attack 중복 방지 플래그

    void Update()
    {
        time += Time.deltaTime;

        if (time > 5f && !isDrakarisAttackActive)
        {
            StartCoroutine(DrakarisAttack());
        }

        if (time > 8f && !isDrakarisFlyAttackActive)
        {
            StartCoroutine(DrakarisFlyAttack());
        }
    }

    private IEnumerator DrakarisAttack()
    {
        isDrakarisAttackActive = true; // 중복 실행 방지
        dragons.SetActive(true);
        yield return new WaitForSeconds(5f);
        dragons.SetActive(false);
        time = 0f;
        isDrakarisAttackActive = false; // 완료 후 다시 실행 가능
    }

    private IEnumerator DrakarisFlyAttack()
    {
        isDrakarisFlyAttackActive = true; // 중복 실행 방지

        // 드래곤 활성
        dragon.SetActive(true);

        // 메테오 생성 코루틴 실행 (메테오가 떨어지는 동안에만 드래곤이 활성화됨)
        yield return StartCoroutine(SpawnMeteors());

        // 메테오 생성 완료 후 드래곤 비활성화 및 시간 초기화
        dragon.SetActive(false);
        time = 0f;

        isDrakarisFlyAttackActive = false; // 완료 후 다시 실행 가능
    }

    // 메테오 생성 코루틴
    private IEnumerator SpawnMeteors()
    {
        int meteorCount = 0; // 현재 생성된 메테오 수

        while (meteorCount < maxMeteors)
        {
            // 1. 플레이어 위치에 메테오 생성
            Instantiate(meteorPrefab, player.position, Quaternion.identity);

            // 2. 랜덤 위치 생성
            Vector3 randomPosition = GetRandomPosition();

            // 3. 랜덤 위치에 메테오 생성
            Instantiate(meteorPrefab, randomPosition, Quaternion.identity);

            meteorCount++; // 플레이어 위치와 랜덤 위치에 각각 하나씩 생성하므로 두 개 증가

            // 메테오가 떨어진 후 대기 (간격 조정)
            yield return new WaitForSeconds(meteorSpawnInterval);
        }
    }

    // 랜덤 위치 생성 (맵 크기에 맞게 조정 가능)
    private Vector3 GetRandomPosition()
    {
        // 원형 범위 내 랜덤 좌표 계산
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // 360도 회전각 랜덤 선택
        float randomDistance = Random.Range(0f, spawnAreaRadius); // 반지름 범위 내 거리 랜덤 선택

        // 원형 좌표를 XZ 평면에 변환
        float x = Mathf.Cos(randomAngle) * randomDistance;
        float z = Mathf.Sin(randomAngle) * randomDistance;

        // 랜덤 좌표에 중심 좌표를 더해 최종 위치 계산
        Vector3 randomPosition = new Vector3(x, 0f, z) + spawnAreaCenter;

        return randomPosition;
    }
}
