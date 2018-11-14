using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUI : MonoBehaviour
{
    // Variables
    private Vector2 startingAnchorHeightX;
    private Vector2 startingAnchorHeightY;
    private Vector2 startingAnchorLengthX;
    private Vector2 startingAnchorLengthY;
    private float timeSinceJump = 0;
    private float maxVerticalVelocity = 0;
    private float sumVerticalVelocity = 0;
    private float sumAngleOfAttack = 0;
    private float currLocation;

    // GUI components
    public Text height;
    public Text length;
    public Text timer;
    public Text verticalVelocity;
    public Text horizontalVelocity;
    public Text scores;
    public RawImage warningLine;                // Line on minimap to show recommended lowest height to open parachute
    public RawImage endLine;                    // Line on minimap to show lowest height possible without
    public GameObject warning;                  // GUI height warning
    public GameObject ingame;                   // GUI while falling
    public GameObject scoreboard;               // GUI after falling

    // References 
    public GameObject player;
    private Falling playerFalling;

    void Start()
    {
        playerFalling = player.GetComponent<Falling>();
        currLocation = playerFalling.location[1] - GameController.instance.startingAltitude + GameController.instance.startingHeight;
        startingAnchorHeightX = new Vector2(height.rectTransform.anchorMin.x, height.rectTransform.anchorMax.x);
        startingAnchorHeightY = new Vector2(height.rectTransform.anchorMin.y, height.rectTransform.anchorMax.y);
        startingAnchorLengthX = new Vector2(length.rectTransform.anchorMin.x, length.rectTransform.anchorMax.x);
        startingAnchorLengthY = new Vector2(length.rectTransform.anchorMin.y, length.rectTransform.anchorMax.y);
        endLine.rectTransform.anchorMin = new Vector2(0, GameController.instance.minHeight / GameController.instance.startingHeight);
        endLine.rectTransform.anchorMax = new Vector2(0.5f, GameController.instance.minHeight / GameController.instance.startingHeight);
        warningLine.rectTransform.anchorMin = new Vector2(0, GameController.instance.warningHeights[(int)GameController.instance.license] / GameController.instance.startingHeight);
        warningLine.rectTransform.anchorMax = new Vector2(0.5f, GameController.instance.warningHeights[(int)GameController.instance.license] / GameController.instance.startingHeight);
    }

    void Update()
    {
        if (GameController.instance.gameRunning)
        {
            if (Input.GetKeyDown(KeyCode.Space) || currLocation < GameController.instance.minHeight)
            {
                DeployParachute();
            }
            horizontalVelocity.text = playerFalling.velocity[0].ToString();
            verticalVelocity.text = playerFalling.velocity[1].ToString();
            // Setting timer
            if (Mathf.FloorToInt(timeSinceJump) % 60 < 10)
            {
                timer.text = "0" + Mathf.Floor(timeSinceJump / 60) + ":0" + Mathf.FloorToInt(timeSinceJump) % 60;
            }
            else
            {
                timer.text = "0" + Mathf.Floor(timeSinceJump / 60) + ":" + Mathf.FloorToInt(timeSinceJump) % 60;
            }
            currLocation = playerFalling.location[1] - GameController.instance.startingAltitude + GameController.instance.startingHeight;
            // Setting location of height and length text on GUI
            height.text = Mathf.Floor(currLocation).ToString();
            height.rectTransform.anchorMax = new Vector2(startingAnchorHeightX[1], startingAnchorHeightY[1] + (currLocation - GameController.instance.startingHeight) * (((float)height.fontSize / Display.main.renderingHeight) - startingAnchorHeightY[1]) / (0 - GameController.instance.startingHeight));
            length.text = Mathf.Floor(playerFalling.location[0]).ToString() + "\n|";
            length.rectTransform.anchorMin = new Vector2(startingAnchorLengthX[0] + playerFalling.location[0] * (0 - startingAnchorLengthX[0]) / (GameController.instance.startingHeight / 2 - 0), startingAnchorLengthY[0]);
            // Checking when warning GUI should be shown
            if (currLocation < GameController.instance.warningHeights[(int)GameController.instance.license])
            {
                warning.gameObject.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        // Checking absolut maximum of vertical velocity
        if (Mathf.Abs(playerFalling.velocity[1]) > Mathf.Abs(maxVerticalVelocity))
        {
            maxVerticalVelocity = playerFalling.velocity[1];
        }
        sumVerticalVelocity += playerFalling.velocity[1];
        sumAngleOfAttack += playerFalling.AngleOfAttack;
        timeSinceJump += Time.fixedDeltaTime;
    }

    // Deploy a parachute and finish the jump with statistics
    public void DeployParachute()
    {
        GameController.instance.gameRunning = false;
        ingame.SetActive(false);
        scoreboard.SetActive(true);
        // Create random between endLine and warningLine. If the player is above this random, show statisctics, otherwise show info about failing the jump
        if ((currLocation > Random.Range(GameController.instance.minHeight, GameController.instance.warningHeights[(int)GameController.instance.license])))
        {
            scores.text = "Altitude of a jump: " + GameController.instance.startingAltitude + " meters"
                + "\nHeight of a jump: " + GameController.instance.startingHeight + " meters"
                + "\nLatitude of a jump: " + GameController.instance.latitude + "°"
                + "\nHeight of a human: " + (GameController.instance.humanHeight * 100) + " centimeters"
                + "\nMass of a human: " + GameController.instance.mass + " kilograms"
                + "\nTime spent in air: " + Mathf.Floor(timeSinceJump / 60) + " minutes " + (timeSinceJump % 60) + " seconds"
                + "\nAverage vertical velocity: " + (sumVerticalVelocity / timeSinceJump * Time.fixedDeltaTime) + " meters per second"
                + "\nMax vertical velocity: " + maxVerticalVelocity + " meters per second"
                + "\nTraveled vertical distance: " + (GameController.instance.startingAltitude - playerFalling.location[1]) + " meters"
                + "\nTraveled horizontal distance: " + playerFalling.location[0] + " meters"
                + "\nAverage angle of attack: " + (sumAngleOfAttack / timeSinceJump * Time.fixedDeltaTime) + "°";
        }
        else
        {
            scores.text = "You opened your parachute too late and had some malfunctions so you didn't manage to fix that or deploy your reserve parachute in time, so you're lucky that you survived. But you were taken to hospital with major injuries.\n Please respect the rules and deploy your parachute higher than recommended minimum for your license as you're risking your life if some malfuctions were to happen.";
        }
    }

    public void PauseGame()
    {
        if (GameController.instance.gameRunning == true)
        {
            GameController.instance.gameRunning = false;
        }
        else
        {
            GameController.instance.gameRunning = true;
        }
    }

    // After opening parachute
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // While still falling
    public void QuitToMenu()
    {
        GameController.instance.gameRunning = false;
        SceneManager.LoadScene("Menu");
    }
}
