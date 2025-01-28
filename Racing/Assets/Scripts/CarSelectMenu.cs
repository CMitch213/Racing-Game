using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class CarSelectMenu : MonoBehaviour
{
    //Variables
    [Header("Not Cars")]
    public GameObject cam;
    public GameObject thisMenu;
    public bool gameHasStarted;
    public GameObject selectedCar;
    [Space(25)]
    [Header("Cars")]
    public GameObject camaro;
    public GameObject miata;
    public GameObject dakar;
    public GameObject rs6;
    public GameObject nineEleven;
    public GameObject tractor;
    public GameObject raptor;
    public GameObject bike;
    public GameObject queen;
    [Header("CarsGO")]
    public GameObject camaroGO;
    public GameObject miataGO;
    public GameObject dakarGO;
    public GameObject rs6GO;
    public GameObject nineElevenGO;
    public GameObject tractorGO;
    public GameObject raptorGO;
    public GameObject bikeGO;
    public GameObject queenGO;


    private void Start()
    {
        gameHasStarted = false;
        selectedCar = thisMenu;
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
        gameHasStarted = true;      // Enable ai and shit
        selectedCar = camaroGO;       // Pick which car is selected
    }

    public void Miata()
    {
        miata.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
        gameHasStarted = true;
        selectedCar = miataGO;
    }

    public void Dakar()
    {
        dakar.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
        gameHasStarted = true;
        selectedCar = dakarGO;
    }

    public void RS6()
    {
        rs6.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
        gameHasStarted = true;
        selectedCar = rs6GO;
    }

    public void NineEleven()
    {
        nineEleven.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
        gameHasStarted = true;
        selectedCar = nineElevenGO;
    }

    public void Tractor()
    {
        tractor.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
        gameHasStarted = true;
        selectedCar = tractorGO;
    }

    public void Raptor()
    {
        raptor.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
        gameHasStarted = true;
        selectedCar = raptorGO;
    }

    public void Bike()
    {
        bike.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
        gameHasStarted = true;
        selectedCar = bikeGO;
    }

    public void McQueen()
    {
        queen.SetActive(true);
        cam.SetActive(false);
        thisMenu.SetActive(false);
        gameHasStarted = true;
        selectedCar = queenGO;
    }

    //Random fucntion
    public void Rand()
    {
        int select = Random.Range(1, 10);    //Set max value 1 highger than possible
        if (select == 1) { Camaro(); }
        if (select == 2) { Miata(); }
        if (select == 3) { Dakar(); }
        if (select == 4) { RS6(); }
        if (select == 5) { NineEleven(); }
        if (select == 6) { Tractor(); }
        if (select == 7) { Raptor(); }
        if (select == 8) { Bike(); }
        if (select == 9) { McQueen(); }
    }

    //If you hit HOME
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
