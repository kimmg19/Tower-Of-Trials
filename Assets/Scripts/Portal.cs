using UnityEngine;

public class Portal : MonoBehaviour
{
    // 캔버스를 참조할 변수
    [SerializeField]
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        // 캔버스를 비활성화
        if (canvas != null)
        {
            canvas.enabled = false;
        }
    }

    // 충돌이 시작될 때 호출됨
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 "Portal" 태그를 가진 경우에만 처리
        if (other.CompareTag("Player"))
        {
            // 캔버스를 활성화
            if (canvas != null)
            {
                canvas.enabled = true;
                print("collision");
            }
            
        }
    }

    // 충돌이 끝날 때 호출됨
    void OnTriggerExit(Collider other)
    {
        // 충돌한 오브젝트가 "Portal" 태그를 가진 경우에만 처리
        if (other.CompareTag("Player"))
        {
            // 캔버스를 비활성화
            if (canvas != null)
            {
                canvas.enabled = false;
            }
        }
    }
}
