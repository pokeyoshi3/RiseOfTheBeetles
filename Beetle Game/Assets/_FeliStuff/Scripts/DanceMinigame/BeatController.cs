using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    [SerializeField]
    private int beatsPerMinute;
    [SerializeField]
    private int songLength;
    [SerializeField]
    private string lastInput;
    [SerializeField]
    private eMinigameState state;
    [SerializeField]
    private int curDot;
    [SerializeField]
    private BeatDot[] allDots;

    private void Update()
    {
        if (PlayerInput() && state == eMinigameState.running)
        {

        }
    }

    private void SetState(eMinigameState state)
    {
        this.state = state;
    }

    private void SpawnBeats()
    {
        SetState(eMinigameState.paused);
        curDot = 0;
        //spawn dem bitches
        StartCoroutine(StartCounter());
    }


    private IEnumerator StartCounter()
    {
        //show a countdown for start
        yield return null;
        SetState(eMinigameState.running);
    }


    private bool PlayerInput()
    {
        if (Input.GetButton("up") || Input.GetButton("down") || Input.GetButton("left") || Input.GetButton("right"))
        {
            lastInput = Input.inputString;
            return true;
        }
        else
            return false;
    }

    private bool InputCorrect()
    {
        if (allDots[curDot].ButtonToPress() == lastInput)
            return true;
        else
            return false;
    }

    private IEnumerator MoveBar()
    {
        yield return null;
    }
}
