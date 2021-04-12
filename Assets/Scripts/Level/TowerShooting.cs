using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    [SerializeField] private float Cooldown;
    [SerializeField] private float Range;
    [SerializeField] private EnemySpawner Spawner;

    private HashSet<EnemyMovement> enemies;
    private Transform[] enemyPath;
    private EnemyMovement target;
    private float timeRemaining;

    void Start()
    {
        timeRemaining = Cooldown;

        SphereCollider col = gameObject.AddComponent<SphereCollider>();
        col.radius = Range;
        col.isTrigger = true;

        enemies = new HashSet<EnemyMovement>();
        enemyPath = Spawner.path;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (enemies.Count > 0)
        {
            if (timeRemaining <= 0)
            {
                targetFirstEnemy();
                transform.LookAt(target.transform);
                Debug.DrawLine(transform.position, target.transform.position, Color.red, 0.9f);
                timeRemaining = Cooldown;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("hi");
        EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
            enemies.Add(enemyMovement);
    }

    private void OnTriggerExit(Collider other)
    {
        enemies.Remove(other.GetComponent<EnemyMovement>());
    }

    private void targetFirstEnemy()
    {
        float firstDist = 0;
        int firstIndexInPath = 0;
        EnemyMovement firstEnemy = null;
        foreach (EnemyMovement e in enemies)
        {
            if (e.PathIndex > firstIndexInPath)
            {
                firstIndexInPath = e.PathIndex;
                firstEnemy = e;
                firstDist = Vector3.Distance(e.transform.position, enemyPath[firstIndexInPath].position);
            }
            else if (e.PathIndex == firstIndexInPath)
            {
                float dist = Vector3.Distance(e.transform.position, enemyPath[firstIndexInPath].position);
                if (dist < firstDist)
                {
                    firstEnemy = e;
                    firstDist = dist;
                }
            }
        }
        target = firstEnemy;
    }

}
