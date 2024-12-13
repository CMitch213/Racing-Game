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
}
