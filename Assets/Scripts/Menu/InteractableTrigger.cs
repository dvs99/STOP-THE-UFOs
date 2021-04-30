using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTrigger : MonoBehaviour
{
    [SerializeField] private int outlineLayerNumber;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private Menu menuToLaunch;

    private int originalLayerNumber = 1;
    private PlayerInteraction player = null;

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerInteraction>();
        if (player != null)
        {
            player.ShowButtonTip();
            
            foreach (GameObject t in targets)
            {
                originalLayerNumber = t.layer;
                t.layer = outlineLayerNumber;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInteraction>() == player)
        {
            player.HideButtonTip();
            foreach (GameObject t in targets)
                t.layer = originalLayerNumber;
            player = null;
        }
    }

    private void Update()
    {
        if (player != null && Input.GetButtonDown("Submit"))
            player.LaunchMenu(menuToLaunch);

        if (player != null && Input.GetButtonDown("Cancel"))
            player.CloseMenus();
    }
}
