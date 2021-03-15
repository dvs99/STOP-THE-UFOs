using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    private Quaternion rotation;

    private void Start()
    {
        rotation = transform.rotation;
    }
    void Update()
    {
        transform.rotation = rotation;
    }
}
