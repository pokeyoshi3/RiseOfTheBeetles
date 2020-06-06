using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDot : MonoBehaviour
{
    [SerializeField]
    private BeatInput buttonToPress;
    [SerializeField]
    private BeatState result;

    public string ButtonToPress()
    {
        return buttonToPress.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        result = BeatState.triggerable;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        result = result != BeatState.right ? BeatState.wrong : BeatState.right;
    }
}
