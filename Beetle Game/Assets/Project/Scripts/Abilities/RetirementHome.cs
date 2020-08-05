using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetirementHome : MonoBehaviour
{
    [Header("Setup")]
    public GameObject triggerInfo;
    //public Text triggerText;

    [Header("Dialogue")]
    public DialogueManager dialogue;
    public int dialogueCannotUse;
    public int dialogueCanUse;

    //public string unlockText;
    //public string gameText;

    private bool playerOnTrigger;
    private bool didTalk;

    // Update is called once per frame
    void Update()
    {
        if (playerOnTrigger && GameManager_New.instance.GetGameState() == eGameState.running & !didTalk)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                dialogue.SetDialogue(0);
                didTalk = true;
            }
        }
    }

    public void OnDialogueExit(string msg)
    {
        Debug.Log("Got call: " + msg);
        switch(msg)
        {
            case "retirement_check":
                if(GameManager_New.instance.abilityManager.abilityCounter > 0) 
                     { dialogue.SetDialogue(dialogueCanUse);    }
                else { dialogue.SetDialogue(dialogueCannotUse); }
                break;
            case "retirement_yes":
                Retire();
                break;
            default:
                break;
        }
    }

    public void Retire()
    {
        GameManager_New.instance.abilityManager.ResetAbilitys();
    }

    void ShowInfo(bool info)
    {
        triggerInfo.SetActive(info);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerOnTrigger = true;
            ShowInfo(true && !didTalk);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            playerOnTrigger = false;
            ShowInfo(false);
            didTalk = false;
        }
    }
}
