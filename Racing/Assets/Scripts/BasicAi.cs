using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAi : MonoBehaviour
{
    [Header("--From Player--")]
    public CarScript carScript;
    public CarSelectMenu menu;

    [Space(15)]
    [Header("--AI Logic--")]
    [SerializeField] private Transform targetPositionTransform;
    float forwardAmount = 0f;
    float turnAmount = 0f;
    float brakeAmount = 0f;

    private Vector3 targetPosition;

    private void Start()
    {
        
    }

    private void Update()
    {
        SetTargetPosition(targetPositionTransform.position);

        //Check if target position is ahead of player,
        Vector3 dirToMovePosition = (targetPosition - transform.position).normalized;
        //If above 0 it is infront, if below it is behind
        float dot = Vector3.Dot(transform.forward, dirToMovePosition);
        //DIstance from target
        float distance = Vector3.Distance(transform.position, targetPosition);

        //Drive forward
        if (dot > 0f)
        {
            //Put Car in Drive
            carScript.gearNum = 4;
            carScript.dashGear = "D";
            //Floor it
            forwardAmount = 1f;
        }
        //Reverse
        else if(dot < 0f)
        {
            //Check if too far to reverse
            if(distance >= 25f)
            {
                //Put Car in Drive
                carScript.gearNum = 4;
                carScript.dashGear = "D";
                //Floor it
                forwardAmount = 1f;
            }
            //Can reverse distance
            else
            {
                //Put Car in Reverse
                carScript.gearNum = 2;
                carScript.dashGear = "R";
                forwardAmount = 0.5f;
            }
        }

        //Find which way to turn
        // - if to the left, + if to the right
        float angleToSteer = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);
        Debug.Log(angleToSteer);

        //Turn Right
        if (angleToSteer > 0)
        {
            turnAmount = 1f * (angleToSteer/180) + 0.1f;
        }
        //Turn Left
        else if (angleToSteer < 0)
        {
            turnAmount = 1f * (angleToSteer / 180) - 0.1f;
        }

        //only run the AI if the player has selected a car.
        if (menu.gameHasStarted)
        {
            carScript.ThrottleInput = forwardAmount;
            carScript.SteeringInput = turnAmount;
            carScript.BrakeInput = brakeAmount;
        }
    }

    public void SetTargetPosition(Vector3 target_Position)
    {
        this.targetPosition = target_Position;
    }
}
