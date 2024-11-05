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
    bool grounded = true;
    float steerangle;

    public List<WheelCollider> SteeringWheels;
    public List<WheelCollider> PoweredWheels;
    public List<GameObject> WheelsGO;
    public List<WheelCollider> Wheels;
    public List<WheelCollider> FrontWheels;
    public List<WheelCollider> RearWheels;
    public int wheelsOnGround;

    [SerializeField] private float ThrottleInput;
    [SerializeField] private float BrakeInput;
    [SerializeField] private float HandBrakeInput;
    [SerializeField] private float SteeringInput;
    [SerializeField] private float ReverseInput;
    [SerializeField] private float dpadX;
    [SerializeField] private float dpadY;

    //Headlights
    [Header("Headlights")]
    public Light headlightL;
    public Light headlightR;
    bool headlightsEnabled = true;
    public float toggleTime = 1;
    public int headLightCycle = 1;
    [SerializeField] private float HeadlightToggle;

    //Shifting
    [Header("Shifting")]
    [SerializeField] private float ShiftUp;
    [SerializeField] private float ShiftDown;
    public int gearNum = 4;
    public string dashGear = "";
    public TMP_Text gearText;
    public float shiftTime = 1;

    //Particles
    [Header("Particles")]
    public ParticleSystem driftParticle;

    //Sounds
    [Header("Sound Effects")]
    public AudioSource horn;

    // Start is called before the first frame update
    void Start()
    {
        driftParticle.Pause();

        //On Run Setup Drivetrain Style
        switch (car.carDrivetrain) {
            case drivetrainType.FWD:
                foreach (WheelCollider FWheel in FrontWheels)
                {
                    PoweredWheels = FrontWheels;
                }

                break;

            case drivetrainType.RWD:
                foreach (WheelCollider RWheel in RearWheels)
                {
                    PoweredWheels = RearWheels;
                }

                break;

            case drivetrainType.AWD:
                foreach (WheelCollider Wheel in Wheels)
                {
                    PoweredWheels = Wheels;
                }

                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Input for Gamepad(s)
        Gamepad CurrentGamepad = Gamepad.current;
        if (CurrentGamepad == null) { Debug.Log("Reconnect Controller/Gamepad"); return; }

        //Use Gas
        ThrottleInput = CurrentGamepad.rightTrigger.ReadValue();
        if (dashGear == "D")
        {
            foreach (WheelCollider PWheel in PoweredWheels)
            {
                PWheel.motorTorque = (ThrottleInput * car.EngineTorque * (car.hp / (20 - car.accelMult)));
            }
        }

        shiftTime += Time.deltaTime;
        //Shift
        if (car.Auto)
        {
            ShiftUp = CurrentGamepad.rightShoulder.ReadValue();
            ShiftDown = CurrentGamepad.leftShoulder.ReadValue();

            // Automatic
            if (car.Auto)
            {
                //Shift Up
                if(ShiftUp == 1 && shiftTime >= 0.25f)
                {
                    gearNum++;
                    shiftTime = 0.0f;
                }
                //Shift Down
                if (ShiftDown == 1 && shiftTime >= 0.25f)
                {
                    gearNum--;
                    shiftTime = 0.0f;
                }

                //Set Dashboard display for each gear in PRNDL
                if (gearNum == 1)
                {
                    dashGear = "P";
                    gearText.text = dashGear;
                }
                else if (gearNum == 2)
                {
                    dashGear = "R";
                    gearText.text = dashGear;
                }
                else if (gearNum == 3)
                {
                    dashGear = "N";
                    gearText.text = dashGear;
                }
                else if (gearNum == 4)
                {
                    dashGear = "D";
                    gearText.text = dashGear;
                }

                //Don't let go below park or above drive
                if (gearNum < 1) { gearNum = 1; }
                if (gearNum > 4) { gearNum = 4; }
            }
        }


        //Use Brakes
        BrakeInput = CurrentGamepad.leftTrigger.ReadValue();
        foreach (WheelCollider Wheel in Wheels)
        {
            Wheel.brakeTorque = BrakeInput * car.BrakePower * 10;
        }
        if(BrakeInput == 1)
        {
            Debug.Log("Braking!");

            foreach (WheelCollider Wheel in Wheels)
            {
                //Reset Friction
                WheelFrictionCurve Wfc;
                Wfc = Wheel.forwardFriction;

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
                Wfc = Wheel.forwardFriction;

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
                myWfc.stiffness = 0.1f;
                //RWheel.forwardFriction = myWfc;
                RWheel.sidewaysFriction = myWfc;

                //Apply Braking
                RWheel.brakeTorque = BrakeInput * car.BrakePower * 50;
            }

            steerangle *= 2.0f;
            driftParticle.Play();
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
                Wfc.stiffness = 1f;

                //RWheel.forwardFriction = Wfc;
                RWheel.sidewaysFriction = Wfc;
            }

            steerangle = car.MaxSteeringAngle * (1 - (kph / car.topSpeed));
            driftParticle.Pause();
            driftParticle.Clear();
        }

        //Steer car
        SteeringInput = CurrentGamepad.leftStick.ReadValue().x;
        steerangle = (car.MaxSteeringAngle * (1 - (kph / car.topSpeed))) + 4;
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
        if (dashGear == "R")
        {
            foreach (WheelCollider PWheel in PoweredWheels)
            {
                PWheel.motorTorque = -(car.EngineTorque * (car.hp / (20 - car.accelMult))) * ThrottleInput;
            }
        }

        //Put Car in Park
        if(dashGear == "P")
        {
            foreach (WheelCollider Wheel in Wheels)
            {
                Wheel.brakeTorque = car.BrakePower * 500;
            }
        }

        //Put Car in Neutral
        if(dashGear == "N")
        {
            //Make it so the car can roll
            foreach (WheelCollider RWheel in RearWheels)
            {
                //Lower Friction
                WheelFrictionCurve myWfc;
                myWfc = RWheel.sidewaysFriction;
                myWfc.extremumSlip = 0.01f;
                myWfc.stiffness = 0.1f;
                RWheel.forwardFriction = myWfc;
                //RWheel.sidewaysFriction = myWfc;

                //Apply Braking
                RWheel.brakeTorque = BrakeInput * car.BrakePower * 50;
            }
        }
        if(dashGear != "N" && HandBrakeInput < 1.0f)
        {
            //Reset Friction
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
                Wfc.stiffness = 1f;

                RWheel.forwardFriction = Wfc;
                RWheel.sidewaysFriction = Wfc;
            }
        }

        
        toggleTime += Time.deltaTime;
        //Toggle Headlights
        HeadlightToggle = CurrentGamepad.buttonNorth.ReadValue();
        if (HeadlightToggle == 1 && toggleTime >= 0.4f)
        {
            headLightCycle++;
            //Loop back around
            if (headLightCycle > 2) { headLightCycle = 0; }

            //Headlights off
            if(headLightCycle == 0)
            {
                headlightL.intensity = 0;
                headlightR.intensity = 0;
                headlightsEnabled = false;
                toggleTime = 0f;

                // Rumble the  low-frequency (left) motor at 1/4 speed and the high-frequency
                // (right) motor at 3/4 speed.
                Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
                InputSystem.ResetHaptics();
            }
            //Normal
            else if(headLightCycle == 1)
            {
                headlightL.intensity = 2;
                headlightL.range = 20;
                headlightR.intensity = 2;
                headlightR.range = 20;
                headlightsEnabled = true;
                toggleTime = 0f;

                // Rumble the  low-frequency (left) motor at 1/4 speed and the high-frequency
                // (right) motor at 3/4 speed.
                Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
                InputSystem.ResetHaptics();
            }
            //High Beams
            else if (headLightCycle == 2)
            {
                headlightL.intensity = 4;
                headlightL.range = 60;
                headlightR.intensity = 4;
                headlightR.range = 60;
                headlightsEnabled = true;
                toggleTime = 0f;

                // Rumble the  low-frequency (left) motor at 1/4 speed and the high-frequency
                // (right) motor at 3/4 speed.
                Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
                InputSystem.ResetHaptics();
            }
        }

        //D-Pad Input
        dpadX = Gamepad.current.dpad.x.ReadValue();
        dpadY = Gamepad.current.dpad.y.ReadValue();
        if (dpadX > 0f)
        {
            // player currently holds right
        }
        if (dpadX < 0f)
        {
            // player currently holds left
        }
        if (dpadY > 0f)
        {
            // player currently holds up
            horn.Play();
        }
        if (dpadY < 0f)
        {
            // player currently holds down
        }

        //Spedometer
        kph = rb.velocity.magnitude;
        Speed_Text.text = ((int)(kph)).ToString();

        //Check if 2/4 of the wheels are off the ground
        foreach (GameObject Wheel in WheelsGO)
        {
            WheelControl wc;
            wc = Wheel.GetComponent<WheelControl>();
            if (wc.grounded)
            {
                wheelsOnGround += 1;
            }
        }
        //Debug.Log(wheelsOnGround);

        //If mostly in the air
        if (wheelsOnGround < 3)
        {
            //SteeringInput = CurrentGamepad.leftStick.ReadValue().x;
            //transform.localRotation.x
        }

    }
}
