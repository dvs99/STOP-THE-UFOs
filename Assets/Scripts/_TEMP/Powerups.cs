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

    [SerializeField] private GameObject speedUpParticleEffect;
    [SerializeField] private GameObject destroyParticleEffect;

    [SerializeField] private AudioSource audioSourceDestroy;

    private TowerShooting[] spedTowers;
    private Queue<GameObject> particleEffects = new Queue<GameObject>();

    private void Start()
    {
        destroyText.text = destroyUses.ToString();
        speedUpText.text = speedUpUses.ToString();
        destroyButton.onClick.AddListener(destroy);
        speedUpButton.onClick.AddListener(speedUp);
    }

    public void destroy()
    {
        audioSourceDestroy.Play();
        Instantiate(destroyParticleEffect);
        foreach (EnemyMovement enemy in FindObjectsOfType<EnemyMovement>())
            enemy.KillNoSpawningNoMoney();
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
            {
                tower.MultiplySpeed(speedUpMultiplier);
                particleEffects.Enqueue(Instantiate(speedUpParticleEffect, tower.transform.position, Quaternion.identity));
            }
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
            if (tower != null)
            {
                if (tower.UpgradePrice != -1)
                    tower.MultiplySpeed(1 / speedUpMultiplier);
                else
                    tower.ResetUpgradedSpeed();
            }

        while (particleEffects.Count > 0)
            Destroy(particleEffects.Dequeue());
    }
}
