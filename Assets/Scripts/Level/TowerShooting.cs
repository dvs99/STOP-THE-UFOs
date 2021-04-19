using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float cooldown;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float range;
    [SerializeField] private Transform ShootingOrigin;
    [SerializeField] private EnemySpawner spawner;

    private HashSet<EnemyMovement> enemies;
    private Transform[] enemyPath;
    private EnemyMovement target;
    private float timeRemaining;

    void Start()
    {
        timeRemaining = cooldown;

        SphereCollider col = gameObject.AddComponent<SphereCollider>();
        col.radius = range;
        col.isTrigger = true;

        enemies = new HashSet<EnemyMovement>();
        enemyPath = spawner.path;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (enemies.Count > 0)
        {
            if (timeRemaining <= 0)
            {
                enemies.RemoveWhere(isNull);
                targetFirstEnemy();
                if (target != null)
                {
                    transform.LookAt(target.transform);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    GameObject instancedBullet = Instantiate(bullet, ShootingOrigin.position, ShootingOrigin.rotation);
                    instancedBullet.GetComponent<BulletMovement>().Move(bulletSpeed);
                    timeRemaining = cooldown;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
            if (e != null)
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

    private bool isNull(EnemyMovement e)
    {
        return e == null;
    }
}
