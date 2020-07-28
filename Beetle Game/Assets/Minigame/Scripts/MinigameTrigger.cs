using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameTrigger : MonoBehaviour
{
    [Header("Setup")]
    public GameObject triggerInfo;
    public Text triggerText;

    [Header("Info")]
    public int unlockCost;
    public eAbility abilityToUnlock;
    public List<BeatInput> beats = new List<BeatInput>();
    public float distanceBetweenBeats;

    [Header("Dialogue")]
    public string unlockText;
    public string gameText;

    private bool playerOnTrigger;
    private bool unlocked;

    // Start is called before the first frame update
    void Start()
    {
        triggerText.text = unlockText;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOnTrigger && GameManager_New.instance.GetGameState() != eGameState.minigame)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (unlocked && !GameManager_New.instance.abilityManager.IsAbilityActive(abilityToUnlock))
                {
                    GameManager_New.instance.minigameManager.StartMiniGame(abilityToUnlock, beats, distanceBetweenBeats);
                }
                else if (!unlocked && (GameManager_New.instance.GetResourcesInBase() >= unlockCost))
                {
                    unlocked = true;
                    triggerText.text = gameText;
                }
            }
        }
    }

    void ShowInfo(bool info)
    {
        triggerInfo.SetActive(info);
        playerOnTrigger = info;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            ShowInfo(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            ShowInfo(false);
        }
    }
}
