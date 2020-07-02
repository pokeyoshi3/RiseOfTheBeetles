using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatSegment", menuName ="ScriptableObjects/BeatSegment")]
public class BeatSegment : ScriptableObject
{
    [Tooltip("List of BeatDot prefabs in the order of spawning")]
    public BeatDot[] Beats;
}
