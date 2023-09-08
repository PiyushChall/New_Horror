using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    //These are the variables to control the sensivity of the mouse.
    public float sensiX;
    public float sensiY;

    float sensitivityMultiplier = 100f;

    float sensX;
    float sensY;


    //These are the variables to control the Orientation of the camera.
    public Transform Orientation;

    //These are the variables to control the rotation of the camera this is set to private so that we can only change this only through the script.
    float xRotation;
    float yRotation;


    // Start is called before the first frame update
    void Start()
    {
        sensX = sensiX * sensitivityMultiplier;
        sensY = sensiY * sensitivityMultiplier;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse input.
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mousey = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mousey;

        //This line of code makes sure that or mouse rotation doesn't exeeds 90Degree.
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //Rotating the camera
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        Orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
