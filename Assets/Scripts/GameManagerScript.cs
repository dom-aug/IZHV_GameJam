using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Runtime.InteropServices;

public class GameManagerScript : MonoBehaviour
{
    public float gameSpeed = 0.0f;

    private float currentScore = 0.0f;
    public bool gameLost = false;
    public bool gameStarted = false;

    private float thresholdIncrease = 10.0f;
    private float thresholdAccumulator = 0.0f;

    private static GameManagerScript thisInstance;
     public static GameManagerScript Instance
    {
        get
        {
            return thisInstance;
        }
    }

    private Label scoreLabelReference;

    private Button startButtonReference;
    private Button exitButtonReference;

    private Button aboutButtonReference;
    private Button aboutBackButtonReference;


    [DllImport("__Internal")]
    private static extern void Exit();

    private void Awake()
    {
        // Initialize the singleton instance, if no other exists.
        if (thisInstance != null && thisInstance != this)
        {
            Destroy(gameObject);
        } else {
            thisInstance = this;
        }

        var uiGameObject = GameObject.Find("UI");
        scoreLabelReference = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<Label>("ScoreValue");

        thresholdAccumulator = thresholdIncrease;

        startButtonReference = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("StartButton");
        startButtonReference.RegisterCallback<ClickEvent>(ev => onStartGame());

        exitButtonReference = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("ExitButton");
        exitButtonReference.RegisterCallback<ClickEvent>(ev => onGameExit());

        aboutButtonReference = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("AboutButton");
        aboutButtonReference.RegisterCallback<ClickEvent>(ev => toggleAbout());

        aboutBackButtonReference = uiGameObject.GetComponent<UIDocument>().rootVisualElement.Q<Button>("AboutButtonMessage");
        aboutBackButtonReference.RegisterCallback<ClickEvent>(ev => toggleAbout());

        Debug.Log("Game manager awake");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted) {
            //if (gameStarted && !gameLost)
            //{
                currentScore += Time.deltaTime * gameSpeed / 10.0f;
            //}
            scoreLabelReference.text = $"{currentScore:0.00}";
            
            if (currentScore > thresholdAccumulator) {
                gameSpeed += 1f;
                thresholdAccumulator += thresholdIncrease;
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                ResetGame();
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }

    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseGame()
    {
        gameLost = true;
        gameSpeed = 0.0f;
        gameStarted = false;

        GameObject.Find("PathSpawner").GetComponent<PathSpawner>().spawnEnabled = false;
        GameObject.Find("ObstacleSpawner").GetComponent<SpawnerScript>().spawnEnabled = false;

        var uiGameObject = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        var mainMenu = uiGameObject.Q<VisualElement>("GameOver");
        mainMenu.visible = true;
        var gamePlay = uiGameObject.Q<VisualElement>("GamePlay");
        gamePlay.visible = false;

        var camera = GameObject.Find("Main Camera");
        camera.GetComponent<Animator>().SetBool("gameStarted", false);
        camera.transform.rotation = new Quaternion(0, 0, 0, 0);

        var scoreLabel = uiGameObject.Q<Label>("distanceResult");
        scoreLabel.text = $"{currentScore:0.00}";

        var GameOverHomeButton = uiGameObject.Q<Button>("GameOverHomeButton");
        GameOverHomeButton.RegisterCallback<ClickEvent>(ev => ResetGame());
    }

    public void onStartGame()
    {
        Debug.Log("Game started");

        //show score and torch life
        var uiGameObject = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        var mainMenu = uiGameObject.Q<VisualElement>("MainMenu");
        mainMenu.visible = false;
        var gamePlay = uiGameObject.Q<VisualElement>("GamePlay");
        gamePlay.visible = true;

        //rotate camera
        var camera = GameObject.Find("Main Camera");
        camera.GetComponent<Animator>().SetBool("gameStarted", true);
        camera.transform.rotation = new Quaternion(0, -5f, 0, 0);


        // set bool variables
        gameStarted = true;
        gameLost = false;

        // reset score, speed
        currentScore = 0.0f;
        gameSpeed = 10.0f;

        GameObject.Find("PathSpawner").GetComponent<PathSpawner>().spawnEnabled = true;
        GameObject.Find("ObstacleSpawner").GetComponent<SpawnerScript>().spawnEnabled = true;

        gameSpeed = 10.0f;
    }

    public void onGameExit()
    {
        Debug.Log("Game exited");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_WEBGL || UNITY_WEBPLAYER
            Exit();
        #else
            Application.Quit();
        #endif
    }

    public void PauseGame() {
        if (gameStarted) {
            Debug.Log("Game paused");

            //show score and torch life
            var uiGameObject = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
            var mainMenu = uiGameObject.Q<VisualElement>("MainMenu");
            mainMenu.visible = true;
            var gamePlay = uiGameObject.Q<VisualElement>("GamePlay");
            gamePlay.visible = false;

            //rotate camera
            var camera = GameObject.Find("Main Camera");
            camera.GetComponent<Animator>().SetBool("gameStarted", false);
            camera.transform.rotation = new Quaternion(0, 0, 0, 0);

            gameStarted = false;
            gameLost = false;
        } else {
            Debug.Log("Game resumed");

            //show score and torch life
            var uiGameObject = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
            var mainMenu = uiGameObject.Q<VisualElement>("MainMenu");
            mainMenu.visible = false;
            var gamePlay = uiGameObject.Q<VisualElement>("GamePlay");
            gamePlay.visible = true;

            //rotate camera
            var camera = GameObject.Find("Main Camera");
            camera.GetComponent<Animator>().SetBool("gameStarted", true);
            camera.transform.rotation = new Quaternion(0, -5f, 0, 0);

            gameStarted = true;
            gameLost = false;
        }
    }

    private void toggleAbout()
    {
        var uiGameObject = GameObject.Find("UI").GetComponent<UIDocument>().rootVisualElement;
        var aboutMessage = uiGameObject.Q<VisualElement>("AboutButtonMessage");
        aboutMessage.visible = !aboutMessage.visible;
    }
}
