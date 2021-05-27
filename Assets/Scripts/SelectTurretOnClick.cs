using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectTurretOnClick : MonoBehaviour
{
    public TowerShooting turret;
    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            turret.Select();
    }
}
