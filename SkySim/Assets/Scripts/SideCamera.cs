using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCamera : MonoBehaviour
{
    // Reference
    private Camera cam;

    void Start()
    {
        // Sets camera to show exactly area between jumper and ground
        cam = gameObject.GetComponent<Camera>();
        cam.orthographicSize = GameController.instance.startingHeight / 2;

        cam.transform.position = new Vector3(cam.orthographicSize * 3 / 4, 
            cam.orthographicSize + GameController.instance.startingAltitude - GameController.instance.startingHeight, 
                                             cam.transform.position.z);
    }
}
