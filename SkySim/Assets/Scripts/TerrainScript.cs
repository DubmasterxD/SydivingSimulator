using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainScript : MonoBehaviour
{
    void Start()
    {
        // Moving terrain so the distance between it and player is startingHeight
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(GameController.instance.player.GetComponent<Falling>().location, new Vector3(0, -1, 0), out hit);
        gameObject.transform.position += new Vector3(0, hit.distance - GameController.instance.startingHeight, 0);
    }
}
