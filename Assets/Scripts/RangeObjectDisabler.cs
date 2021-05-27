using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RangeObjectDisabler : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!EndGameManager.Instance.hasEnded())
        {
            foreach (RangeObject range in FindObjectsOfType<RangeObject>())
                range.gameObject.SetActive(false);
        }
    }
}