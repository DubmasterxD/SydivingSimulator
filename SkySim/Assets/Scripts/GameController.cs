using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum Licenses { student, A, B, C, D };

    // Variables
    public float startingAltitude;
    public float startingHeight;
    public float jumpersMass;
    public float jumpersLatitude;
    public float jumpersHeight;
    public float planeSpeed;
    public float minHeight = 457;
    public float[] warningHeights = { 1219, 1066, 914, 762, 609 };
    public Licenses license;
    public bool gameRunning = false;
    public bool challengeJump = false;

    // References
    public static GameController instance;
    public GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            instance.player = player;
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
