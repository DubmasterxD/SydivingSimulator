using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.position = new Vector3(rb.position.x, GameController.instance.startingAltitude, rb.position.z);
    }

    void FixedUpdate()
    {
        // Plane flight direction and speed
        if (GameController.instance.gameRunning)
        {
            rb.position += new Vector3(GameController.instance.planeSpeed * Time.fixedDeltaTime, 0, -3 * Time.fixedDeltaTime);
        }
    }
}
