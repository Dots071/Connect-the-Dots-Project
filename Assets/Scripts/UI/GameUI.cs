using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] Button hintButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button yesButton;
    [SerializeField] Button cancelButton;
   // [SerializeField] Button finishButton;

    [SerializeField] GameObject preQuitPopup;

    private void Start()
    {
        hintButton.onClick.AddListener(HandleHintButtonClicked);
        quitButton.onClick.AddListener(HandleQuitButtonClicked);
        yesButton.onClick.AddListener(HandleYesButtonClicked);
        cancelButton.onClick.AddListener(HandleCancelButtonClicked);
      //  finishButton.onClick.AddListener(HandleFinishButtonClicked);
    }

/*    private void HandleFinishButtonClicked()
    {
        LevelManager.Instance.ClosedShapeCheck();
    }*/

    private void HandleCancelButtonClicked()
    {
        preQuitPopup.SetActive(false);
    }

    private void HandleYesButtonClicked()
    {
        preQuitPopup.SetActive(false);
        GameManager.Instance.UpdateGameState(GameManager.GameState.PREGAME);
    }

    private void HandleQuitButtonClicked()
    {
        preQuitPopup.SetActive(true);
    }

    private void HandleHintButtonClicked()
    {
        LevelManager.Instance.HintPressed();
    }
}
