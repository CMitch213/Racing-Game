using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargetStats : MonoBehaviour
{
    //Just accessed in the ai script
    public bool shouldBreak;
    public bool shouldStop;
    [Tooltip("On a scale of 0-1")]
    public float brakeStrength;
}
