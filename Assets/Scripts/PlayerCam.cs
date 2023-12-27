using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensx = 400f; //x sensitivity
    public float sensy = 400f;  //y sensitivity
    public Transform orientation;   //orientation of the player
    float xRotation = 0f;
    float yRotation = 0f;

    private void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;
       Cursor.visible = false;
    }

    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensx;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensy;

        //calculate camera rotation
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
