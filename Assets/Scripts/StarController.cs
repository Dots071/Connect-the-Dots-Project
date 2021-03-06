using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

//This script should be attached to all the stars that create the wanted shape.
public class StarController : MonoBehaviour
{
    [SerializeField] Color starColor;

    public int nextStarIndex1; 
    public int nextStarIndex2;

    public int starIndex;
    private Animator animator;
    private Renderer rend;


    private void Start()
    {
        GameManager.Instance.CorrectStarChosen.AddListener(HandleCorrectStarChosen);
        GameManager.Instance.WrongStarChosen.AddListener(HandleWrongStarChosen);
        GameManager.Instance.HintRequested.AddListener(HandleHintRequested);

        animator = gameObject.GetComponent<Animator>();
        rend = gameObject.GetComponent<Renderer>();


    }

    private void HandleHintRequested(int index1, int index2)
    {
        if (starIndex == index1 || starIndex == index2)
            StartCoroutine(FlashHalo(Color.green));
    }

    private void HandleWrongStarChosen(int index)
    {
        if (index == starIndex)
            // FlashRedHalo();
            StartCoroutine(FlashHalo(Color.red));
    }

    private void HandleCorrectStarChosen(int index)
    {
        AnimateClicked(index == starIndex);

    }

    private void OnMouseDown()
    {
        LevelManager.Instance.OnCorrectStarPressed(this);
    }


    public void AnimateClicked(bool state)
    {
        animator.SetBool("isClicked", state);
    }


    IEnumerator FlashHalo(Color color)
    {

        gameObject.transform.DOScale(0.4f, 1f);
        rend.material.DOColor(color, 1f);
        yield return new WaitForSeconds(2);

        gameObject.transform.DOScale(0.2f, 1f);

        rend.material.DOColor(starColor, 1f);

    }


}