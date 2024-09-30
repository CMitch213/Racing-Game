using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Car Physics/Car Stats")]
public class CarStats : ScriptableObject
{
    [Header("Engine Stats")]
    public float hp;
    public AnimationCurve horsepowerCurve;          //Currently Unused
    public float EngineTorque;
    public float BrakePower;
    public float topSpeed;

    [Header("Vehicle Stats")]
    public float centreOfGravityOffsetf;

    [Header("Steering Stats")]
    public float MaxSteeringAngle;

    [Header("General Stats")]
    public string carName;
    public string carBrand;

    [Header("Drive Train")]
    public bool AWD;
    public bool FWD;
    public bool RWD;

    //[Header("Code Stats")]
}
