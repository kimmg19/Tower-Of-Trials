using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public int movingTime = 0;     // ������ ������ �ٲٴ� �ֱ� (sec)
    public int maxMoveCount = 3;   // �� �� �̵��� ������ ����
    private float delta = 0;       // deltaTime ���� �����ؼ�, ���� �ð��� �귶������ �Ǵ�
    private int moveCount = 0;     // �̵� Ƚ�� ī��Ʈ
    private bool movingFlag = true;    // ��� �������� �������� ����
    public List<Vector3> directionList = new List<Vector3>();  // �̵� ���� ����Ʈ
    private int currentDirectionIndex = 0;  // ���� ������ ����Ű�� �ε���

    void Start()
    {
        
    }

    void Update()
    {
        // �̵� Ƚ���� maxMoveCount���� ũ�� ������Ʈ�� ����
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
            moveCount++; // ������ �ٲ� ������ �̵� Ƚ�� ����

            // ���� �������� ��ȯ
            currentDirectionIndex = (currentDirectionIndex + 1) % directionList.Count;
        }

        float speedMultiplier = 4.0f;  // �ӵ��� ������ ���
        Vector3 currentDirection = directionList[currentDirectionIndex];  // ���� �̵��� ����

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
        // �浹�� ��ü�� �±װ� "Player"���� Ȯ��
        if (other.gameObject.tag == "Player")
        {
            // ���ϴ� ���� ����
            Debug.Log("Player�� �浹����!");
            // ���⼭ Ư�� ������ �߰��ϼ���.
        }
    }
}
