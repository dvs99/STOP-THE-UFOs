using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementForce = 1;
    [SerializeField] private Transform model;
    [SerializeField] private float rotationAnimationSpeed = 1;


    private Vector3 movement;
    private Rigidbody rb;

    private void Start()
    {
        movement = Vector2.zero;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //get and process the movement input
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (movement.magnitude > 1)
            movement = movement.normalized;

        //rotate the model to look foward
        Vector3 velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (velocity.magnitude > 0.01)
        {
            Vector3 modelFoward = new Vector3(model.forward.x, 0, model.forward.z);

            Vector3 rotationTargetDirection = Vector3.RotateTowards(modelFoward, velocity.normalized, Mathf.Deg2Rad * rotationAnimationSpeed * Time.deltaTime, float.PositiveInfinity);
            model.LookAt(model.position + rotationTargetDirection);
        }
    }

    private void FixedUpdate()
    {
        //apply movement to the rigidbody
        rb.AddForce(movement * movementForce);
    }

    public void Stop()
    {
        rb.Sleep();
        rb.velocity = Vector3.zero;
        movement = Vector3.zero;
    }

    //TODO: add foward model-> GetPositionInSeconds
}
