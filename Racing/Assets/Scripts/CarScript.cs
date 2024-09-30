using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarScript : MonoBehaviour
{

    public CarStats car;
    public TMP_Text Speed_Text;
    public Rigidbody rb;
    float kph = 0;

    public List<WheelCollider> SteeringWheels;
    public List<WheelCollider> PoweredWheels;
    public List<WheelCollider> Wheels;
    public List<WheelCollider> FrontWheels;
    public List<WheelCollider> RearWheels;

    [SerializeField] private float ThrottleInput;
    [SerializeField] private float BrakeInput;
    [SerializeField] private float HandBrakeInput;
    [SerializeField] private float SteeringInput;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Input for Gamepad(s)
        Gamepad CurrentGamepad = Gamepad.current;
        if (CurrentGamepad == null) { Debug.Log("Reconnect Controller/Gamepad"); return; }

        //Use Gas
        ThrottleInput = CurrentGamepad.rightTrigger.ReadValue();
        foreach (WheelCollider PWheel in PoweredWheels)
        {
            PWheel.motorTorque = (ThrottleInput * car.EngineTorque * (car.hp / 20));
        }


        //Use Brakes
        BrakeInput = CurrentGamepad.leftTrigger.ReadValue();
        foreach (WheelCollider Wheel in Wheels)
        {
            Wheel.brakeTorque = BrakeInput * car.BrakePower * 10;
        }

        //Use handbrake
        HandBrakeInput = CurrentGamepad.aButton.ReadValue();
        if(HandBrakeInput == 1.0f)
        {
            foreach (WheelCollider RWheel in RearWheels)
            {
                //Lower Friction
                WheelFrictionCurve myWfc;
                myWfc = RWheel.sidewaysFriction;
                myWfc.extremumSlip = 0.01f;
                RWheel.forwardFriction = myWfc;

                //Apply Braking
                RWheel.brakeTorque = BrakeInput * car.BrakePower * 30;
            }
        }
        //Reset after using handbrake
        else
        {
            foreach (WheelCollider RWheel in RearWheels)
            {
                //Reset Friction
                WheelFrictionCurve myWfc;
                myWfc = RWheel.sidewaysFriction;
                myWfc.extremumSlip = 0.6f;
                RWheel.forwardFriction = myWfc;
            }
        }

        SteeringInput = CurrentGamepad.leftStick.ReadValue().x;
        foreach (WheelCollider SWheel in SteeringWheels)
        {
            SWheel.steerAngle = Mathf.Lerp(-car.MaxSteeringAngle, car.MaxSteeringAngle, SteeringInput + 0.5f);
        }

        //Limit Speed
        if (kph > car.topSpeed)
        {
            rb.velocity = rb.velocity.normalized * car.topSpeed;
        }

        //Spedometer
        kph = rb.velocity.magnitude;
        Speed_Text.text = ((int)(kph)).ToString();
    }
}
