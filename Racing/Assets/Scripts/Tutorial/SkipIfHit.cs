using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipIfHit : MonoBehaviour
{
    public TutorialUI tutUI;
    bool hasSkipped = false;

    private void OnTriggerEnter(Collider other)
    {
        //Can only skip once
        if (!hasSkipped)
        {
            tutUI.SkipTip();
            hasSkipped = true;
        }
    }
}
