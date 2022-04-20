using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        POSTGAME
    }

   [SerializeField] GameObject[] systemPrefabs;

    public Events.EventGameState OnGameStateChanged;
    public Events.CorrectStarChosen CorrectStarChosen;
    public Events.WrongStarChosen WrongStarChosen;
    public Events.HintRequested HintRequested;

    // public Events.GameStarted GameStartedEvent;

    List<GameObject> instancedSystemPrefabs;
    List<AsyncOperation> loadOperations;
    GameState currentGameState = GameState.PREGAME;
    public GameState CurrentGameState
    {
        get { return currentGameState; }
        private set { currentGameState = value; }
    }

    public int currentLevelIndex = 0;
    

    public bool isGameRunning = false;



    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        instancedSystemPrefabs = new List<GameObject>();
        loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();

    }
/*
    private void Update()
    {
        if (currentGameState == GameManager.GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            TogglePause();
        }
    }*/


    public void LoadLevel(int levelIndex)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Error loading level." + levelIndex);
            return;
        }
        loadOperations.Add(ao);
        currentLevelIndex = levelIndex;
        ao.completed += OnLevelLoadComplete;
    }

    public void UnLoadLevel(int LevelIndex)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(LevelIndex);

        ao.completed += OnUnLevelLoadComplete;
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < instancedSystemPrefabs.Count; i++)
        {
            Destroy(instancedSystemPrefabs[i]);
        }

        instancedSystemPrefabs.Clear();
    }

    private void OnLevelLoadComplete(AsyncOperation ao)
    {
        if (loadOperations.Contains(ao))
        {
            loadOperations.Remove(ao);

            if (loadOperations.Count == 0)
            {
                UpdateGameState(GameState.RUNNING);
            }
        }

        Debug.Log("Level load complete!");
    }
    private void OnUnLevelLoadComplete(AsyncOperation ao)
    {
        Debug.Log("Level unloaded complete!");
    }



    public void UpdateGameState(GameState state)
    {
        GameState previousGameState = currentGameState;
        currentGameState = state;

        switch (currentGameState)
        {
            case GameState.PREGAME:
                if(currentLevelIndex > 0)
                {
                    UnLoadLevel(currentLevelIndex);
                }
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
               // mainCamera.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;

            case GameState.POSTGAME:
                Time.timeScale = 1.0f;
                break;

            default:
                break;
        }
        OnGameStateChanged.Invoke(currentGameState, previousGameState);
    }


    private void InstantiateSystemPrefabs()
    {
        GameObject instancedPrefab;
        for (int i = 0; i < systemPrefabs.Length; i++)
        {
            instancedPrefab = Instantiate(systemPrefabs[i]);
            instancedSystemPrefabs.Add(instancedPrefab);
        }
    }
    public void StartGame()
    {
        isGameRunning = true;
        LoadLevel(1);
    }

    public void GoToNextLevel()
    {
        UnLoadLevel(currentLevelIndex);
        currentLevelIndex++;
        Debug.Log("Will load level " + currentLevelIndex);
       LoadLevel(currentLevelIndex);
    }


    private void ResetCSVReport()
    {
        CSVManager.CreateReport();
    }

/*

    public void TogglePause()
    {
        UpdateGameState(currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }*/


    public void QuitGame()
    {
        Debug.Log("Game is quitting!");
        Application.Quit();
    }

}
