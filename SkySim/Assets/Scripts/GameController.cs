using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public GameObject player;
    public Text height;
    public Text length;
    public Text timer;
    public float startingHeight;
    private Vector2 startingAnchorHeightX;
    private Vector2 startingAnchorHeightY;
    private Vector2 startingAnchorLengthX;
    private Vector2 startingAnchorLengthY;
    private float timeSinceJump = 0;
    public bool gameStarted = false;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name!="Menu")
        {
            startingAnchorHeightX = new Vector2(height.rectTransform.anchorMin.x, height.rectTransform.anchorMax.x);
            startingAnchorHeightY = new Vector2(height.rectTransform.anchorMin.y, height.rectTransform.anchorMax.y);
            startingAnchorLengthX = new Vector2(length.rectTransform.anchorMin.x, length.rectTransform.anchorMax.x);
            startingAnchorLengthY = new Vector2(length.rectTransform.anchorMin.y, length.rectTransform.anchorMax.y);
            gameStarted = true;
        }
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            instance.height = height;
            instance.length = length;
            instance.timer = timer;
            instance.player = player;
            instance.startingAnchorHeightX = new Vector2(height.rectTransform.anchorMin.x, height.rectTransform.anchorMax.x);
            instance.startingAnchorHeightY = new Vector2(height.rectTransform.anchorMin.y, height.rectTransform.anchorMax.y);
            instance.startingAnchorLengthX = new Vector2(length.rectTransform.anchorMin.x, length.rectTransform.anchorMax.x);
            instance.startingAnchorLengthY = new Vector2(length.rectTransform.anchorMin.y, length.rectTransform.anchorMax.y);
            instance.gameStarted = gameStarted;
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            timeSinceJump += Time.deltaTime;
            if (Mathf.FloorToInt(timeSinceJump) % 60 < 10)
            {
                timer.text = "0" + Mathf.Floor(timeSinceJump / 60) + ":0" + Mathf.FloorToInt(timeSinceJump) % 60;
            }
            else
            {
                timer.text = "0" + Mathf.Floor(timeSinceJump / 60) + ":" + Mathf.FloorToInt(timeSinceJump) % 60;
            }
            height.text = Mathf.Floor(player.GetComponent<Falling>().location[1]).ToString();
            height.rectTransform.anchorMax = new Vector2(startingAnchorHeightX[1], startingAnchorHeightY[1] + (player.GetComponent<Falling>().location[1] - startingHeight) * (((float)height.fontSize / Display.main.renderingHeight) - startingAnchorHeightY[1]) / (0 - startingHeight));
            length.text = Mathf.Floor(player.GetComponent<Falling>().location[0]).ToString() + "\n|";
            length.rectTransform.anchorMin = new Vector2(startingAnchorLengthX[0] + (2000 - player.GetComponent<Falling>().location[0] - startingHeight / 2) * (0 - startingAnchorLengthX[0]) / (0 - startingHeight / 2), startingAnchorLengthY[0]);
        }
    }
}
