using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Cube")) {
            print("ť�� �浹");
            animator.SetTrigger("hitTrigger");
        }
    }
}
