using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorScript : MonoBehaviour
{
    Gamepad CurrentGamepad = Gamepad.current;
    [SerializeField] private float pauseInput;

    void Update()
    {
        pauseInput = CurrentGamepad.startButton.ReadValue();
        if(pauseInput == 0) { Cursor.visible = false; }
        else if (pauseInput == 1) { Cursor.visible = true; }
    }
}
