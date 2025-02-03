using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    GameObject circle;
    void Start() {
        this.circle = GameObject.Find("Circle");
    }

    void Update() {
        Vector3 circlePos = this.circle.transform.position;
        if (Math.Abs(circlePos.x-transform.position.x)<=10) transform.position = new Vector3(transform.position.x,circlePos.y,transform.position.z);
        else if (circlePos.x-transform.position.x>5) {
            transform.position = new Vector3(circlePos.x-10,circlePos.y,transform.position.z);
        }
        else {
            transform.position = new Vector3(circlePos.x+10,circlePos.y,transform.position.z);
        }
    }
}
