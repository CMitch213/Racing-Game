using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BasicAi : MonoBehaviour
{
    [Header("--From Player--")]
    public CarScript carScript;
    public CarSelectMenu menu;

    [Space(15)]
    [Header("--AI Logic--")]
    public GameObject parent;
    public List<Transform> targets = new List<Transform>();
    public int currentTarget = 0;
    private Transform targetPositionTransform;
    float forwardAmount = 0f;
    float turnAmount = 0f;
    float brakeAmount = 0f;
    float driftT = 3.0f;

    private Vector3 targetPosition;

    private void Start()
    {
        // Add all of the children to the list of point
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            targets.Add(parent.transform.GetChild(i).transform);
        }
    }

    private void Update()
    {
        //Add to timer
        driftT += Time.deltaTime;
        //Set current target position
        targetPositionTransform = targets[currentTarget];
        targetPosition = targetPositionTransform.position;

        //Loop around list
        if (currentTarget > targets.Count)
        {
            currentTarget = 0;
        }

        SetTargetPosition(targetPositionTransform.position);

        //Check if target position is ahead of player,
        Vector3 dirToMovePosition = (targetPosition - transform.position).normalized;
        //If above 0 it is infront, if below it is behind
        float dot = Vector3.Dot(transform.forward, dirToMovePosition);
        //DIstance from target
        float distance = Vector3.Distance(transform.position, targetPosition);

        NextTarget(distance);
        Braking(distance);
        Debug.Log(distance);
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
        //Debug.Log(angleToSteer);

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

        //Ai Drifting (Random Chance)
        if (Mathf.Abs(angleToSteer) > 50)
        {
            int rnd = Random.Range(1, 5);
            
            //DRIFT
            if(rnd == 2)
            {
                driftT = 0.0f;
            }
        }
        if(driftT < 0.25f)
        {
            carScript.HandBrakeInput = 1.0f;
        }
        else
        {
            carScript.HandBrakeInput = 0.0f;
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

    public void NextTarget(float dist)
    {
        //If you're close enough select the next target in the array
        if (dist < 30f)
        {
            currentTarget++;
        }
    }

    public void Braking(float dist)
    {
        //If you should brake at the current turn
        if (targetPositionTransform.GetComponent<AITargetStats>().shouldBreak == true)
        {
            //Braking for if you're going slow
            if (carScript.kph < 60)
            {
                //Brake if youre going over 20 mph
                if (carScript.kph > 30f && dist < 200f)
                {
                    forwardAmount = 0f;
                    brakeAmount = 1f;
                }
                //Dont break if you're going slower than 20
                else
                {
                    forwardAmount = 1f;
                    brakeAmount = 0f;
                }
            }
            else if (carScript.kph > 60)
            {
                //Brake if youre going over 20 mph
                if (carScript.kph > 30f && dist < 500f)
                {
                    forwardAmount = 0f;
                    brakeAmount = 1f;
                }
                //Dont break if you're going slower than 20
                else
                {
                    forwardAmount = 1f;
                    brakeAmount = 0f;
                }
            }

        }
        else
        {
            forwardAmount = 1f;
            brakeAmount = 0f;
        }
    }
}
