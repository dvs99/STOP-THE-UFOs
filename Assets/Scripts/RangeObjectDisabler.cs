using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RangeObjectDisabler : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && !EndGameManager.Instance.hasEnded())
        {
            SelectedTurretManager.Instance.Deselect();
            foreach (RangeObject range in FindObjectsOfType<RangeObject>())
                range.gameObject.SetActive(false);
        }
    }
}