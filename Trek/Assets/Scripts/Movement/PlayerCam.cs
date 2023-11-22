using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] float sensX;
    [SerializeField] float sensY;

    [SerializeField] Transform orientation;
    [SerializeField] Transform camHolder;

    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        MouseInput();
    }

    void MouseInput()
    {
        //Gets the users mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;


        yRotation += mouseX;
        xRotation -= mouseY;
        //clamps the rotation on the xAxis
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //applies rotations to the camera
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void SetFov(float endFovVal)
    {
        GetComponent<Camera>().DOFieldOfView(endFovVal, 0.15f);
    }

    public void SetTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.15f);
    }
}
