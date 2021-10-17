using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreLevelMessage : MonoBehaviour
{
    [SerializeField] Button okButton;
    [SerializeField] GameObject environment;


    private void Start()
    {
        okButton.onClick.AddListener(HandleOkButtonPressed);
    }

    private void HandleOkButtonPressed()
    {
        environment.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
