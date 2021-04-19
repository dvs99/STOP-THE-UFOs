using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [HideInInspector] private float speed;

    private bool moving = false;

    public void Move(float speed)
    {
        this.speed = speed;
        moving = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moving)
                transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<EnemyMovement>() != null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Border"))
            Destroy(gameObject);
    }
}


