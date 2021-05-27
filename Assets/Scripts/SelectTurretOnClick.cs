using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTurretOnClick : MonoBehaviour
{
    public TowerShooting turret;
    private void OnMouseDown()
    {
        print("dasd");
        turret.Select();
    }
}
