using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayMenu : MonoBehaviour
{
    public Ghost ghost;
    //Recording
    public void Record()
    {
        ghost.isRecord = true;
        ghost.isReplay = false;
    }

    //Replaying
    public void Replay()
    {
        ghost.isRecord = false;
        ghost.isReplay = true;
    }
}
