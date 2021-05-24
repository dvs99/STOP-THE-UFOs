using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int value;

    private Transform[] path;
    public int PathIndex { get; private set;} = 0;

    void Start()
    {
        path = FindObjectOfType<EnemySpawner>().path; 

        if (path == null || path.Length == 0)
        {
            Destroy(gameObject);
            this.enabled = false;
        }
        else
            transform.position = path[PathIndex].position;
    }

    private void FixedUpdate()
    {

        if (Vector3.Distance(transform.position, path[PathIndex].position) < 0.001f)
        {
            //Got to waypoint
            if (PathIndex >= path.Length - 1)
            {
                //Got to the end of the level
                Destroy(gameObject);
            }
            else
                PathIndex++;
        }

        // Move to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, path[PathIndex].position, step);
    }

    public void Kill()
    {
        Destroy(gameObject);
        MoneyManager.Instance.Earn(value);
    }
}
