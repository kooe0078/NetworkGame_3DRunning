using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public float CameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObj;
    //Vector3 FollowPos;
    public float clampAngle = 20.0f;
    public float InputSensitivity = 2.0f;

    public float finalInputX;
    public float finalInputZ;
    public float smoothX;
    public float smoothY;
    private float rotX = 0.0f;
    private float rotY = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        //커서 비활성화
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        finalInputX = mouseX;
        finalInputZ = -mouseY;

        rotY += finalInputX * InputSensitivity + Time.deltaTime;
        rotX += finalInputZ * (InputSensitivity / 3) + Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle / 2, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;
        CameraUpdater();
    }
    void CameraUpdater()
    {
        Transform target = CameraFollowObj.transform;
        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
