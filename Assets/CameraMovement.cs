using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform objectTofollow; // ���󰡾� �� ������Ʈ�� ����.
    public float followSpeed = 10f; //���� �ӵ�
    public float sensitivity = 100f; //����
    public float clampAngle = 70f; // ī�޶� ������ �� ���� ����

    private float rotX;
    private float rotY; // ���콺 �Է¹��� ������ 

    public Transform realCamera; //ī�޶� ����
    public Vector3 dirNormalized; // ���� ����
    public Vector3 finalDir; // ���������� ������ ����
    public float minDistance;
    public float maxDistance; // �ּ�, �ִ�Ÿ� 
    public float finalDistance;// �����Ÿ�
    public float smoothness = 10f;
    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized; // ��ֶ���� �ϸ� ũ�Ⱑ 0���� �Ǽ� ���⸸.
        finalDistance = realCamera.localPosition.magnitude; // magnitude - ũ�� 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        rotX += -Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // Time.deltatime -������ �����ӿ��� ���� �����ӱ����� �ð� ����

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle); //Mathf �޼ҵ�� ���� ����
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0); // ȸ�� �޼ҵ�
        transform.rotation = rot;
    }

    void LateUpdate()
    { // Update�� ���� �Ŀ� �۵�
        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position, followSpeed);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);  // ���ý����̽����� ���彺���̽��� �ٲ���

        RaycastHit hit; // ���ع� ������Ʈ�� ������ �����ϴ� ����
        if (Physics.Linecast(transform.position, finalDir, out hit))
        { // ���� ���ع��� ������ 
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            finalDistance = maxDistance;
        }
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);

    }
}
