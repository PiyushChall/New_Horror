using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{

    //Variable for the CameraPos Object.
    public Transform CamPos;

    // Update is called once per frame
    void Update()
    {
        transform.position = CamPos.position;
    }

    //I Made this code Because when we Set the camera to an Object with rigidbody it becomes laggi.
    //so I didn't parent the player directly to the camera, instead I parented a seperate Game object and made it follow the Player.
}
