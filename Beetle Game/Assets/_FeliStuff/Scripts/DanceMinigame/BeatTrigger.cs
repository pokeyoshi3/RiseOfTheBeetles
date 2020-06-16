using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("touched " + gameObject.name);
        BeatDot dot = other.GetComponentInChildren<BeatDot>();
        if (dot != null)
            dot.EnteredTrigger();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        BeatDot dot = other.GetComponentInChildren<BeatDot>();
        if (dot != null)
            dot.LeftTrigger();
    }
}
