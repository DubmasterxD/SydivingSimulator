using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    private float startingDistGroundSea = 207.3877f;

    void Start()
    {
        // Moving terrain so the distance between it and player is startingHeight
        gameObject.transform.position += new Vector3(0, GameController.instance.startingAltitude-GameController.instance.startingHeight - startingDistGroundSea, 0);
    }
}
