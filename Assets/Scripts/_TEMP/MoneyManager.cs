using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private int intialMoney;
    private int CurrentMoney;

    public static MoneyManager Instance {get; private set;}

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

private void Start()
    {
        CurrentMoney = intialMoney;
        moneyText.text = "$" + CurrentMoney;
    }

    public void Pay(int amount)
    {
        CurrentMoney -= amount;
        moneyText.text = "$" + CurrentMoney;
    }

    public void Earn(int amount)
    {
        CurrentMoney += amount;
        moneyText.text = "$" + CurrentMoney;
    }

    public bool CanAfford(int amount)
    {
        return CurrentMoney >= amount;
    }
}
