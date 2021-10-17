using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
    [SerializeField] private Button finishHelpButton;

    // Start is called before the first frame update
    void Start()
    {
        finishHelpButton.onClick.AddListener(HandleFinishHelpButton);
    }

    private void HandleFinishHelpButton()
    {
        UIManager.Instance.FinishHelpMenu();
    }
}
