using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    //HACK: for gate 1
    public CanvasGroup BlackScreen;
    public GameObject Tetrisboard;

    [Tooltip("Scriptable Objects containing possible segments for this queens minigame")]
    public BeatSegment[] BeatSegments;
    


    [SerializeField]
    public Animator anim;
    [SerializeField]
    private int BPM = 120;
    [SerializeField]
    private eAbility ability;
    [SerializeField]
    private GameObject unlockInfo;
    [SerializeField]
    private GameObject matingInfo;
    [SerializeField]
    private int unlockCost;
    private AbilityManager AM;
    private bool selected;
    public eAbility Ability { get { return ability; } }
    public bool Unlocked { get { return AM.IsAbilityUnlocked(Ability); } }
    public int UnlockCost { get { return unlockCost; } }

    public static event System.Action<BeatSegment[], int, eAbility> OnMinigameStart = delegate { };

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }


    private void Start()
    {
        matingInfo.SetActive(false);
        unlockInfo.SetActive(false);
        AM = AbilityManager.Instance;
    }

    private void Update()
    {
        if(selected && GameManager.Instance.GameState != eGameState.minigame)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                print("pressed Action");

                if (Unlocked && !AM.IsAbilityActive(Ability))
                    StartCoroutine(StartMinigame());
                else //if (!Unlocked && (GameManager.Instance.ResourcesInBase >= UnlockCost))
                    Unlock();
            }
        }
    }

    public void ShowInfo(bool show)
    {
        selected = show;

        if (show)
        {
            if (Unlocked)
                matingInfo.SetActive(true);
            else
                unlockInfo.SetActive(true); 
        }
        else
        {
            matingInfo.SetActive(false);
            unlockInfo.SetActive(false); 
        }
    }

    public void Unlock()
    {
        AM.UnlockAbility(ability);
        matingInfo.SetActive(true);
        unlockInfo.SetActive(false);
        print(this.gameObject.name + " got unlocked");
    }

    public IEnumerator StartMinigame()
    {
        GameManager.Instance.ChangeGameState(eGameState.minigame);
        //while(BlackScreen.alpha < 1.0f)
        //{
        //    BlackScreen.alpha += 0.1f;
        //    yield return new WaitForSeconds(0.1f);
        //}

        //Tetrisboard.SetActive(true);

        OnMinigameStart(BeatSegments, BPM, ability);
        yield return new WaitForSeconds(0.25f);

        //while (BlackScreen.alpha > 0.0f)
        //{
        //    BlackScreen.alpha -= 0.1f;
        //    yield return new WaitForSeconds(0.1f);
        //}

        //UnityEngine.SceneManagement.SceneManager.LoadScene("Minigame", UnityEngine.SceneManagement.LoadSceneMode.Additive);
        //yield return null;
        //UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName("Minigame"));
    }
}
