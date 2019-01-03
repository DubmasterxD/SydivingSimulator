using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    // Reference
    private Falling jumpersFalling;

    // Variables
    public float rotationSpeed = 10.0f;

    void Start()
    {
        jumpersFalling = gameObject.GetComponent<Falling>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameController.instance.gameRunning)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                jumpersFalling.jumpersRotation[0] += rotationSpeed * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                jumpersFalling.jumpersRotation[0] -= rotationSpeed * Time.fixedDeltaTime;
            }
            if (jumpersFalling.jumpersRotation[0] > 180)
            {
                jumpersFalling.jumpersRotation[0] = 180;
            }
            else if (jumpersFalling.jumpersRotation[0] < 0)
            {
                jumpersFalling.jumpersRotation[0] = 0;
            }
            if (jumpersFalling.jumpersRotation[0] - 90 > rotationSpeed / 5 * Time.fixedDeltaTime)
            {
                jumpersFalling.jumpersRotation[0] -= rotationSpeed / 5 * Time.fixedDeltaTime;
            }
            else if (jumpersFalling.jumpersRotation[0] - 90 < -rotationSpeed / 5 * Time.fixedDeltaTime)
            {
                jumpersFalling.jumpersRotation[0] += rotationSpeed / 5 * Time.fixedDeltaTime;
            }
        }
    }
}
