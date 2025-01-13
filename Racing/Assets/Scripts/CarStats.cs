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
    [ContextMenuItem("Reset all Stats", "ZeroOutAll")]      //Right click on hp and select 'Reset all stats' to reset all of the stats.
    public float hp;
    [Tooltip("Currently Unused")]
    public AnimationCurve horsepowerCurve;          //Currently Unused
    public float EngineTorque;
    [Tooltip("Usually very high in the thousands")]
    public float BrakePower;
    public int idleRPM;
    public int maxRPM;
    [Tooltip("Car top speed is in mp/h")]
    public float topSpeed;
    [Tooltip("Can not go above 20 or below 0")]
    public float accelMult;

    [Space(25)]

    [Header("Vehicle Stats")]
    [Tooltip("Usually -7 to 0")]
    public float centreOfGravityOffsetf;
    public int dampenRate;

    [Space(25)]

    [Header("Steering Stats")]
    public float MaxSteeringAngle;

    [Space(25)]

    [Header("General Stats")]
    public string carName;
    public string carBrand;

    [Space(25)]

    [Header("Drive Train")]
    public drivetrainType carDrivetrain;

    [Space(25)]

    [Header("Transmission")]
    [Tooltip("Speed referring to how many gears it has")]
    public int Speed;
    public bool Auto;
    public bool Manual;
    [Tooltip("Only matters for manual cars")]
    public float gearRat1, gearRat2, gearRat3, gearRat4, gearRat5, gearRat6;

    public void ZeroOutAll()
    {
        hp = 0;
        EngineTorque = 0;
        BrakePower = 0;
        idleRPM = 0;
        maxRPM = 0;
        topSpeed = 0;
        accelMult = 0;
        centreOfGravityOffsetf = 0;
        MaxSteeringAngle = 0;
        Speed = 0;
        Auto = false;
        Manual = false;
        carName = "";
        carBrand = "";
    }

    //[Header("Code Stats")]
}
