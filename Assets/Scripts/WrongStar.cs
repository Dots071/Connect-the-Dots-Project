using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

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

        /*       SerializedObject halo = new SerializedObject(gameObject.GetComponent("Halo"));
               halo.FindProperty("m_Size").floatValue = 0.3f;
               halo.FindProperty("m_Color").colorValue = Color.red;
               halo.ApplyModifiedProperties();

               yield return new WaitForSeconds(1);
               halo.FindProperty("m_Size").floatValue = 0.2f;
               halo.FindProperty("m_Enabled").boolValue = true;
               halo.FindProperty("m_Color").colorValue = starColor;
               halo.ApplyModifiedProperties();*/
    }
}
