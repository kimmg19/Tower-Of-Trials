using UnityEngine;

public class Portal : MonoBehaviour
{
    // ĵ������ ������ ����
    [SerializeField]
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        // ĵ������ ��Ȱ��ȭ
        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }

    // �浹�� ���۵� �� ȣ���
    void OnTriggerEnter(Collider other)
    {
        // �浹�� ������Ʈ�� "Portal" �±׸� ���� ��쿡�� ó��
        if (other.CompareTag("Player"))
        {
            // ĵ������ Ȱ��ȭ
            if (canvas != null)
            {
                canvas.enabled = true;
                print("collision");
            }
            
        }
    }

    // �浹�� ���� �� ȣ���
    void OnTriggerExit(Collider other)
    {
        // �浹�� ������Ʈ�� "Portal" �±׸� ���� ��쿡�� ó��
        if (other.CompareTag("Player"))
        {
            // ĵ������ ��Ȱ��ȭ
            if (canvas != null)
            {
                canvas.enabled = false;
            }
        }
    }
}
