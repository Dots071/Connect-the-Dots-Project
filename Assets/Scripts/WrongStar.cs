using System.Collections;
using UnityEngine;
using DG.Tweening;


//This script should be attached to all the stars that are not a part of the desired shape.
public class WrongStar : MonoBehaviour
{
    [SerializeField] Color starColor;

    private Renderer rend;

    private void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
    }


    private void OnMouseDown()
    {
        StartCoroutine(FlashRedHalo());
        LevelManager.Instance.WrongStarPressed();
    }
    // Changes the halo of the star to red for 2 seconds.
    IEnumerator FlashRedHalo()
    {

        gameObject.transform.DOScale(0.4f, 1f);
        rend.material.DOColor(Color.red, 1f);
        yield return new WaitForSeconds(2);

        gameObject.transform.DOScale(0.2f, 1f);

        rend.material.DOColor(starColor, 1f);

    }
}
