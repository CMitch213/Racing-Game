using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Car Physics/Car Stats")]
public class CarStats : ScriptableObject
{
    [Header("Engine Stats")]
    public float acceleration;
    public float dragForce;
    public float brakePressure;     //0 (instant braking) - 1 (standard  braking)
    public float turnSpeed;
    public float maxSpeed;
    public float maxReverseSpeed;
    public AnimationCurve horsepowerCurve;          //Currently Unused
    public float motorTorque;
    public float brakeTorque;

    [Header("Vehicle Stats")]
    public float centreOfGravityOffsetf;

    [Header("Steering Stats")]
    public float steeringRange;
    public float steeringRangeAtMaxSpeed;

    [Header("General Stats")]
    public string carName;
    public string carBrand;
}
