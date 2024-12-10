using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Lap : MonoBehaviour
{

    public TMP_Text timeGO;
    float time = 0;
    public TMP_Text lapGO;
    int lap = 0;
    public int count = 0;


    // Update is called once per frame
    void Update()
    {
        // Add time
        time += Time.deltaTime;

        // Take time and convert to min s and seconds
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);

        //Format mins and secs into a string
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        //Set the text of the GUI
        timeGO.text = niceTime;
        lapGO.text = lap.ToString();
    }

    void OnTriggerEnter(Collider other)
    {
        //Will create a better checkpoint system later
        if (other.CompareTag("1") && count == 0)
        {
            count = 1;
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
