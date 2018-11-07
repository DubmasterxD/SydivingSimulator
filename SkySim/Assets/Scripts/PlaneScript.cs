using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour {
    
	// Update is called once per frame
	void Update ()
    {
        gameObject.GetComponent<Rigidbody>().position += new Vector3(45*Time.deltaTime, 0, -3*Time.deltaTime);
    }
}
