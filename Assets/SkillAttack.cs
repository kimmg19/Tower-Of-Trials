using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CastInfo
{
    public Vector3 Point;
    public float Distance;
    public float Angle;
}
public class SkillAttack : MonoBehaviour
{
    [Header("Circle")]
    [Range(0, 30)]
    [SerializeField] float viewRange;               // 시야 범위
    [Range(0, 360)]
    [SerializeField] float viewAngle;               // 시야 각도

    [Header("Target")]
    [SerializeField] LayerMask targetMask;          // 탐색 대상
    [SerializeField] List<Transform> targetList;    // 탐색 결과 리스트

    /*[Header("Draw Line")]
    [Range(0.1f, 1f)]
    [SerializeField] float angle;                   // 선이 표시될 각도. 작을 수록 선이 촘촘해진다.
    [SerializeField] List<CastInfo> lineList;       // 표시된 선의 정보 리스트
    [SerializeField] Vector3 offset;                // 위치 보정용 벡터. zero 로 해도 무관*/

    void Start()
    {
        targetList = new();
        //lineList = new();
        //StartCoroutine(DrawRayLine());      스킬 공격범위 보여주는 거임
    }
    public void Skill()
    {
        StartCoroutine(CheckTarget());
        
    }
    IEnumerator CheckTarget()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.1f);

        
            targetList.Clear();
            // 원형 범위 내 대상을 검출한다.
            Collider[] cols = Physics.OverlapSphere(transform.position, viewRange, targetMask);

            foreach (var col in cols)
            {
                // 검출한 대상의 방향을 구한다.
                Vector3 direction = (col.transform.position - transform.position).normalized;
                direction.y = 0f;
                print("target in range");

                // 대상과의 각도가 설정한 각도 이내에 있는지 확인한다.
                // viewAngle 은 부채꼴 전체 각도이기 때문에, 0.5를 곱해준다.
                if (Vector3.Angle(transform.forward, direction) < (viewAngle * 0.5f))
                {
                    print("target in angle");
                    col.GetComponent<BaseEnemy>().TakeDamage(20);
                }
            }
            yield return null;
        
    }
    CastInfo GetCastInfo(float _angle)
    {
        // 입력받은 각도에 따라 방향을 결정한다.
        Vector3 dir = new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
        CastInfo Info;
        Info.Angle = _angle;            // 각도
        Info.Distance = viewRange;      // 최대 도달 거리인 Range                                        
        Info.Point = transform.position + dir * viewRange; //위치 보정하고 싶으면offset 더하면 됨

        return Info;
    }
    /*IEnumerator DrawRayLine()
    {
        while (true)
        {
            lineList.Clear();       // 이미 생성된 레이캐스트 정보는 삭제한다.

            // 선이 표시될 갯수. 시야각에서 선이 표시될 각도를 나눠서 구한다.
            int count = Mathf.RoundToInt(viewAngle / angle) + 1;
            // 가장 오른쪽 각도. 시야각과 플레이어의 방향을 기준으로 결정된다.
            float fAngle = -(viewAngle * 0.5f) + transform.eulerAngles.y;

            // 선이 표시될 갯수만큼 실행한다.
            for (int i = 0; i < count; ++i)
            {
                // 해당 각도로 발사한 레이캐스트 정보를 가져온다.
                CastInfo info = GetCastInfo(fAngle + (angle * i));
                lineList.Add(info);

                // 해당 레이캐스트 정보에 따라 선을 그린다.
                Debug.DrawLine(transform.position + offset, info.Point, Color.green);
            }

            yield return null;
        }
    }*/
}







