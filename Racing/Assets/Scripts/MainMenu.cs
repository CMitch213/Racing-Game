using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
        #if UNITY_STANDALONE
        Application.Quit();
        #endif
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial1");
    }
}
