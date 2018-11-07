using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button jumpButton;
    public Button editButton;
    public Button exitButton;
    public GameObject plane;
    private float jumpTimer = 0;
    private bool jumpStarted = false;
    public Camera mainCamera;
    public Vector3 startCamPosition = new Vector3(-0.52f, 4001f, 2.06f);
    public Vector3 startCamRotation = new Vector3(0, 130.787f, 0);
    public Vector3 endCamPosition = new Vector3(-2f, 4001f, 0);
    public Vector3 endCamRotation = new Vector3(0, 90f, 0);

    private void Start()
    {
        GameController.instance.player.GetComponent<Animator>().speed = 0;
        mainCamera.gameObject.transform.localPosition = startCamPosition;
        mainCamera.gameObject.transform.localRotation = Quaternion.Euler(startCamRotation);
    }
    // Update is called once per frame
    void Update ()
    {
		if(GameController.instance.player.GetComponent<Animator>().GetNextAnimatorClipInfoCount(0)==1)
        {
            SceneManager.LoadScene("Map");
        }
        if(jumpStarted==true)
        {
            mainCamera.gameObject.transform.localPosition = new Vector3(startCamPosition[0] + jumpTimer * (endCamPosition[0] - startCamPosition[0]) / 2, startCamPosition[1] + jumpTimer * (endCamPosition[1] - startCamPosition[1]) / 2, startCamPosition[2] + jumpTimer * (endCamPosition[2] - startCamPosition[2]) / 2);
            mainCamera.gameObject.transform.localRotation = Quaternion.Euler(startCamRotation[0] + jumpTimer * (endCamRotation[0] - startCamRotation[0]) / 2, startCamRotation[1] + jumpTimer * (endCamRotation[1] - startCamRotation[1]) / 2, startCamRotation[2] + jumpTimer * (endCamRotation[2] - startCamRotation[2]) / 2);
            jumpTimer += Time.deltaTime;
            if (jumpTimer > 1.3f)
            {
                plane.GetComponent<Rigidbody>().MovePosition(new Vector3(plane.transform.position.x + 3* Time.deltaTime, plane.transform.position.y, plane.transform.position.z - 3* Time.deltaTime));
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
        GameController.instance.player.GetComponent<Animator>().speed = 1;
    }

    public void Edit()
    {

    }
}
