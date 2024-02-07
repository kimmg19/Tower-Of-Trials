using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    Transform characterBody;
    [SerializeField]
    Transform followCam;
    Animator animator;
    public float playerSpeed = 5f;
    public float smoothDampTime = 0.15f;
    public float gravity = -9.8f;
    public float attackmotiontime = 0.35f;
    public Vector2 moveInput { get; private set; }
    Vector3 velocity;
    float turnSmoothVelocity;
    CharacterController characterController;
    bool isAttacking = false;

    void Start() {
        animator = characterBody.GetComponent<Animator>();
        characterController = characterBody.GetComponent<CharacterController>();
    }

    void Update() {
        Move();
        ApplyGravity();
    }

    void Move() {
        bool isMoving = !isAttacking && moveInput.magnitude != 0;
        animator.SetBool("isMoving", isMoving);
        if (isMoving) {
            Vector3 lookForward = new Vector3(followCam.forward.x, 0f, followCam.forward.z).normalized;
            Vector3 lookRight = new Vector3(followCam.right.x, 0f, followCam.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;

            float currentAngle = Mathf.SmoothDampAngle(characterBody.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothDampTime);

            characterBody.rotation = Quaternion.Euler(0f, currentAngle, 0f);
            characterController.Move(moveDir * Time.deltaTime * playerSpeed);
        }
    }

    void ApplyGravity() {
        if (!characterController.isGrounded) {
            velocity.y += gravity * Time.deltaTime;
        } else {
            velocity.y = -0.5f; // 지면에 닿아 있을 때 중력 초기화
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput != null) {

        }
    }
    public void OnAttack(InputAction.CallbackContext context) {
        if (context.performed) {
            isAttacking = true;
            animator.SetTrigger("onWeaponAttack");
            StartCoroutine(ResetAttackMotion());
        }
    }

    IEnumerator ResetAttackMotion() {
        yield return new WaitForSeconds(attackmotiontime);
        isAttacking = false;
    }
}