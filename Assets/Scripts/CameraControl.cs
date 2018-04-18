using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {


    [SerializeField]
    float sensitivity = 5.0f;
    [SerializeField]
    float smoothing = 2.0f;

    private Vector2 mouseLook;
    private Vector2 smoothV;

    [SerializeField]
    GameObject placement;

    private void Update()
    {
        var md = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
        mouseLook += smoothV;
         

        mouseLook.y = Mathf.Clamp(mouseLook.y, 0f, 40f);
        mouseLook.x = Mathf.Clamp(mouseLook.x, -35f, 35f);

        transform.localRotation = Quaternion.AngleAxis(mouseLook.y, Vector3.right);
        placement.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, placement.transform.up);

        
    }
}
