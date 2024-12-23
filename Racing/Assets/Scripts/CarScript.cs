using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CarScript : MonoBehaviour
{
    [Header("Car and UI")]
    public CarStats car;
    public TMP_Text Speed_Text;
    public Rigidbody rb;
    float kph = 0;
    bool grounded = true;
    float steerangle;

    [Space(10)]

    [Header("Wheels")]
    public List<WheelCollider> SteeringWheels;
    public List<WheelCollider> PoweredWheels;
    public List<GameObject> WheelsGO;
    public List<WheelCollider> Wheels;
    public List<WheelCollider> FrontWheels;
    public List<WheelCollider> RearWheels;
    public int wheelsOnGround;

    [Space(10)]

    [Header("Inputs")]
    [SerializeField] private float ThrottleInput;
    [SerializeField] private float BrakeInput;
    [SerializeField] private float HandBrakeInput;
    [SerializeField] private float SteeringInput;
    [SerializeField] private float ReverseInput;
    [SerializeField] private float dpadX;
    [SerializeField] private float dpadY;
    private float cruiseT;

    [Space(10)]

    //Headlights
    [Header("Headlights")]
    public Light headlightL;
    public Light headlightR;
    bool headlightsEnabled = true;
    public float toggleTime = 1;
    public int headLightCycle = 1;
    [SerializeField] private float HeadlightToggle;

    [Space(10)]

    //Shifting
    [Header("Shifting")]
    [SerializeField] private float ShiftUp;
    [SerializeField] private float ShiftDown;
    public int gearNum = 4;
    public string dashGear = "";
    public TMP_Text gearText;
    public float shiftTime = 1;

    [Space(10)]

    //Particles
    [Header("Particles")]
    public ParticleSystem driftParticle;
    public TrailRenderer[] skidMarks;

    [Space(10)]

    //Sounds
    [Header("Audio Effects")]
    public AudioSource carSound;
    public AudioSource horn;
    public AudioSource handbrakeSound;
    public AudioClip handbrakeClip;
    public AudioSource headlightSound;
    public AudioSource startSound;

    [Space(10)]

    [Header("Misc")]
    public bool CruiseOn;
    public GameObject cruiseUI;
    public float rpm = 0;
    public int gear = 1;
    public TMP_Text transmissionText;
    public GameObject rpmNeedle;
    public float Pitch = 0.05f;
    bool carIsOn = false;
    public GameObject carOnUI;
    bool isHandBraking = false;
    public GameObject exhaust;

    // Calculate RPM and Do Transmission/Gear Changes
    void EngineRPM()
    {
        // Calculate RPM
        // Check that you're actually in a gear
        if (gear >= 1 && dashGear != "N")
        {
            rpm = (kph * (90 / gear * 2)) + car.idleRPM;
        }
        //Let you rev bomb in Neutral
        if(dashGear == "N")
        {
            //rpm = (ThrottleInput * car.maxRPM) + car.idleRPM;
            // Rev that ho in Neutral
            if (ThrottleInput > 0.0f)
            {
                rpm = Mathf.Lerp(rpm, (ThrottleInput * car.maxRPM + (kph * 20)) + car.idleRPM, 50000.0f);
            }
            // Slowly go back to Idle
            else
            {
                rpm = Mathf.Lerp(rpm, car.idleRPM, 50000.0f);
            }
            
        }

        //Make Engine Sound Follow RPM
        Pitch = 0.7f + (rpm / car.maxRPM) * 2;
        if (Pitch <= 0.7f)
        {
            Pitch = 0.7f;
        }

        //Set max and min rpm
        if (rpm >= car.maxRPM)
        {
            rpm = car.maxRPM;
        }
        else if (rpm <= car.idleRPM)
        {
            rpm = car.idleRPM;
        }

        //Automatic Transmission 
        if (car.Auto && dashGear != "N" && dashGear != "P")
        {
            // Upshift after a specfic rpm
            if(rpm / car.maxRPM >= 0.35 && gear < car.Speed)
            {
                gear++;
                rpm /= 1.7f;
            }
            // Downshift at a specific rpm
            if (rpm / car.maxRPM <= 0.28 && gear > 1)
            {
                gear--;
                rpm *= 1.7f;
            }
        }

        // Display what gear you currently are in
        transmissionText.text = gear.ToString();

        // Don't shift past your maximum gear, duh
        if(gear > car.Speed)
        {
            gear = car.Speed;
        }

        //Speedometer
        // Move it according to your max and min rpm (it says 0-8k in UI but thats bullshit)
        rpmNeedle.transform.localEulerAngles = new Vector3(0, 0, (rpm / car.maxRPM * 260.0f - 130.0f) * -1.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Reset Values
        driftParticle.Pause();
        gear = 1;
        Pitch = 0.05f;

        //On Run Setup Drivetrain Style
        switch (car.carDrivetrain) {
            //If you're in a FWD
            case drivetrainType.FWD:
                foreach (WheelCollider FWheel in FrontWheels)
                {
                    PoweredWheels = FrontWheels;
                }

                break;

            //If you're in a RWD
            case drivetrainType.RWD:
                foreach (WheelCollider RWheel in RearWheels)
                {
                    PoweredWheels = RearWheels;
                }

                break;

            //If you're in a AWD
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
        //Get Input
        Gamepad CurrentGamepad = Gamepad.current;
        if (CurrentGamepad == null) { Debug.Log("Reconnect Controller/Gamepad"); return; }

        //If the car is running
        if (carIsOn)
        {
            // Turn on the exhaust fumes
            exhaust.SetActive(true);
            // Make it so it doesnt ask you to turn on the car
            carOnUI.SetActive(false);

            //Calculate RPM
            EngineRPM();
            carSound.pitch = Pitch;

            //Use Gas
            ThrottleInput = CurrentGamepad.rightTrigger.ReadValue();
            if (dashGear == "D")
            {
                foreach (WheelCollider PWheel in PoweredWheels)
                {
                    PWheel.motorTorque = (ThrottleInput * car.EngineTorque * (car.hp / (20 - car.accelMult + (gear * 2))));
                }
            }
            //Slowly remove speed if not pressing gas
            if (ThrottleInput == 0 && CruiseOn == false)
            {
                rb.drag = 0.15f;
            }
            // If cruise control is on or if you're accelerating do not slowly remove speed
            else
            {
                rb.drag = 0f;
            }

            //Cruise Control
            if (CruiseOn)
            {
                // Enable cruise control UI
                cruiseUI.SetActive(true);
            }
            else
            {
                // Disable cruise control UI
                cruiseUI.SetActive(false);
            }

            //Shifting Time
            shiftTime += Time.deltaTime;
            //Shift
            if (car.Auto)
            {
                //Inputs
                ShiftUp = CurrentGamepad.rightShoulder.ReadValue();
                ShiftDown = CurrentGamepad.leftShoulder.ReadValue();

                // Automatic
                if (car.Auto)
                {
                    //Shift Up
                    if (ShiftUp == 1 && shiftTime >= 0.25f)
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
                    PWheel.motorTorque = -(car.EngineTorque * (car.hp / (20 - car.accelMult + (gear * 4)))) * ThrottleInput;
                }
            }

            //Put Car in Park
            if (dashGear == "P")
            {
                foreach (WheelCollider Wheel in Wheels)
                {
                    Wheel.brakeTorque = car.BrakePower * 500;
                }
            }

            //Put Car in Neutral
            if (dashGear == "N")
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
            if (dashGear != "N" && HandBrakeInput < 1.0f)
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


            //Toggle Headlights
            // Mapped to Y button or whatever that would be on playstation
            HeadlightToggle = CurrentGamepad.buttonNorth.ReadValue();
            if (HeadlightToggle == 1 && toggleTime >= 0.4f)
            {
                headLightCycle++;
                headlightSound.Play();
                //Loop back around
                if (headLightCycle > 2) { headLightCycle = 0; }

                //Headlights off
                if (headLightCycle == 0)
                {
                    //Turns lights off duh
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
                else if (headLightCycle == 1)
                {
                    //Set lights to normal
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
                    // MAXIMUM POWER RAHHH
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
        }
        else
        {
            // Car is currently off, reset values
            headLightCycle = 0;
            headlightL.intensity = 0;
            headlightR.intensity = 0;
            headlightsEnabled = false;
            carSound.pitch = 0.0f;
            carOnUI.SetActive(true);
            cruiseUI.SetActive(false);
            exhaust.SetActive(false);
        }

        //Use Brakes
        BrakeInput = CurrentGamepad.leftTrigger.ReadValue();
        foreach (WheelCollider Wheel in Wheels)
        {
            Wheel.brakeTorque = BrakeInput * car.BrakePower * 10;
        }
        if (BrakeInput == 1)
        {
            //Debug.Log("Braking!");

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
        if (HandBrakeInput == 1.0f)
        {
            //Draw Skid Marks if you're going faster than 15 mph
            if (kph >= 15.0f)
            {
                DrawSkidMarks();
            }
            // Play handbrake sound
            if (!isHandBraking)
            {
                handbrakeSound.PlayOneShot(handbrakeClip, 1.0f);
            }
            isHandBraking = true;

            //Change the wheel friction
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

            //Increase how much you can turn, bc drifts are awesome
            steerangle *= 2.0f;
            //Play Particles
            driftParticle.Play();
        }
        //Reset after using handbrake
        else
        {
            //Stop drawing the skid marks, duh
            StopDrawSkidMarks();

            //Reset stuff
            isHandBraking = false;
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

            //Reset your max steering angle
            steerangle = (car.MaxSteeringAngle * (1 - (kph / car.topSpeed))) + 4;
            //Stop playing the particles
            driftParticle.Pause();
            driftParticle.Clear();
        }

        //Steer car
        SteeringInput = CurrentGamepad.leftStick.ReadValue().x;
        steerangle = (car.MaxSteeringAngle * (1 - (kph / car.topSpeed))) + 4;
        //Actually do the steering
        foreach (WheelCollider SWheel in SteeringWheels)
        {
            SWheel.steerAngle = Mathf.Lerp(-steerangle, steerangle, SteeringInput + 0.5f);
        }


        //D-Pad Input
        dpadX = Gamepad.current.dpad.x.ReadValue();
        dpadY = Gamepad.current.dpad.y.ReadValue();

        //Timers
        toggleTime += Time.deltaTime;
        cruiseT += Time.deltaTime;
        if (dpadX > 0f)
        {
            //Player Holding Right
            if (cruiseT > 0.4f)
            {
                //Enable Cruise Control
                CruiseOn = !CruiseOn;
                cruiseT = 0;
            }
        }
        if (dpadX < 0f)
        {
            // player currently holds left
            // Car Start/Stop
            if(toggleTime > 0.4f)
            {
                if(carIsOn == false)
                {
                    startSound.Play();
                }
                carIsOn = !carIsOn;
                toggleTime = 0;
            }
        }
        if (dpadY > 0f)
        {
            // player currently holds up
            //Enable Horn
            horn.Play();
        }
        if (dpadY < 0f)
        {
            // player currently holds down
        }

        //Spedometer, says kph is actually mph
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

    void DrawSkidMarks()
    {
        foreach(TrailRenderer T in skidMarks)
        {
            T.emitting = true;
        }
    }
    void StopDrawSkidMarks()
    {
        foreach (TrailRenderer T in skidMarks)
        {
            T.emitting = false;
        }
    }
}
