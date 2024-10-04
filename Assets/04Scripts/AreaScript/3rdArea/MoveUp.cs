using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public int movingTime = 0;     // 발판이 방향을 바꾸는 주기 (sec)
    public int maxMoveCount = 3;   // 몇 번 이동할 것인지 설정
    private float delta = 0;       // deltaTime 값을 누적해서, 얼마의 시간이 흘렀는지를 판단
    private int moveCount = 0;     // 이동 횟수 카운트
    private bool movingFlag = true;    // 어느 방향으로 움직일지 결정
    public List<Vector3> directionList = new List<Vector3>();  // 이동 방향 리스트
    private int currentDirectionIndex = 0;  // 현재 방향을 가리키는 인덱스

    void Start()
    {
        
    }

    void Update()
    {
        // 이동 횟수가 maxMoveCount보다 크면 오브젝트를 제거
        if (moveCount >= maxMoveCount)
        {
            Destroy(gameObject);
            return;
        }

        this.delta += Time.deltaTime;

        if (this.delta >= this.movingTime)
        {
            this.delta = 0;
            this.movingFlag = !this.movingFlag;
            moveCount++; // 방향이 바뀔 때마다 이동 횟수 증가

            // 다음 방향으로 전환
            currentDirectionIndex = (currentDirectionIndex + 1) % directionList.Count;
        }

        float speedMultiplier = 4.0f;  // 속도를 조절할 계수
        Vector3 currentDirection = directionList[currentDirectionIndex];  // 현재 이동할 방향

        if (this.movingFlag)
        {
            this.transform.Translate(currentDirection * Time.deltaTime * speedMultiplier);
        }
        else
        {
            this.transform.Translate(-currentDirection * Time.deltaTime * speedMultiplier);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체의 태그가 "Player"인지 확인
        if (other.gameObject.tag == "Player")
        {
            // 원하는 동작 실행
            Debug.Log("Player와 충돌했음!");
            // 여기서 특정 동작을 추가하세요.
        }
    }
}
