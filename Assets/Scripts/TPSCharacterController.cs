using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class TPSCharacterController : MonoBehaviour {
    [SerializeField]
    Transform characterBody;
    [SerializeField]
    Transform cameraArm;
    Animator animator;
    public float playerSpeed = 5f;
    float mouseSensitivity = 0.1f;
    float turnSmoothVelocity;

    public Vector2 moveInput { get; private set; }
    void Start() {
        animator = characterBody.GetComponent<Animator>();
    }
    void FixedUpdate() {
        Move();
    }
    public void OnLookAround(InputAction.CallbackContext context) {
        // X 및 Y 각각의 값을 따로 읽어오기
        float mouseDeltaX = context.ReadValue<Vector2>().x * mouseSensitivity;
        float mouseDeltaY = context.ReadValue<Vector2>().y * mouseSensitivity;

        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDeltaY;

        float mouseY = (x > 180f) ? Mathf.Clamp(x, 335f, 361f) : Mathf.Clamp(x, -1f, 70f);

        cameraArm.rotation = Quaternion.Euler(mouseY, camAngle.y + mouseDeltaX, camAngle.z);
    }
    void Move() {
        bool isMoving = moveInput.magnitude != 0;
        animator.SetBool("isMoving", isMoving);
        if (isMoving) {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z);
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z);
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            //characterBody.forward = moveDir;

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float smoothDampTime = 0.05f;
            float currentAngle = Mathf.SmoothDampAngle(characterBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothDampTime);

            characterBody.rotation = Quaternion.Euler(0f, currentAngle, 0f);
            transform.position += moveDir * Time.deltaTime * playerSpeed;
            print(moveDir.magnitude);
            
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();        
    }
    /*void Attack() {
        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("onWeaponAttack");
        }
    }*/

}
