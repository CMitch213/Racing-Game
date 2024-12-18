using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BikeTilt : MonoBehaviour
{

    [SerializeField] private float SteeringInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gamepad CurrentGamepad = Gamepad.current;
        if (CurrentGamepad == null) { Debug.Log("Reconnect Controller/Gamepad"); return; }

        SteeringInput = CurrentGamepad.leftStick.ReadValue().y;

        // Convert the current rotation to degrees
        float currentAngleX = transform.eulerAngles.x;
        if (currentAngleX > 180) currentAngleX -= 360; // Convert 0-360 to -180 to 180 range

        //Tilt back
        if (SteeringInput <= -0.33 && currentAngleX < 20.0f)
        {
            transform.Rotate(-SteeringInput * 50.0f * Time.deltaTime, 0, 0);
        }
        //Slowly go down if in deadzone
        else if (SteeringInput > -0.33 && currentAngleX > 0.0f)
        {
            transform.Rotate(-50 * Time.deltaTime, 0, 0);
        }
        //Tilt forward
        else if (SteeringInput > 0.4 && currentAngleX > 0.0f)
        {
            transform.Rotate(SteeringInput * -125 * Time.deltaTime, 0, 0);
        }
    }
}
