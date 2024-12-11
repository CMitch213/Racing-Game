using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Lap : MonoBehaviour
{

    [Header("In-Game")]
    public TMP_Text timeGO;
    float time = 0;
    public TMP_Text lapGO;
    int lap; // How many laps complete you start with
    public int count = 0;
    int minutes;
    int seconds;
    [Header("Win Screen")]
    bool hasWon = false;
    public GameObject winScreen, gameUI;
    public TMP_Text lap1GO, lap2GO, lap3GO, totalGO;
    string lap1time, lap2time, lap3time;
    


    // Update is called once per frame
    void Update()
    {
        if (!hasWon)
        {
            // Add time
            time += Time.deltaTime;
        }
        else
        {
            winScreen.SetActive(true);
            gameUI.SetActive(false);
        }

        // Take time and convert to min s and seconds
        minutes = Mathf.FloorToInt(time / 60F);
        seconds = Mathf.FloorToInt(time - minutes * 60);

        //Format mins and secs into a string
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        //Set the text of the GUI
        timeGO.text = niceTime;
        totalGO.text = niceTime;
        lapGO.text = lap.ToString();

        if (lap >= 3)
        {
            hasWon = true;
            lap1GO.text = lap1time;
            lap2GO.text = lap2time;
            lap3GO.text = lap3time;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        //Will create a better checkpoint system later
        if (other.CompareTag("1") && count == 0)
        {
            count = 1;

            if (lap == 1)
            {
                lap1time = string.Format("{0:0}:{1:00}", minutes, seconds);
            }
            else if (lap == 2)
            {
                lap2time = string.Format("{0:0}:{1:00}", minutes, seconds);
            }
            else if (lap == 3)
            {
                lap3time = string.Format("{0:0}:{1:00}", minutes, seconds);
            }

            lap++;

        }
        else if (other.CompareTag("2") && count == 1)
        {
            count = 2;
        }
        else if (other.CompareTag("3") && count == 2)
        {
            count = 3;
        }
        else if (other.CompareTag("4") && count == 3)
        {
            count = 0;
        }
    }
}
