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
    PlayerStats playerstats;

   
    void Start()
    {
        playerstats = GetComponent<PlayerStats>();
        targetList = new();
 
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
                        enemy.TakeDamage(Mathf.RoundToInt(playerstats.Attack * 1.5f), false);
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
        Vector3 dir = new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0.0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
        CastInfo Info;
        Info.Angle = _angle;            // ����
        Info.Distance = viewRange;      // �ִ� ���� �Ÿ��� Range                                        
        Info.Point = transform.position + dir * viewRange; //��ġ �����ϰ� ������offset ���ϸ� ��

        return Info;
    }

}