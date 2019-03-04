using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    private float startingDistGroundSea = 207.3877f;  //starting height above sea level of ground

    void Start()
    {
        float startingAltitude = GameController.instance.startingAltitude;
        float startingHeight = GameController.instance.startingHeight;
        // Moving terrain so the distance between it and player is startingHeight
        gameObject.transform.position += new Vector3(0, startingAltitude - startingHeight - startingDistGroundSea, 0);
    }
}
