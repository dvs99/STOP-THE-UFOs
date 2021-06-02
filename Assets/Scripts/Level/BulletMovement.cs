using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float explosionRange;
    [SerializeField] private GameObject explosionParticleEffect;
    private float speed;
    private ParticleSystem part;
    private bool triggered;

    private bool moving = false;

    private void Start()
    {
        if (explosionRange > 0)
            part = Instantiate(explosionParticleEffect, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
    }

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
        if (!triggered)
        {
            triggered = true;
            EnemyMovement enemy = other.transform.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.Kill();
                if (explosionRange > 0)
                {
                    part.transform.position = transform.position;
                    part.Play();
                    foreach (Collider col in Physics.OverlapSphere(transform.position, explosionRange))
                        col.GetComponent<EnemyMovement>()?.Kill();
                }
                Destroy(gameObject);
            }
            else if (other.CompareTag("Border"))
                Destroy(gameObject);
            else
                triggered = false;
        }
    }
}


