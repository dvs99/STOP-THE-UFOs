using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    private int currentHP;
    [SerializeField] private Text HPText;
    [SerializeField] private int initialHP;

    public static HPManager Instance { get; private set; }

    private void Start()
    {
        currentHP = initialHP;
        HPText.text = currentHP + " HP";
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void recieveDamage(int points)
    {
        currentHP -= points;
        if (currentHP <= 0)
            currentHP = 0;
        HPText.text = currentHP + " HP";
        if (currentHP == 0)
            EndGameManager.Instance.End(false);
    }
}
