using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] path;

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Wave[] waves;
    [SerializeField] private Text waveText;
    [SerializeField] private Button nextWaveButton;

    private int waveIndex = -1;
    private int waveInnerRoundIndex = 0;
    private int amountIndex = 0;
    private float timer = 0f;


    private void Start()
    {
        waveText.text = "Wave  " + (waveIndex + 1);
        nextWaveButton.onClick.AddListener(() => onNextWaveClick());
        nextWaveButton.interactable = true;
    }


    void Update()
    {
        if (!nextWaveButton.interactable)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (waveInnerRoundIndex < waves[waveIndex].spawnRounds.Length && amountIndex < waves[waveIndex].spawnRounds[waveInnerRoundIndex].amount)
                {
                    //spawn next enemy
                    amountIndex++;
                    EnemyMovement enemy = Instantiate(enemyPrefabs[(int)waves[waveIndex].spawnRounds[waveInnerRoundIndex].enemy], transform).GetComponent<EnemyMovement>();
                    enemy.run(path, enemyPrefabs);
                    if (amountIndex < waves[waveIndex].spawnRounds[waveInnerRoundIndex].amount)
                        timer = waves[waveIndex].spawnRounds[waveInnerRoundIndex].secondsAfterEnemy;
                }
                else
                {
                    if (waveInnerRoundIndex == waves[waveIndex].spawnRounds.Length)
                    {
                        //wave ended
                        waveInnerRoundIndex = 0;
                        if (waveIndex < waves.Length - 1)
                        {
                            nextWaveButton.interactable = true;
                            timer = 0;
                        }
                        else
                        {
                            enabled = false;
                            //WIN
                        }
                    }
                    else
                    {
                        //round ended
                        timer = waves[waveIndex].spawnRounds[waveInnerRoundIndex].secondsAfterRound;
                        amountIndex = 0;
                        waveInnerRoundIndex++;
                    }

                }
            }
        }

    }

    public void onNextWaveClick()
    {
        waveIndex++;
        waveText.text = "Wave " + (waveIndex + 1);
        MoneyManager.Instance.Earn(waves[waveIndex].moneyValue);
        nextWaveButton.interactable = false;
    }
}

