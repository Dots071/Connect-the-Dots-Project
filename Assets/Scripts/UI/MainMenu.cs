using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button howToPlayButton;
    [SerializeField] private Button quitGameButton;

    // Start is called before the first frame update
    void Start()
    {
        startGameButton.onClick.AddListener(HandleStartGameButtonClicked);
        howToPlayButton.onClick.AddListener(HandleHowToPlayButtonClicked);
        quitGameButton.onClick.AddListener(HandleQuitGameButtonClicked);
    }

    private void HandleQuitGameButtonClicked()
    {
        GameManager.Instance.QuitGame();
    }

    private void HandleHowToPlayButtonClicked()
    {
        UIManager.Instance.GoToHelpMenu();
    }

    private void HandleStartGameButtonClicked()
    {
        GameManager.Instance.StartGame();
    }
}
