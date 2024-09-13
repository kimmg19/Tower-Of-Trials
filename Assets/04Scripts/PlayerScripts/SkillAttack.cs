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
    [SerializeField] float viewRange;               // �þ� ����
    [Range(0, 360)]
    [SerializeField] float viewAngle;               // �þ� ����

    [Header("Target")]
    [SerializeField] LayerMask targetMask;          // Ž�� ���
    [SerializeField] List<Transform> targetList;    // Ž�� ��� ����Ʈ

    /*[Header("Draw Line")]
    [Range(0.1f, 1f)]
    [SerializeField] float angle;                   // ���� ǥ�õ� ����. ���� ���� ���� ����������.
    [SerializeField] List<CastInfo> lineList;       // ǥ�õ� ���� ���� ����Ʈ
    [SerializeField] Vector3 offset;                // ��ġ ������ ����. zero �� �ص� ����*/

    void Start()
    {
        targetList = new();
        //lineList = new();
        //StartCoroutine(DrawRayLine());      ��ų ���ݹ��� �����ִ� ����
    }
    public void Skill()
    {
        StartCoroutine(CheckTarget());
        
    }
    IEnumerator CheckTarget()
    {
        
        WaitForSeconds wfs = new WaitForSeconds(0.1f);

        
            targetList.Clear();
            // ���� ���� �� ����� �����Ѵ�.
            Collider[] cols = Physics.OverlapSphere(transform.position, viewRange, targetMask);

            foreach (var col in cols)
            {
                Vector3 direction = (col.transform.position - transform.position).normalized;
                direction.y = 0f;

                if (Vector3.Angle(transform.forward, direction) < (viewAngle * 0.5f))
                {
                    BaseEnemy enemy = col.GetComponentInParent<BaseEnemy>();
                    if (enemy != null)
                    {
                        print("target in angle");
                        enemy.TakeDamage(20, false);
                    }
                    else
                    {
                        Debug.LogWarning($"{col.gameObject.name} does not have a BaseEnemy component.");
                    }
                }
            }
            yield return null;
        
    }
    CastInfo GetCastInfo(float _angle)
    {
        // �Է¹��� ������ ���� ������ �����Ѵ�.
        Vector3 dir = new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
        CastInfo Info;
        Info.Angle = _angle;            // ����
        Info.Distance = viewRange;      // �ִ� ���� �Ÿ��� Range                                        
        Info.Point = transform.position + dir * viewRange; //��ġ �����ϰ� ������offset ���ϸ� ��

        return Info;
    }
    /*IEnumerator DrawRayLine()
    {
        while (true)
        {
            lineList.Clear();       // �̹� ������ ����ĳ��Ʈ ������ �����Ѵ�.

            // ���� ǥ�õ� ����. �þ߰����� ���� ǥ�õ� ������ ������ ���Ѵ�.
            int count = Mathf.RoundToInt(viewAngle / angle) + 1;
            // ���� ������ ����. �þ߰��� �÷��̾��� ������ �������� �����ȴ�.
            float fAngle = -(viewAngle * 0.5f) + transform.eulerAngles.y;

            // ���� ǥ�õ� ������ŭ �����Ѵ�.
            for (int i = 0; i < count; ++i)
            {
                // �ش� ������ �߻��� ����ĳ��Ʈ ������ �����´�.
                CastInfo info = GetCastInfo(fAngle + (angle * i));
                lineList.Add(info);

                // �ش� ����ĳ��Ʈ ������ ���� ���� �׸���.
                Debug.DrawLine(transform.position + offset, info.Point, Color.green);
            }

            yield return null;
        }
    }*/
}







