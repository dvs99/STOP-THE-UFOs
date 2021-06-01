using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int value;
    [SerializeField] private Enemy[] spawnOnDeath;
    [SerializeField] private float spreadOnDeath;
    public int Damage;


    private GameObject[] enemyPrefabs;
    private Transform[] path;

    public float EstimatedLifeLeft { get; set; } = 0f;
    public int PathIndex { get; private set;} = 0;
    private bool runnning;

    public void run(Transform[] path, GameObject[] enemyPrefabs, int index = -1)
    {
        this.path = path;
        this.enemyPrefabs = enemyPrefabs;

        if (path == null || path.Length == 0)
        {
            Destroy(gameObject);
            this.enabled = false;
        }
        else if (index == -1)
        {
            transform.position = path[PathIndex].position;
            PathIndex = 1;
        }
        else
            PathIndex = index;

        runnning = true;
    }

    private void FixedUpdate()
    {
        EstimatedLifeLeft -= Time.deltaTime;
        if (runnning)
        {
            if (Vector3.Distance(transform.position, path[PathIndex].position) < 0.001f)
            {
                //Got to waypoint
                if (PathIndex >= path.Length - 1)
                {
                    HPManager.Instance.recieveDamage(Damage);
                    KillNoSpawningNoMoney();
                }
                else
                {
                    transform.position = path[PathIndex].position;
                    PathIndex++;
                }
            }

            // Move to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, path[PathIndex].position, step);
        }
    }

    public Vector3 GetPositionInSeconds(float seconds)
    {
        float dist = seconds * speed;
        int auxPathIndex = PathIndex;
        Vector3 futurePos = transform.position;
        while (auxPathIndex < path.Length -1 && dist > Vector3.Distance(path[auxPathIndex].position, futurePos))
        {
            dist -= Vector3.Distance(path[auxPathIndex].position, futurePos);
            futurePos = path[auxPathIndex].position;
            auxPathIndex++;
        }
        futurePos = Vector3.MoveTowards(futurePos, path[auxPathIndex].position, dist);
        return futurePos;
    }

    public void Kill()
    {
        for (int i = 0; i < spawnOnDeath.Length; i++)
        {
            EnemyMovement enemy = Instantiate(enemyPrefabs[(int)spawnOnDeath[i]], transform.position, transform.rotation, transform.parent).GetComponent<EnemyMovement>();
            float dist = spreadOnDeath * i;
            int auxPathIndex = PathIndex - 1;
            while (auxPathIndex > 0 && dist > Vector3.Distance(path[auxPathIndex].position, enemy.transform.position))
            {
                dist -= Vector3.Distance(path[auxPathIndex].position, enemy.transform.position);
                enemy.transform.position = path[auxPathIndex].position;
                auxPathIndex--;
            }
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, path[auxPathIndex].position, dist);
            enemy.run(path, enemyPrefabs, auxPathIndex + 1);
        }

        MoneyManager.Instance.Earn(value);
        KillNoSpawningNoMoney();
    }

    public void KillNoSpawningNoMoney()
    {
        Destroy(gameObject);
    }
}
