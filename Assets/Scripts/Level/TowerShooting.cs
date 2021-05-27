using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShooting : MonoBehaviour
{
    public float Cooldown;
    public float BulletSpeed;
    public float Range;
    public int Price;

    [SerializeField] private float RangeMargin;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform ShootingOrigin;
    [SerializeField] private Transform ShootingOriginAlt;
    [SerializeField] private EnemySpawner spawner;

    private HashSet<EnemyMovement> enemies;
    private Transform[] enemyPath;
    private GameObject rangeObject;
    private EnemyMovement target;
    private float timeRemaining;
    private bool shootwithAlt = false;

    public void StartRunning(EnemySpawner spawner, GameObject rangeObject)
    {
        timeRemaining = 0;

        SphereCollider col = gameObject.AddComponent<SphereCollider>();
        col.radius = Range/transform.localScale.x;
        col.isTrigger = true;

        enemies = new HashSet<EnemyMovement>();
        enemyPath = spawner.path;

        foreach (Collider c in Physics.OverlapSphere(transform.position, Range))
        {
            EnemyMovement enemyMovement = c.gameObject.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
                enemies.Add(enemyMovement);
        }

        this.rangeObject = rangeObject;
        this.rangeObject.SetActive(false);
    }

    void Update()
    {
        if (enemyPath != null)
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
                        Transform shootFrom = ShootingOrigin;
                        if (shootwithAlt)
                            shootFrom = ShootingOriginAlt;

                        float aproxTimeToHit = Vector3.Distance(shootFrom.position, target.transform.position) / BulletSpeed;
                        Vector3 futurePos = target.GetPositionInSeconds(aproxTimeToHit);
                        if (Vector3.Distance(transform.position, futurePos) < Range + RangeMargin)
                        {
                            transform.LookAt(futurePos);
                            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                            GameObject instancedBullet = Instantiate(bullet, shootFrom.position, shootFrom.rotation);
                            instancedBullet.GetComponent<BulletMovement>().Move(BulletSpeed);
                            target.EstimatedLifeLeft = aproxTimeToHit;
                            timeRemaining = Cooldown;
                            if (ShootingOriginAlt != null)
                                shootwithAlt = !shootwithAlt;
                        }
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enemyPath != null)
        {
            EnemyMovement enemyMovement = other.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
                enemies.Add(enemyMovement);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemyPath != null && enemies.Contains(other.GetComponent<EnemyMovement>()))
            enemies.Remove(other.GetComponent<EnemyMovement>());
    }

    private void targetFirstEnemy()
    {
        float firstDist = 0;
        int firstIndexInPath = 0;
        EnemyMovement firstEnemy = null;
        foreach (EnemyMovement e in enemies)
        {
            if (e != null && e.EstimatedLifeLeft<-0.1f) //if EstimatedLifeLeft is positive the enemy is already targeted
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

    public void MultiplySpeed(float times)
    {
        Cooldown /= times;
        BulletSpeed *= times;
    }

    public void Select()
    {
        if (!EndGameManager.Instance.hasEnded())
        {
            foreach (RangeObject range in FindObjectsOfType<RangeObject>())
                range.gameObject.SetActive(false);
            rangeObject.SetActive(true);
        }
    }
}
