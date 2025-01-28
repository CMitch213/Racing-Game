using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggresiveAI : MonoBehaviour
{
    //This AI is very aggresive and it's goal is to attack the player.
    [Header("--From Player--")]
    public CarScript carScript;
    public CarSelectMenu menu;

    [Space(15)]
    [Header("--AI Logic--")]
    public GameObject player;
    public GameObject carHolder;
    float forwardAmount = 0f;
    float turnAmount = 0f;
    float brakeAmount = 0f;
    [SerializeField] CarScript playerCarScript;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        player = menu.selectedCar;
        playerCarScript = player.GetComponentInChildren<CarScript>();

        //Check if player position is ahead of AI,
        Vector3 dirToMovePosition = (player.transform.position - transform.position).normalized;
        //If above 0 it is infront, if below it is behind
        float dot = Vector3.Dot(transform.forward, dirToMovePosition);
        //Distance from player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        //Find which way to turn
        // - if to the left, + if to the right
        float angleToSteer = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);


        //Put Car in Drive
        carScript.gearNum = 4;
        carScript.dashGear = "D";
        //Try and go just slightly higher than player speed

        if (carScript.kph <= playerCarScript.kph + 10f)
        {
            if (Mathf.Abs(angleToSteer) <= 90)
            {
                forwardAmount = 1.0f;
                brakeAmount = 0.0f;
                carScript.HandBrakeInput = 0.0f;
            }
            else
            {
                forwardAmount = 0.5f;
                brakeAmount = 0.0f;
                carScript.HandBrakeInput = 0.0f;
            }
        }
        //If player is aggresively slower than you
        else if (carScript.kph - 25f <= playerCarScript.kph)
        {
            forwardAmount = 0.0f;
            brakeAmount = 1.0f;
            //if player is MUCH slower
            if (carScript.kph - 45f <= playerCarScript.kph)
            {
                carScript.HandBrakeInput = 1.0f;
            }

        }
        //Kinda just vibe
        else
        {
            forwardAmount = 0.25f;
            brakeAmount = 0.0f;
            carScript.HandBrakeInput = 0.0f;
        }


        //Turn Right
        if (angleToSteer > 0)
        {
            turnAmount = 1f * (angleToSteer / 180) + 0.1f;
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

            playerCarScript = player.GetComponent<CarScript>();
        }
    }
}
