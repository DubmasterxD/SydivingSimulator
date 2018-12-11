using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {

    private int width = 0, height = 1, depth = 2;
    private float jumpersTopArea;
    private float jumpersFrontArea;
    private float jumpersMass;
    private float jumpersHeight;
    private float[] humanHead50c = { 15.9f, 23.8f };
    private float[] humanArms50c = { 175.0f, 10.0f, 10.0f };
    private float[] humanTorso50c = { 31.2f, 91.0f, 35.4f };
    private float[] humanLegs50c = { 25.0f, 50.2f };
    private float humanHeight50c = 175.0f;

	// Use this for initialization
	void Start ()
    {
        jumpersMass = GameController.instance.jumpersMass;
        jumpersHeight = GameController.instance.jumpersHeight;
        jumpersHeight = GameController.instance.jumpersHeight;
        jumpersFrontArea = humanHead50c[width] / humanHeight50c * jumpersHeight * humanHead50c[height] / humanHeight50c * jumpersHeight + humanArms50c[width] / humanHeight50c * jumpersHeight * humanArms50c[height] / humanHeight50c * jumpersHeight + humanTorso50c[width] / humanHeight50c * jumpersHeight * humanTorso50c[height] / humanHeight50c * jumpersHeight + humanLegs50c[width] / humanHeight50c * jumpersHeight * humanLegs50c[height] / humanHeight50c * jumpersHeight;
        jumpersFrontArea *= (jumpersMass * 5.5f) / (jumpersMass * 4.5f + 75);
        jumpersTopArea = humanTorso50c[width] / humanHeight50c * jumpersHeight * humanTorso50c[depth] / humanHeight50c * jumpersHeight + 40f / humanHeight50c * jumpersHeight * humanArms50c[depth] / humanHeight50c * jumpersHeight;
        jumpersTopArea *= (jumpersMass * 1.5f) / (jumpersMass * 0.5f + 75);
        gameObject.GetComponent<Falling>().jumpersTopProjectedArea = jumpersTopArea;
        gameObject.GetComponent<Falling>().jumpersFrontProjectedArea = jumpersFrontArea;
    }
}
