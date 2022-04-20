using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject helpMenu;
    [SerializeField] private GameObject InGameUI;
    [SerializeField] private GameObject endLevelScreen;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
      //  GameManager.Instance.GameStartedEvent.AddListener(HandleGameStarted);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
         
        mainMenu.gameObject.SetActive(currentState == GameManager.GameState.PREGAME);
        endLevelScreen.gameObject.SetActive(currentState == GameManager.GameState.POSTGAME);
        InGameUI.SetActive(currentState == GameManager.GameState.RUNNING);


    }
/*
    private void HandleGameStarted()
    {
        mainMenu.SetActive(false);
    }*/

    public void GoToHelpMenu()
    {
        helpMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void FinishHelpMenu()
    {
        helpMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
