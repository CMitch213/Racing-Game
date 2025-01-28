using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TutorialUI : MonoBehaviour
{
    //Skip tip  variables 
    [Header("Skip Through Tips")]
    public float skipInput = 0.0f;
    public GameObject tutorialUI;
    public int progress = 0;
    float skipTimer = 1.0f;
    public bool isLastLevel;
    public GameObject currentTip;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(tutorialUI.transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {
        //Get Input
        Gamepad CurrentGamepad = Gamepad.current;
        if (CurrentGamepad == null) { Debug.Log("Reconnect Controller/Gamepad"); return; }

        currentTip = tutorialUI.transform.GetChild(progress).gameObject;

        //Skip the tip with b
        skipInput = CurrentGamepad.buttonEast.ReadValue();
        //Add to the timer
        skipTimer += Time.deltaTime;

        if (skipInput > 0.0f && skipTimer >= 1.0f && currentTip.GetComponent<TipStats>().canSkip)
        {
            SkipTip();
            skipTimer = 0.0f;
        }
    }

    public void SkipTip()
    {
        if (progress < tutorialUI.transform.childCount-1)
        {
            //Set the tip that you want to skip to unactive
            currentTip.SetActive(false);
            //Enable the next tip
            GameObject nextTip = tutorialUI.transform.GetChild(progress+1).gameObject;
            nextTip.SetActive(true);
        }
        else
        {
            if (!isLastLevel)
            {
                NextLevel();
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        progress++;
    }
    
    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
