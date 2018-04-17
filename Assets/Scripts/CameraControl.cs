using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {


    [SerializeField]
    float speedH = 5.0f;
    [SerializeField]
    float speedV = 2.0f;

    private float yaw;
    private float pitch;



    private void Update()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        pitch = Mathf.Clamp(pitch, 20f, 35f);
        yaw = Mathf.Clamp(yaw, -40f, 40f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        
    }
}
