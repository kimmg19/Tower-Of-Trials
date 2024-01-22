using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {
    float speed = 10f;
    Animator animator;
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Move();

        
    }

    private void Move() {
            if (Input.GetKey(KeyCode.W)) {
                animator.Play("runFoward");
                transform.position += new Vector3(0, 0, 1) * Time.deltaTime * speed;
            } else if (Input.GetKey(KeyCode.S)) {
                animator.Play("runBackward");
                transform.position += new Vector3(0, 0, -1) * Time.deltaTime * speed;

            } else if (Input.GetKey(KeyCode.A)) {
                animator.Play("runLeftward");
                transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * speed;
            } else if (Input.GetKey(KeyCode.D)) {
                animator.Play("runRightward");
                transform.position += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            }else if(Input.GetMouseButtonDown(0)) {
                animator.Play("Attack");
            } 
            else
                animator.Play("Idle");
        
    }
    
}
