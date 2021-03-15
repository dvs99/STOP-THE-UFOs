using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineTrigger : MonoBehaviour
{
    [SerializeField] private int outlineLayerNumber;
    [SerializeField] private GameObject target;
    private int originalLayerNumber;

    private void OnTriggerEnter(Collider other)
    {
        originalLayerNumber = target.layer;
        target.layer = outlineLayerNumber;
    }

    private void OnTriggerExit(Collider other)
    {
        target.layer = originalLayerNumber;
    }
}
