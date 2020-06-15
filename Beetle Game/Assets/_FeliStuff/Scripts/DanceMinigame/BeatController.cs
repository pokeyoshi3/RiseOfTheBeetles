using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
    public UnityEngine.UI.Text TestTimer;


    [SerializeField]
    private int maxSegmentsPerGame = 5;
    [SerializeField]
    private int beatsPerMinute = 12;
    [SerializeField]
    private float distBetweenDots = 250.0f;
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
    [SerializeField]
    [Tooltip("up = 0, down = 1, left = 2, right = 3")]
    //up = 0, down = 1, left = 2, right = 3
    private Sprite[] buttonImages;
    [SerializeField]
    private RectTransform dotParent;

    private float secsPerBeat;
    private float movePerSec;
    private bool lerping = false;


    private void Start()
    {
        Queen.OnMinigameStart += GetQueenInfo;
    }

    private void Update()
    {
        if (lerping)
            LerpBar();

        if (state == eMinigameState.running)
        {
            if (Input.anyKeyDown)
            {
                switch (allDots[curDot].state)
                {
                    case BeatState.wrong:
                        return;
                    case BeatState.right:
                        return;
                    case BeatState.pending:
                        //not triggerable so input makes dot wrong
                        allDots[curDot].SetDotState(BeatState.wrong);
                        break;
                    case BeatState.triggerable:
                        //check if button was right and set to wrong/right
                        if (AllowedPlayerInput())
                        {
                            if (WasInputCorrect())
                                allDots[curDot].SetDotState(BeatState.right);
                            else
                                allDots[curDot].SetDotState(BeatState.wrong);
                        }
                        break;
                    default:
                        return;
                }
            }
        }
    }

    private void SetState(eMinigameState state)
    {
        this.state = state;
    }

    private void GetQueenInfo(BeatSegment[] possibleBeatSegments, int BPM)
    {
        beatsPerMinute = BPM;
        secsPerBeat = 60 / beatsPerMinute;
        movePerSec = distBetweenDots / secsPerBeat;

        List<BeatDot> dots = new List<BeatDot>();
        for (int i = 0; i < maxSegmentsPerGame; i++)
        {
            int rand = Random.Range(0, possibleBeatSegments.Length - 1);
            print("rand "+ rand);
            print("segs in list " + possibleBeatSegments.Length);
            BeatDot[] dotSeg = possibleBeatSegments[rand].Beats;
            dots.AddRange(dotSeg);
        }
        allDots = dots.ToArray();
        SpawnBeats();
    }

    private void SpawnBeats()
    {
        SetState(eMinigameState.paused);
        curDot = 0;
        for (int i = 0; i < allDots.Length; i++)
        {
            //spawn dot object and set image
            BeatDot newDot = Instantiate(allDots[i], dotParent.transform);
            newDot.SetButtonImage(buttonImages[newDot.ButtonImage()]);
        }
        StartCoroutine(StartCounter());
    }

    private IEnumerator StartCounter()
    {
        yield return new WaitForSeconds(0.5f);
        //show a countdown for start
        yield return null;
        SetState(eMinigameState.running);
        //StartCoroutine(MoveBar());
        lerping = true;
    }

    //One of the arrow keys has been pressed
    private bool AllowedPlayerInput()
    {
        if (Input.GetButtonDown("up") || Input.GetButtonDown("down") || Input.GetButtonDown("left") || Input.GetButtonDown("right"))
        {
            lastInput = Input.inputString;
            print("lastinput = "+ lastInput);
            return true;
        }
        else
            return false;
    }

    private bool WasInputCorrect()
    {
        switch (allDots[curDot].state)
        {
            case BeatState.wrong:
                return false;
            case BeatState.right:
                return true;
            case BeatState.pending:
                return false;
            case BeatState.triggerable:
                return allDots[curDot].ButtonToPress() == lastInput ? true : false;
            default:
                return false;
        }
    }

    private void LerpBar()
    {
        if (state == eMinigameState.paused)
        {
            //pause music and bar movement, dont allow input
        }

        calculatestep
        return (Mathf.Abs(end - start) / totalSteps);


        newPos = new Vector2(curPos.x - stepX, curPos.y);
        coinPos.anchoredPosition = newPos;
        curPos = newPos;
        //print("new pos: " + newPos);
        yield return new WaitForSeconds(timeUnit);


        //float timer = 0.0f;
        //TestTimer.text = timer.ToString();

        //while (state == eMinigameState.running)
        //{
        //    if (state == eMinigameState.paused)
        //    {
        //        //pause music and movement
        //        yield return null;
        //    }
        //    else
        //    {
        //        timer += Time.deltaTime;
        //        TestTimer.text = timer.ToString();
        //        //move bar and play music               
        //        dotParent.transform.position = new Vector3(dotParent.transform.position.x - movePerSec, dotParent.transform.position.y, dotParent.transform.position.z);                    
        //        yield return new WaitForSeconds(1.0f);
        //        timer += Time.deltaTime;
        //        TestTimer.text = timer.ToString();
        //    }
        //}
        //yield return null;
    }
}
