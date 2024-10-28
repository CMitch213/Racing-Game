using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum drivetrainType
{
    FWD, RWD, AWD
}

[CreateAssetMenu(menuName = "Car Physics/Car Stats")]
public class CarStats : ScriptableObject
{
    [Header("Engine Stats")]
    public float hp;
    public AnimationCurve horsepowerCurve;          //Currently Unused
    public float EngineTorque;
    public float BrakePower;
    [Tooltip("Car top speed is in mp/h")]
    public float topSpeed;
    public float accelMult;

    [Header("Vehicle Stats")]
    public float centreOfGravityOffsetf;

    [Header("Steering Stats")]
    public float MaxSteeringAngle;

    [Header("General Stats")]
    public string carName;
    public string carBrand;

    [Header("Drive Train")]
    public drivetrainType carDrivetrain;
    [Header("Transmission")]
    public int Speed;
    public bool Auto;
    public bool Manual;

    //[Header("Code Stats")]
}
