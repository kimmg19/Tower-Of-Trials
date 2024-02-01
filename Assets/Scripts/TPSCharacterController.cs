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
    public float cameraSensitivity;
    Animator animator;
    public float playerSpeed=5f;    
    float mouseSensitivity = 0.3f;

    void Start() {
        animator = characterBody.GetComponent<Animator>();
    }    

    public void OnLookAround(InputAction.CallbackContext context) {
        float mouseDeltaX = context.ReadValue<float>()* mouseSensitivity;
        float mouseDeltaY = context.ReadValue<float>()* mouseSensitivity;

        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDeltaY;

        float mouseY = (x > 180f) ? Mathf.Clamp(x, 335f, 361f) : Mathf.Clamp(x, -1f, 70f);

        cameraArm.rotation = Quaternion.Euler(mouseY, camAngle.y + mouseDeltaX, camAngle.z);
    }

    public void OnMove(InputAction.CallbackContext context) {
        Vector2 moveInput = context.ReadValue<Vector2>();
        bool isMoving = moveInput.magnitude != 0;
        animator.SetBool("isMoving", isMoving);

        if (isMoving) {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            characterBody.forward = moveDir;
            transform.position += moveDir * Time.deltaTime * 8f;
        }
    }
    /*void Attack() {
        if (Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("onWeaponAttack");
        }
    }*/
    
}
