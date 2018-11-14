using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Variables
    private float jumpTimer = 0;
    private bool jumpStarted = false;
    private Vector3 startCamPosition = new Vector3(-0.52f, 4001f, 2.06f);       // For jump animation
    private Vector3 startCamRotation = new Vector3(0, 130.787f, 0);
    private Vector3 endCamPosition = new Vector3(-2f, 4001f, 0);
    private Vector3 endCamRotation = new Vector3(0, 90f, 0);

    // GUI components
    public Button jumpButton;
    public Button editButton;
    public Button exitButton;
    public Slider planeSpeedSlider;
    public Slider humanHeightSlider;
    public Slider latitudeSlider;
    public Slider altitudeSlidert;
    public Slider massSlider;
    public Slider heightSlider;
    public Text heightValueText;
    public Text planeSpeedValueText;
    public Text humanHeightValueText;
    public Text latitudeValueText;
    public Text altitudeValueText;
    public Text massValueText;
    public Dropdown licenseDropdown;

    // References
    public GameObject plane;
    public Camera mainCamera;
    private Animator playerAnim;
    private Rigidbody planeRB;

    private void Start()
    {
        playerAnim = GameController.instance.player.GetComponent<Animator>();
        planeRB = plane.GetComponent<Rigidbody>();
        playerAnim.speed = 0; // Pausing animation
        mainCamera.gameObject.transform.localPosition = startCamPosition;
        mainCamera.gameObject.transform.localRotation = Quaternion.Euler(startCamRotation);
    }

    void Update()
    {
        // If jump animation ended
        if (playerAnim.GetNextAnimatorClipInfoCount(0) == 1)
        {
            SceneManager.LoadScene("Map");
            GameController.instance.gameRunning = true;
        }
    }

    void FixedUpdate()
    {
        if (jumpStarted == true)
        {
            mainCamera.gameObject.transform.localPosition = new Vector3(startCamPosition[0] + jumpTimer * (endCamPosition[0] - startCamPosition[0]) / 2, startCamPosition[1] + jumpTimer * (endCamPosition[1] - startCamPosition[1]) / 2, startCamPosition[2] + jumpTimer * (endCamPosition[2] - startCamPosition[2]) / 2);
            mainCamera.gameObject.transform.localRotation = Quaternion.Euler(startCamRotation[0] + jumpTimer * (endCamRotation[0] - startCamRotation[0]) / 2, startCamRotation[1] + jumpTimer * (endCamRotation[1] - startCamRotation[1]) / 2, startCamRotation[2] + jumpTimer * (endCamRotation[2] - startCamRotation[2]) / 2);
            jumpTimer += Time.fixedDeltaTime;
            if (jumpTimer > 1.3f)
            {
                // Plane moves away as jumper gets off of the ground
                planeRB.MovePosition(new Vector3(plane.transform.position.x + 3 * Time.fixedDeltaTime, plane.transform.position.y, plane.transform.position.z - 3 * Time.fixedDeltaTime));
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Jump()
    {
        jumpStarted = true;
        jumpButton.gameObject.SetActive(false);
        editButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        playerAnim.speed = 1; // Start animation
    }

    // Load data from GameController
    public void Edit()
    {
        massValueText.text = GameController.instance.mass + " kg";
        humanHeightValueText.text = GameController.instance.humanHeight * 100 + " cm";
        heightValueText.text = GameController.instance.startingHeight + " m";
        altitudeValueText.text = GameController.instance.startingAltitude + " m";
        latitudeValueText.text = GameController.instance.latitude + "°";
        planeSpeedValueText.text = GameController.instance.planeSpeed + "m/s";
        planeSpeedSlider.value = GameController.instance.planeSpeed;
        humanHeightSlider.value = GameController.instance.humanHeight * 100;
        latitudeSlider.value = GameController.instance.latitude;
        altitudeSlidert.value = GameController.instance.startingAltitude;
        massSlider.value = GameController.instance.mass;
        heightSlider.value = GameController.instance.startingHeight;
        licenseDropdown.value = (int)GameController.instance.license;
    }

    public void SetMass(float newMass)
    {
        GameController.instance.mass = newMass;
        massValueText.text = newMass + " kg";
    }

    public void SetHumanHeight(float newHeight)
    {
        GameController.instance.humanHeight = newHeight / 100;
        humanHeightValueText.text = newHeight + " cm";
    }

    public void SetStartingHeight(float newHeight)
    {
        GameController.instance.startingHeight = newHeight;
        heightValueText.text = newHeight + " m";
    }

    public void SetStartingAltitude(float newAltitude)
    {
        GameController.instance.startingAltitude = newAltitude;
        // Height can't be higher than altitude
        if (heightSlider.value > newAltitude)
        {
            heightSlider.value = newAltitude;
        }
        heightSlider.maxValue = newAltitude;
        altitudeValueText.text = newAltitude + " m";
    }

    public void SetLatitude(float newLatitude)
    {
        GameController.instance.latitude = newLatitude;
        latitudeValueText.text = newLatitude + "°";
    }

    public void SetSpeedOfPlane(float newSpeed)
    {
        GameController.instance.planeSpeed = newSpeed;
        planeSpeedValueText.text = newSpeed + "m/s";
    }

    public void SetLicense(int license)
    {
        GameController.instance.license = (GameController.Licenses)license;
    }
}
