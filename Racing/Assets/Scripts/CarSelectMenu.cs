using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class CarSelectMenu : MonoBehaviour
{

    public GameObject cam;
    public GameObject thisMenu;
    public GameObject camaro;
    public GameObject miata;
    public GameObject dakar;
    public GameObject rs6;
    public GameObject nineEleven;
    public GameObject tractor;
    public GameObject raptor;
    public GameObject bike;

    private void Start()
    {
    }

    void Update()
    {
        //Make it so you can always see the mouse cursor while in the menu
        Cursor.visible = true;

        //Get The Controller
        Gamepad CurrentGamepad = Gamepad.current;
        if (CurrentGamepad == null) { Debug.Log("Reconnect Controller/Gamepad"); return; }

    }

    public void Camaro()
    {
        camaro.SetActive(true);     // Enable car
        cam.SetActive(false);       // Turn off menu camera
        thisMenu.SetActive(false);  // Disable Selection Menu
    }

    public void Miata()
    {
        miata.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
    }

    public void Dakar()
    {
        dakar.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
    }

    public void RS6()
    {
        rs6.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
    }

    public void NineEleven()
    {
        nineEleven.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
    }

    public void Tractor()
    {
        tractor.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
    }

    public void Raptor()
    {
        raptor.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
    }

    public void Bike()
    {
        bike.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
    }

    public void Rand()
    {
        int select = Random.Range(1, 9);    //Set max value 1 highger than possible
        if (select == 1) { Camaro(); }
        if (select == 2) { Miata(); }
        if (select == 3) { Dakar(); }
        if (select == 4) { RS6(); }
        if (select == 5) { NineEleven(); }
        if (select == 6) { Tractor(); }
        if (select == 7) { Raptor(); }
        if (select == 8) { Bike(); }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
