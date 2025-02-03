using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CircleController : MonoBehaviour
{
    Rigidbody2D rigid2D;
    float jumpForce = 6800.0f;
    float walkForce = 100.0f;
    float maxWalkSpeed = 2.0f;
    void Start() {
        this.rigid2D=GetComponent<Rigidbody2D>();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)&&this.rigid2D.linearVelocity.y==0) this.rigid2D.AddForce(transform.up*this.jumpForce);
        int direction = 0;
        if (Input.GetKey(KeyCode.RightArrow)) direction = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) direction = -1;
        float speedx = Mathf.Abs(this.rigid2D.linearVelocity.x);
        if (speedx<this.maxWalkSpeed) {
            this.rigid2D.AddForce(transform.right*direction*this.walkForce);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
    }
}
