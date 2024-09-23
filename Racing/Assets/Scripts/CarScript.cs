using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    [Header("Stats")]
    public float acceleration;    
    public float brakingForce;   
    public float turnSpeed;       
    public float maxSpeed;        

    private Rigidbody carRigidbody;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleSteering();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Vertical");
        Vector3 forwardForce = transform.forward * moveInput * acceleration;

        if (carRigidbody.velocity.magnitude < maxSpeed)
        {
            carRigidbody.AddForce(forwardForce, ForceMode.Acceleration);
        }

        // If you press space, brake
        if (Input.GetKeyDown(KeyCode.Space))
        {
            carRigidbody.drag = brakingForce;
        }
        if(carRigidbody.velocity.magnitude > 0.1f)
        {
            carRigidbody.drag = 0;
        }
    }

    private void HandleSteering()
    {
        float turnInput = Input.GetAxis("Horizontal");

        if (turnInput != 0)
        {
            float turn = turnInput * turnSpeed * Time.deltaTime;
            transform.Rotate(0, turn, 0);
        }
    }
}
