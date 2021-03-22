using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 1)]
public class Wave : ScriptableObject
{
    [Serializable]
    public struct EnemyRound
    {
        public Enemy enemy;
        public int amount;
        public float secondsAfterEnemy;
        public float secondsAfterRound;
    }

    public EnemyRound[] spawnRounds;
}