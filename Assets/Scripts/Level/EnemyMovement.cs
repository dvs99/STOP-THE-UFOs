using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Transform[] path;
    public int PathIndex { get; private set;} = 0;

    void Start()
    {
        path = GetComponentInParent<EnemySpawner>().path;

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
                print("end");
                Destroy(gameObject);
            }
            else
                PathIndex++;
        }

        // Move to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, path[PathIndex].position, step);
    }
}
