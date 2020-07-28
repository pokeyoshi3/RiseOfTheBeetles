using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    [System.Serializable]
    public struct Score //Score window
    {
        public string name;
        public float window;
        public int points;
        public GameObject effectTextPrefab;
    }

    [Header("Current Game Info")]
    public List<BeatInput> beats;
    public eAbility abilityToUnlock;
    public float distanceBetweenDots = 1f;
    public float beatDotSpeed = 200f;

    [Header("Setup")]
    public GameObject holder;
    public GameObject beatDotPrefab;
    public Transform beatDotHolder;
    public Transform textEffectHolder;
    public Animator cameraAnimator;

    [Header("Game Dot Window")]
    public float DotStartPos = 550;
    public List<Score> scoreWindow;
    public float inputOutOfRange;
    public GameObject loseEffectTextPrefab;
    public GameObject DotExplodeEffect;

    [Header("Debug")]
    public bool running;
    public int currentBeatIndex;
    public List<MiniGameDot> beatDots;
    private bool inputAxisReset;

    private MiniGameDot currentBeat { get { return (beatDots.Count > 0) ? beatDots[currentBeatIndex] : null; } }

    private void Start()
    {
        //TEST BEFORE QUEEN IS ADDED
        //StartMiniGame(eAbility.horn, beats, distanceBetweenDots);
    }

    private void Update()
    {
        holder.SetActive(running);

        if (!running) { return; }

        if(currentBeat != null)
        {
            InputCheck();
        }

        if(currentBeat != null && currentBeat.GetCurrentPos() <= -inputOutOfRange)
        {
            PressBeatInput(BeatInput.up);
        }
    }

    public void StartMiniGame(eAbility abilityToUnlock, List<BeatInput> beats, float distanceBetweenDots)
    {
        this.abilityToUnlock = abilityToUnlock;
        this.beats = beats;        
        this.distanceBetweenDots = distanceBetweenDots;

        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        GameManager_New.instance.SetGameState(eGameState.minigame);        
        currentBeatIndex = 0;

        GameManager_New.instance.canvasFader.blendActive = true;
        yield return new WaitForSeconds(2.0f);
        SpawnBeatDots(beats);
        running = true;
        GameManager_New.instance.canvasFader.blendActive = false;
    }

    public void EndMiniGame()
    {
        StartCoroutine(EndGame());
    }

    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager_New.instance.canvasFader.blendActive = true;
        yield return new WaitForSeconds(1.0f);
        GameManager_New.instance.canvasFader.blendActive = false;

        //TODO: WIN CONDITION
        GameManager_New.instance.abilityManager.UnlockAbility(abilityToUnlock);
        GameManager_New.instance.abilityManager.ToggleAbilityActive(abilityToUnlock, true);

        running = false;
        DestroyBeatDots();
        GameManager_New.instance.SetGameState(eGameState.running);
    }

    public void SpawnBeatDots(List<BeatInput> beats)
    {        
        for (int i = 0; i < beats.Count; i++)
        {
            GameObject go = Instantiate(beatDotPrefab, beatDotHolder);
            Vector2 newPos = new Vector2(DotStartPos + distanceBetweenDots * i, go.GetComponent<RectTransform>().anchoredPosition.y);
            
            go.GetComponent<RectTransform>().anchoredPosition = newPos;
            go.GetComponent<MiniGameDot>().SetupValues(this, beatDotSpeed, beats[i]); 

            beatDots.Add(go.GetComponent<MiniGameDot>());
        }
    }

    int GetBeatState(int beat)
    {
        float pos = Mathf.Abs(beatDots[beat].GetCurrentPos());

        if(beatDots[beat].direction != beatDots[beat].GetInputValue())
        {
            return -1;
        }

        for (int i = 0; i < scoreWindow.Count; i++)
        {
            if (pos < scoreWindow[i].window)
            {
                return i;   //return if window is in range
            }
        }

        return -1;
    }

    public void DestroyBeatDots()
    { 
        foreach(MiniGameDot mgd in beatDots)
        {
            Destroy(mgd.gameObject);
        }

        beatDots.Clear();
    }

    public void InputCheck()
    {
        Vector2 axis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (currentBeat.GetCurrentPos() < inputOutOfRange && currentBeat.run)
        {
            if (axis.x != 0 && inputAxisReset)
            {
                if (axis.x > 0) { PressBeatInput(BeatInput.right); }
                else { PressBeatInput(BeatInput.left); }

                inputAxisReset = false;
            }
            else if (axis.y != 0 && inputAxisReset)
            {
                if (axis.y > 0) { PressBeatInput(BeatInput.up); }
                else { PressBeatInput(BeatInput.down); }

                inputAxisReset = false;
            }
            
            if(axis.x == 0 && axis.y == 0) { inputAxisReset = true; }
        }
    }

    public void PressBeatInput(BeatInput inp)
    {
        currentBeat.StopBeatDot(inp);
        int won = GetBeatState(currentBeatIndex);

        //Instantiate text vfx
        Instantiate( won < 0 ? loseEffectTextPrefab : scoreWindow[won].effectTextPrefab, textEffectHolder, false);

        //Instantiate vfx explode
        Instantiate(DotExplodeEffect, currentBeat.transform.position - Vector3.forward, Quaternion.identity);

        if(won >= 0)
        {
            cameraAnimator.Play("Shake", 0, 0.0f);
        }

        //condition for last beat input
        if (currentBeatIndex < beatDots.Count - 1) { currentBeatIndex++; }
        else { EndMiniGame();  }
    }
}
