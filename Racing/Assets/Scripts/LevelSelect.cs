using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void LoadNurbur()
    {
        SceneManager.LoadScene("Nurburgring");
    }
    public void LoadOval()
    {
        SceneManager.LoadScene("OvalTrack");
    }
    public void LoadDrag()
    {
        SceneManager.LoadScene("DragStrip");
    }
    public void LoadRainbow()
    {
        SceneManager.LoadScene("Rainbow-Road");
    }
    public void LoadToad()
    {
        SceneManager.LoadScene("Toads-Harbour");
    }
    public void Ran()
    {
        int select = Random.Range(1, 6);    //Set max value 1 highger than possible
        if (select == 1) { LoadNurbur(); }
        if (select == 2) { LoadOval(); }
        if (select == 3) { LoadDrag(); }
        if (select == 4) { LoadRainbow(); }
        if (select == 5) { LoadToad(); }
    }
}
