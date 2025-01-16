using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Ghost : ScriptableObject
{
    //Booleans that we cahnge (Names are self explanatory)
    [Header("Controls")]
    //Right Click on isRecord and select reset to reset all the postions
    [ContextMenuItem("Reset all save points!", "ResetData")]
    public bool isRecord;
    public bool isReplay;
    public float recordFrequency;

    [Space(15)]
    //Stuff that is saved for you to rpelay
    [Header("Save points")]
    public List<float> timeStamp;
    public List<Vector3> position;
    public List<Quaternion> rotation;

    //Reset all of our lists.
    public void ResetData()
    {
        timeStamp.Clear();
        position.Clear();
        rotation.Clear();
    }
}
