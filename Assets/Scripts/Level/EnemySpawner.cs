using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] path;

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Wave[] waves;

    private int waveIndex = 0;
    private int waveInnerRoundIndex = 0;
    private int amountIndex = 0;
    private float timer = 0f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (amountIndex < waves[waveIndex].spawnRounds[waveInnerRoundIndex].amount)
            {
                //spawn next enemy
                amountIndex++;
                EnemyMovement enemy = Instantiate(enemyPrefabs[(int)waves[waveIndex].spawnRounds[waveInnerRoundIndex].enemy], transform).GetComponent<EnemyMovement>();
                enemy.run(path, enemyPrefabs);
                timer = waves[waveIndex].spawnRounds[waveInnerRoundIndex].secondsAfterEnemy;
            }
            else
            {
                //round ended
                timer = waves[waveIndex].spawnRounds[waveInnerRoundIndex].secondsAfterRound;
                amountIndex = 0;
                waveInnerRoundIndex++;

                if (waveInnerRoundIndex == waves[waveIndex].spawnRounds.Length)
                {
                    //wave ended
                    waveInnerRoundIndex = 0;
                    enabled = false; //will start next wave in final version
                }
            }
        }

    }
}
