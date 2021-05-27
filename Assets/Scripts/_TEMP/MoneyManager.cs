using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private int intialMoney;
    private int currentMoney;

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
        currentMoney = intialMoney;
        moneyText.text = "$" + currentMoney;
    }

    public void Pay(int amount)
    {
        currentMoney -= amount;
        moneyText.text = "$" + currentMoney;
    }

    public void Earn(int amount)
    {
        currentMoney += amount;
        moneyText.text = "$" + currentMoney;
    }

    public bool CanAfford(int amount)
    {
        return currentMoney >= amount;
    }
}
