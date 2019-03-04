using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUI : MonoBehaviour
{
    // Variables
    private Vector2 startingAnchorGUIHeightX;
    private Vector2 startingAnchorGUIHeightY;
    private Vector2 startingAnchorGUILengthX;
    private Vector2 startingAnchorGUILengthY;
    private float timeSinceJump = 0;
    private float maxSpeed = 0;
    private float sumSpeed = 0;
    private float sumAngleOfAttack = 0;
    private float currLocation;

    // GUI components
    public Text height;
    public Text length;
    public Text timer;
    public Text speed;
    public Text verticalVelocity;
    public Text horizontalVelocity;
    public Text scores;
    public Button deployParachute;
    public RawImage warningLine;                    // Line on minimap to show recommended lowest height to open parachute
    public RawImage endLine;                        // Line on minimap to show lowest height possible without
    public GameObject warning;                      // GUI height warning
    public GameObject ingame;                       // GUI while falling
    public GameObject scoreboard;                   // GUI after falling

    // References 
    public GameObject player;
    private Falling playerFalling;

    void Start()
    {
        playerFalling = player.GetComponent<Falling>();
        currLocation = playerFalling.jumpersLocation[1] - GameController.instance.startingAltitude + 
            GameController.instance.startingHeight;

        startingAnchorGUIHeightX = new Vector2(height.rectTransform.anchorMin.x, height.rectTransform.anchorMax.x);
        startingAnchorGUIHeightY = new Vector2(height.rectTransform.anchorMin.y, height.rectTransform.anchorMax.y);
        startingAnchorGUILengthX = new Vector2(length.rectTransform.anchorMin.x, length.rectTransform.anchorMax.x);
        startingAnchorGUILengthY = new Vector2(length.rectTransform.anchorMin.y, length.rectTransform.anchorMax.y);
        endLine.rectTransform.anchorMin = new Vector2(0, 
            GameController.instance.minHeight / GameController.instance.startingHeight);

        endLine.rectTransform.anchorMax = new Vector2(0.75f, 
            GameController.instance.minHeight / GameController.instance.startingHeight);

        warningLine.rectTransform.anchorMin = new Vector2(0,
            GameController.instance.warningHeights[(int)GameController.instance.license] / GameController.instance.startingHeight);

        warningLine.rectTransform.anchorMax = new Vector2(0.75f, 
            GameController.instance.warningHeights[(int)GameController.instance.license] / GameController.instance.startingHeight);

        if(GameController.instance.challengeJump)
        {
            deployParachute.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (GameController.instance.gameRunning)
        {
            if (!GameController.instance.challengeJump)
            {
                if (Input.GetKeyDown(KeyCode.Space) || currLocation < GameController.instance.minHeight)
                {
                    DeployParachute();
                }
            }
            else
            {
                if (currLocation < GameController.instance.warningHeights[4])
                {
                    DeployParachute();
                }
            }
            speed.text = Mathf.Sqrt(Mathf.Pow(playerFalling.jumpersVelocity[0], 2) + Mathf.Pow(playerFalling.jumpersVelocity[1], 2)).ToString();
            horizontalVelocity.text = playerFalling.jumpersVelocity[0].ToString();
            verticalVelocity.text = playerFalling.jumpersVelocity[1].ToString();
            // Setting timer
            if (Mathf.FloorToInt(timeSinceJump) % 60 < 10)
            {
                timer.text = "0" + Mathf.Floor(timeSinceJump / 60) + ":0" + Mathf.FloorToInt(timeSinceJump) % 60;
            }
            else
            {
                timer.text = "0" + Mathf.Floor(timeSinceJump / 60) + ":" + Mathf.FloorToInt(timeSinceJump) % 60;
            }
            currLocation = playerFalling.jumpersLocation[1] - GameController.instance.startingAltitude + GameController.instance.startingHeight;
            // Setting location of height and length text on GUI
            height.text = Mathf.Floor(currLocation).ToString();
            height.rectTransform.anchorMax = new Vector2(startingAnchorGUIHeightX[1], startingAnchorGUIHeightY[1] + (currLocation - GameController.instance.startingHeight) * (((float)height.fontSize / Display.main.renderingHeight) - startingAnchorGUIHeightY[1]) / (0 - GameController.instance.startingHeight));
            length.text = Mathf.Floor(playerFalling.jumpersLocation[0]).ToString() + "\n|";
            length.rectTransform.anchorMin = new Vector2(startingAnchorGUILengthX[0] + playerFalling.jumpersLocation[0] * (0 - startingAnchorGUILengthX[0]) / (GameController.instance.startingHeight * 3 / 4 - 0), startingAnchorGUILengthY[0]);
            // Checking when warning GUI should be shown
            if (currLocation < GameController.instance.warningHeights[(int)GameController.instance.license])
            {
                warning.gameObject.SetActive(true);
            }
        }
    }

    void FixedUpdate()
    {
        if (GameController.instance.gameRunning)
        {
            // Checking absolut maximum of vertical velocity
            if (Mathf.Abs(float.Parse(speed.text)) > Mathf.Abs(maxSpeed))
            {
                maxSpeed = float.Parse(speed.text);
            }
            sumSpeed += float.Parse(speed.text);
            sumAngleOfAttack += playerFalling.angleOfAttack;
            timeSinceJump += Time.fixedDeltaTime;
        }
    }

    // Deploy a parachute and finish the jump with statistics
    public void DeployParachute()
    {
        GameController.instance.gameRunning = false;
        ingame.SetActive(false);
        scoreboard.SetActive(true);
        // Create random between endLine and warningLine. If the player is above this random, show statisctics, otherwise show info about failing the jump
        if (!GameController.instance.challengeJump)
        {
            if ((currLocation > Random.Range(GameController.instance.minHeight, GameController.instance.warningHeights[(int)GameController.instance.license])))
            {
                scores.text = "Altitude of a jump: " + GameController.instance.startingAltitude + " meters"
                    + "\nHeight of a jump: " + GameController.instance.startingHeight + " meters"
                    + "\nLatitude of a jump: " + GameController.instance.jumpersLatitude + "°"
                    + "\nHeight of a human: " + (GameController.instance.jumpersHeight * 100) + " centimeters"
                    + "\nMass of a human: " + GameController.instance.jumpersMass + " kilograms"
                    + "\nTime spent in air: " + Mathf.Floor(timeSinceJump / 60) + " minutes " + (timeSinceJump % 60) + " seconds"
                    + "\nTraveled vertical distance: " + (GameController.instance.startingAltitude - playerFalling.jumpersLocation[1]) + " meters"
                    + "\nTraveled horizontal distance: " + playerFalling.jumpersLocation[0] + " meters"
                    + "\nAverage angle of attack: " + (sumAngleOfAttack / timeSinceJump * Time.fixedDeltaTime) + "°"
                    + "\nMax speed: " + maxSpeed + " meters per second"
                    + "\nAverage speed: " + (sumSpeed / timeSinceJump * Time.fixedDeltaTime) + " meters per second";
            }
            else
            {
                scores.text = "You opened your parachute too late and had some malfunctions so you didn't manage to fix that or deploy your reserve parachute in time, so you're lucky that you survived. But you were taken to hospital with major injuries.\n Please respect the rules and deploy your parachute higher than recommended minimum for your license as you're risking your life if some malfuctions were to happen.";
            }
        }
        else
        {
            scores.text = "\nTime spent in air: " + Mathf.Floor(timeSinceJump / 60) + " minutes " + (timeSinceJump % 60) + " seconds"
                + "\nTraveled horizontal distance: " + playerFalling.jumpersLocation[0] + " meters"
                + "\nAverage angle of attack: " + (sumAngleOfAttack / timeSinceJump * Time.fixedDeltaTime) + "°"
                + "\nMax speed: " + maxSpeed + " meters per second"
                + "\nAverage speed: " + (sumSpeed / timeSinceJump * Time.fixedDeltaTime) + " meters per second";
            if (GameController.instance.challengeJump)
            {
                if ((sumSpeed / timeSinceJump * Time.fixedDeltaTime) > 95.4f)
                {
                    scores.text += "\nCongratulations you passed the challenge and beat average speed of 95.4 m/s!";
                }
                else
                {
                    scores.text += "\nYou didn't pass the challenge of average speed above 95.4 m/s, good luck next time.";
                }
            }
        }
    }

    // After opening parachute
    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu");
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

    // While still falling
    public void QuitToMenu()
    {
        GameController.instance.gameRunning = false;
        SceneManager.LoadScene("Menu");
    }
}
