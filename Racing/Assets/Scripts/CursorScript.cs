using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorScript : MonoBehaviour
{
    [SerializeField] private float pauseInput;

    void Update()
    {
        Gamepad CurrentGamepad = Gamepad.current;
        if (CurrentGamepad == null) { Debug.Log("Reconnect Controller/Gamepad"); return; }

        pauseInput = CurrentGamepad.startButton.ReadValue();
        if(pauseInput == 0) { Cursor.visible = false; }
        else if (pauseInput == 1) { Cursor.visible = true; }
    }
}
