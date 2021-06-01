using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Canvas settingsMenu;
    [SerializeField] private Canvas quitMenu;
    [SerializeField] private Canvas levelMenu;
    [SerializeField] private Canvas buttonTip;

    private PlayerMovement movement;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    public void ShowButtonTip()
    {
        buttonTip.enabled = true;
    }

    public void HideButtonTip()
    {
        buttonTip.enabled = false;
    }

    public void LaunchMenu(Menu menu)
    {
        if (menu == Menu.Settings)
            settingsMenu.enabled = true;
        else if (menu == Menu.Quit)
            quitMenu.enabled = true;
        else if (menu == Menu.Level)
            levelMenu.enabled = true;

        movement.Stop();
        movement.enabled = false;
    }

    public void CloseMenus()
    {
        quitMenu.enabled = false;
        levelMenu.enabled = false;
        settingsMenu.enabled = false;

        movement.enabled = true;
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
