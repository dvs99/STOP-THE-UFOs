using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Transform[] path;
    private int pathIndex = 0;

    void Start()
    {
        path = GetComponentInParent<EnemySpawner>().path;

        if (path == null || path.Length == 0)
        {
            Destroy(gameObject);
            this.enabled = false;
        }
        else
            transform.position = path[pathIndex].position;
    }

    private void FixedUpdate()
    {
        // Check if the position of the cube and sphere are approximately equal.
        if (path == null || path.Length == 0 || Vector3.Distance(transform.position, path[pathIndex].position) < 0.001f)
        {
            if (pathIndex >= path.Length - 1)
            {
                print("end");
                Destroy(gameObject);
            }
            else
                pathIndex++;
        }

        // Move to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, path[pathIndex].position, step);
    }
}
