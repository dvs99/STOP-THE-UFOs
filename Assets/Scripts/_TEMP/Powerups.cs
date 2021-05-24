using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerups : MonoBehaviour
{
    [SerializeField] private int destroyUses;
    [SerializeField] private int speedUpUses;
    [SerializeField] private float speedUpMultiplier;
    [SerializeField] private float speedUpSeconds;

    [SerializeField] private Button destroyButton;
    [SerializeField] private Button speedUpButton;

    [SerializeField] private Text destroyText;
    [SerializeField] private Text speedUpText;

    [SerializeField] private TowerShooting[] spedTowers;

    private void Start()
    {
        destroyText.text = destroyUses.ToString();
        speedUpText.text = speedUpUses.ToString();
        destroyButton.onClick.AddListener(destroy);
        speedUpButton.onClick.AddListener(speedUp);
    }

    public void destroy()
    {
        foreach (EnemyMovement enemy in FindObjectsOfType<EnemyMovement>())
            enemy.Kill();
        destroyUses--;
        destroyText.text = destroyUses.ToString();
        if (destroyUses <= 0)
            destroyButton.interactable = false;                          
    }

    public void speedUp()
    {
        spedTowers = FindObjectsOfType<TowerShooting>();
        foreach (TowerShooting tower in spedTowers)
            if (tower != null)
                tower.MultiplySpeed(speedUpMultiplier);
        StartCoroutine(recoverSpeedAfterSeconds(speedUpSeconds));
        speedUpUses--;
        speedUpText.text = speedUpUses.ToString();
        if (speedUpUses <= 0)
            speedUpButton.interactable = false;
    }

    IEnumerator recoverSpeedAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        foreach (TowerShooting tower in spedTowers)
            if (tower!=null)
                tower.MultiplySpeed(1/speedUpMultiplier);
    }
}
