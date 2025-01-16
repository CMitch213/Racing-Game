using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRecorder : MonoBehaviour
{
    // Variable Declaration
    public Ghost ghost;
    public CarSelectMenu menu;
    private float timer;
    private float timeValue;

    //Before everything runs, check if you're recording and if you are reset it.
    private void Awake()
    {
        if (ghost.isRecord)
        {
            ghost.ResetData();
            timeValue = 0f;
            timer = 0f;
        }
    }

    void Update()
    {
        //Add time
        if (menu.gameHasStarted)
        {
            timer += Time.unscaledDeltaTime;
            timeValue += Time.unscaledDeltaTime;
        }

        //Record save data
        if(ghost.isRecord & timer >= 1/ghost.recordFrequency && menu.gameHasStarted)
        {
            ghost.timeStamp.Add(timeValue);
            ghost.position.Add(this.transform.position);
            ghost.rotation.Add(this.transform.rotation);

            timer = 0f;
        }
    }
}
