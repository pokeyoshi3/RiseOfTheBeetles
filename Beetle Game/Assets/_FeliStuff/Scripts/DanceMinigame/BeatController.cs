using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BeatController : MonoBehaviour
{
    public UnityEngine.UI.Text TestTimer;

    [SerializeField]
    [Tooltip("Same as abilities: wings = 0, horn = 1, water = 2, claw = 3")]
    private Animator[] queenAnims;

    private Animator curAnim;

    [SerializeField]
    private int maxSegmentsPerGame = 5;
    [SerializeField]
    private int beatsPerMinute = 100;
    [SerializeField]
    private float distBetweenDots = 250.0f;
    [SerializeField]
    private int songLength;
    //[SerializeField]
    //private string lastInput;
    [SerializeField]
    private eMinigameState state;
    [SerializeField]
    private BeatDot curDot;
    [SerializeField]
    private BeatDot[] allDots;
    [SerializeField]
    [Tooltip("up = 0, down = 1, left = 2, right = 3")]
    //up = 0, down = 1, left = 2, right = 3
    private Sprite[] buttonImages;
    [SerializeField]
    private RectTransform dotSpawn;

    public Sprite[] queenSprites;
    public SpriteRenderer queenTest;

    private float timePerStep;
    private float movePerStep;
    private bool lerping = false;


    private void Start()
    {
        Queen.OnMinigameStart += GetQueenInfo;
        BeatDot.NextDotInLine += SetCurDot;
    }

    private void SetCurDot(int dotID)
    {
        if (dotID > allDots.Length - 1)            
            return;

        curDot = allDots[dotID];
    }

    private void Update()
    {
        if (state == eMinigameState.running)
        {
            queenTest.sprite = queenSprites[curDot.ButtonImage() + 1];

            if (Input.GetButtonDown("up"))
            {
                if (WasInputCorrect("up"))
                {
                    curDot.SetDotState(BeatState.right);
                }
                else
                {
                    curDot.SetDotState(BeatState.wrong);
                }
            }
            else if (Input.GetButtonDown("down"))
            {
                if (WasInputCorrect("down"))
                {
                    curDot.SetDotState(BeatState.right);
                }
                else
                {
                    curDot.SetDotState(BeatState.wrong);
                }
            }
            else if (Input.GetButtonDown("left"))
            {
                if (WasInputCorrect("left"))
                {
                    curDot.SetDotState(BeatState.right);
                }
                else
                {
                    curDot.SetDotState(BeatState.wrong);
                }
            }
            else if (Input.GetButtonDown("right"))
            {
                if (WasInputCorrect("right"))
                {
                    curDot.SetDotState(BeatState.right);
                }
                else
                {
                    curDot.SetDotState(BeatState.wrong);
                }
            }
            else if (Input.anyKeyDown)
            {
                curDot.SetDotState(BeatState.wrong);
            }        
        }
        else
        {
            queenTest.sprite = queenSprites[0];
        }

        //hack for gate 2
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            state = eMinigameState.normalWin;
            GameManager_New gm = GameManager_New.instance;

            //gm.abilityManager.UnlockAbility(eAbility.claw);
            //gm.abilityManager.ToggleAbilityActive(eAbility.claw, true);
            gm.GetPlayerInstance().SetAbilities();

            foreach(BeatDot bd in allDots)
            {
                Destroy(bd.gameObject);
            }

            gm.SetGameState(eGameState.running);
        }
    }

    private bool WasInputCorrect(string lastInput)
    {
        switch (curDot.state)
        {
            case BeatState.wrong:
                return false;
            case BeatState.right:
                return true;
            case BeatState.pending:
                return false;
            case BeatState.triggerable:
                return curDot.ButtonToPress() == lastInput ? true : false;
            default:
                return false;
        }
    }

    private void SetState(eMinigameState state)
    {
        this.state = state;
    }

    private void GetQueenInfo(BeatSegment[] possibleBeatSegments, int BPM, eAbility ability)
    {
        //curAnim = queenAnims[(int)ability];
        beatsPerMinute = BPM;
        timePerStep = (60.0f / beatsPerMinute) / 50.0f;
        movePerStep = distBetweenDots / 50.0f;

        List<BeatDot> dots = new List<BeatDot>();
        for (int i = 0; i < maxSegmentsPerGame; i++)
        {
            int rand = Random.Range(0, possibleBeatSegments.Length - 1);
            //print("rand " + rand);
            //print("segs in list " + possibleBeatSegments.Length);
            BeatDot[] dotSeg = possibleBeatSegments[rand].Beats;
            dots.AddRange(dotSeg);            
        }
        allDots = dots.ToArray();
        SpawnBeats();
    }

    private void SpawnBeats()
    {
        SetState(eMinigameState.paused);
        for (int i = 0; i < allDots.Length; i++)
        {
            //spawn dot object and set image
            BeatDot newDot = Instantiate(allDots[i], dotSpawn.transform);
            newDot.gameObject.name = "Dot_" + i;
            newDot.SetDotState(BeatState.pending);
            newDot.DotSetup(i, buttonImages[newDot.ButtonImage()]);
            newDot.SetPos(new Vector2(i * distBetweenDots, -75f));
            allDots[i] = newDot;
        }
        curDot = allDots[0];
        StartCoroutine(StartCounter());
    }

    private IEnumerator StartCounter()
    {
        yield return new WaitForSeconds(0.5f);
        //show a countdown for start

        bool noInput = true;

        while (noInput)
        {
            yield return null;
            if (Input.anyKey)
                noInput = false;
        }       

        SetState(eMinigameState.running);
        StartCoroutine(MoveBar());
    }

    private IEnumerator MoveBar()
    {
        print("start moving all dots");

        while (state == eMinigameState.running)
        {

            if (state == eMinigameState.paused)
            {
                //pause music and bar movement, dont allow input
                yield return null;
            }

            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    state = eMinigameState.paused;
            //    yield return null;
            //}

            foreach(BeatDot dot in allDots)
            {
                dot.MoveDotOneStep(movePerStep);
            }
            yield return new WaitForSeconds(timePerStep);
        }
    }
}
