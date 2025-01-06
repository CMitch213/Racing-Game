using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorScript : MonoBehaviour
{
    //FOrmatting for the inspector
    [Space(10)]
    [Header("Do you want the cursor visible?")]
    //Set in Inspector
    public bool startVisible;
    [Space(30)]

    [Header("--DEBUG--ONLY--")]
    [SerializeField] private float pauseInput;


    void Start()
    {
        //Make it start depdning on your start visible variable
        if (startVisible)
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
    }

    void Update()
    {
        Gamepad CurrentGamepad = Gamepad.current;
        if (CurrentGamepad == null) { Debug.Log("Reconnect Controller/Gamepad"); return; }

        pauseInput = CurrentGamepad.startButton.ReadValue();
        if(pauseInput == 0) { Cursor.visible = false; }
        else if (pauseInput == 1) { Cursor.visible = true; }
    }
}
