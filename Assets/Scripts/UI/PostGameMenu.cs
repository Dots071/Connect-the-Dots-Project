using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PostGameMenu : MonoBehaviour
{
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button nextLevelButton;


    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(HandleMainMenuButtonClicked);
        nextLevelButton.onClick.AddListener(HandleNextLevelButtonClicked);
    }

    private void OnEnable()
    {
        nextLevelButton.gameObject.SetActive(!LevelManager.Instance.isLastLevel);
    }

    private void HandleNextLevelButtonClicked()
    {
        GameManager.Instance.GoToNextLevel();
    }

    private void HandleMainMenuButtonClicked()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.PREGAME);
    }
}
