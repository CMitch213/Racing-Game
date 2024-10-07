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
    [SerializeField] private float ReverseInput;


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
        if(BrakeInput == 1)
        {
            foreach (WheelCollider Wheel in Wheels)
            {
                //Reset Friction
                WheelFrictionCurve Wfc;
                Wfc = Wheel.sidewaysFriction;

                //Reset Values
                Wfc.extremumSlip = 0.4f;
                Wfc.extremumValue = 1.5f;
                Wfc.asymptoteSlip = 0.6f;
                Wfc.asymptoteValue = 1f;

                Wheel.forwardFriction = Wfc;
            }
        }
        else
        {
            foreach (WheelCollider Wheel in Wheels)
            {
                //Reset Friction
                WheelFrictionCurve Wfc;
                Wfc = Wheel.sidewaysFriction;

                //Reset Values
                Wfc.extremumSlip = 0.4f;
                Wfc.extremumValue = 3.0f;
                Wfc.asymptoteSlip = 0.6f;
                Wfc.asymptoteValue = 2f;

                Wheel.forwardFriction = Wfc;
            }
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
                WheelFrictionCurve Wfc;
                Wfc = RWheel.sidewaysFriction;

                //Reset Values
                Wfc.extremumSlip = 0.4f;
                Wfc.extremumValue = 3.0f;
                Wfc.asymptoteSlip = 0.6f;
                Wfc.asymptoteValue = 2f;

                RWheel.forwardFriction = Wfc;
            }
        }

        SteeringInput = CurrentGamepad.leftStick.ReadValue().x;
        float steerangle;
        steerangle = car.MaxSteeringAngle * (1 - (kph / car.topSpeed));
        foreach (WheelCollider SWheel in SteeringWheels)
        {
            SWheel.steerAngle = Mathf.Lerp(-steerangle, steerangle, SteeringInput + 0.5f);
        }

        //Limit Speed
        if (kph > car.topSpeed)
        {
            rb.velocity = rb.velocity.normalized * car.topSpeed;
        }

        //Reverse
        ReverseInput = CurrentGamepad.bButton.ReadValue();
        if (ReverseInput == 1.0f)
        {
            foreach (WheelCollider PWheel in PoweredWheels)
            {
                PWheel.motorTorque = -(car.EngineTorque * (car.hp / 20));
            }
        }

        //Spedometer
        kph = rb.velocity.magnitude;
        Speed_Text.text = ((int)(kph)).ToString();
    }
}
