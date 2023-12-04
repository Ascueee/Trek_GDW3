using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpRotation : MonoBehaviour
{
    [Header("Weapon Sway")]
    [SerializeField] float smooth;
    [SerializeField] float swayMultiplier;



    // Update is called once per frame
    void Update()
    {
        GetMouseInput();
    }

    void GetMouseInput()
    {
        //gets the mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * swayMultiplier;

        //calculate target rotation
        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
