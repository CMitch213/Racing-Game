using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{

    public CarStats car;
    public float speedFactor;
    public float moveInput;
    float timer;
    bool isBraking = false;

    WheelControl[] wheels;
    private Rigidbody carRigidbody;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        wheels = GetComponentsInChildren<WheelControl>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        carRigidbody.centerOfMass += Vector3.up * car.centreOfGravityOffsetf;
    }

    void Update()
    {
        HandleMovement();
        HandleSteering();

        // Calculate how close the car is to top speed
        // as a number from zero to one
        speedFactor = Mathf.InverseLerp(0, car.maxSpeed, carRigidbody.velocity.magnitude);
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Vertical");
        Vector3 forwardForce = transform.forward * moveInput * car.acceleration;

        if (moveInput < 0 && carRigidbody.velocity.magnitude < car.maxReverseSpeed && isBraking == false)
        {
            carRigidbody.AddForce(forwardForce / 2, ForceMode.Acceleration);
        }
        else if (carRigidbody.velocity.magnitude < car.maxSpeed && isBraking == false)
        {
            carRigidbody.AddForce(forwardForce, ForceMode.Acceleration);
        }

        // If you press space, brake
        if (Input.GetButton("Jump"))
        {
            moveInput = 0.0f;
            carRigidbody.drag = car.dragForce;
            carRigidbody.velocity *= 0.995f * car.brakePressure;
            Debug.Log("Braking!");
            isBraking = true;
            Debug.Log("Braking =" + isBraking);
        }
        else
        {
            isBraking = false;
        }
        if(carRigidbody.velocity.magnitude > 0.1f  && !Input.GetKeyDown(KeyCode.Space))
        {
            carRigidbody.drag = 0;
        }
    }

    private void HandleSteering()
    {
        float turnInput = Input.GetAxis("Horizontal");

        /*
        if (turnInput != 0)
        {
            float turn = turnInput * car.turnSpeed * Time.deltaTime;
            transform.Rotate(0, turn, 0);
        }
        */

        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(car.steeringRange, car.steeringRangeAtMaxSpeed, speedFactor);

        float currentMotorTorque = Mathf.Lerp(car.motorTorque, 0, speedFactor);

        bool isAccelerating = Mathf.Sign(moveInput) == Mathf.Sign(carRigidbody.velocity.magnitude);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = turnInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = moveInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(moveInput) * car.brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}
