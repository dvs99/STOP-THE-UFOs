using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedTurretManager : MonoBehaviour
{
    private TowerShooting current = null;
    [SerializeField] private Text focusText;
    [SerializeField] private Text priceText;
    [SerializeField] private GameObject priceTag;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject menu;

    public static SelectedTurretManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void Select(TowerShooting turret)
    {
        current = turret;
        menu.SetActive(true);

        if (current.Focus == TurretFocus.First)
            focusText.text = "First";
        else if (current.Focus == TurretFocus.Strongest)
            focusText.text = "Strongest";
        else if (current.Focus == TurretFocus.Last)
            focusText.text = "Last";
        priceText.text = "$" + current.UpgradePrice.ToString();
    }

    public void Deselect()
    {
        current = null;
        menu.SetActive(false);
    }

    public void SetNextFocus()
    {
        if (current.Focus == TurretFocus.First)
        {
            current.Focus = TurretFocus.Strongest;
            focusText.text = "Strongest";
        }
        else if (current.Focus == TurretFocus.Strongest)
        {
            current.Focus = TurretFocus.Last;
            focusText.text = "Last";
        }
        else if (current.Focus == TurretFocus.Last)
        {
            current.Focus = TurretFocus.First;
            focusText.text = "First";
        }
    }

    public void SetPrevFocus()
    {
        if (current.Focus == TurretFocus.Last)
        {
            current.Focus = TurretFocus.Strongest;
            focusText.text = "Strongest";
        }
        else if (current.Focus == TurretFocus.First)
        {
            current.Focus = TurretFocus.Last;
            focusText.text = "Last";
        }
        else if (current.Focus == TurretFocus.Strongest)
        {
            current.Focus = TurretFocus.First;
            focusText.text = "First";
        }
    }

    public void Upgrade()
    {
        current.Upgrade();
    }

    private void Update()
    {
        if (current != null)
        {
            if (current.UpgradePrice >= 0 && MoneyManager.Instance.CanAfford(current.UpgradePrice))
            {
                upgradeButton.interactable = true;
                priceTag.SetActive(true);
            }
            else if (current.UpgradePrice >= 0)
            {
                priceTag.SetActive(true);
                upgradeButton.interactable = false;
            }
            else
            {
                priceTag.SetActive(false);
                upgradeButton.interactable = false;
            }
        }
    }
}
