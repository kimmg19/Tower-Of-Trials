using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform objectTofollow; // 따라가야 할 오브젝트의 정보.
    public float followSpeed = 10f; //따라갈 속도
    public float sensitivity = 100f; //감도
    public float clampAngle = 70f; // 카메라를 움직일 때 제한 각도

    private float rotX;
    private float rotY; // 마우스 입력받을 변수들 

    public Transform realCamera; //카메라 정보
    public Vector3 dirNormalized; // 방향 벡터
    public Vector3 finalDir; // 최종적으로 결정된 방향
    public float minDistance;
    public float maxDistance; // 최소, 최대거리 
    public float finalDistance;// 최종거리
    public float smoothness = 10f;
    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized; // 노멀라이즈를 하면 크기가 0으로 되서 방향만.
        finalDistance = realCamera.localPosition.magnitude; // magnitude - 크기 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        rotX += -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // Time.deltatime -마지막 프레임에서 현재 프레임까지의 시간 간격

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle); //Mathf 메소드로 제한 각도
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0); // 회전 메소드
        transform.rotation = rot;
    }

    void LateUpdate()
    { // Update가 끝난 후에 작동
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);  // 로컬스페이스에서 월드스페이스로 바꿔줌

        RaycastHit hit; // 방해물 오브젝트의 정보를 저장하는 변수
        if (Physics.Linecast(transform.position, finalDir, out hit))
        { // 만약 방해물이 있으면 
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);

    }
}
