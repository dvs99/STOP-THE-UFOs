using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    private float speed;

    private bool moving = false;

    public void Move(float speed)
    {
        this.speed = Mathf.Min(maxSpeed,speed);
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
        EnemyMovement enemy = other.transform.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            enemy.Kill();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Border"))
            Destroy(gameObject);
    }
}


