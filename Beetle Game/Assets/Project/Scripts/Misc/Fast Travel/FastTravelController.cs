using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelController : MonoBehaviour
{
    public bool canTravel;
    public int travelIndex;

    [Header("Setup")]
    public GameObject triggerInfo;
    public GameObject particleNotActive;
    public GameObject particleActive;
    //public Text triggerText;

    [Header("Dialogue")]
    public DialogueManager dialogue;    
    public int dialogueAlreadyThere;
    public int dialogueCannotUse;

    //public string unlockText;
    //public string gameText;

    public bool playerOnTrigger;
    public bool didTalk;

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

        particleNotActive.SetActive(!canTravel);
        particleActive.SetActive(canTravel);
    }

    public void OnDialogueExit(string msg)
    {
        Debug.Log("Got call: " + msg);
        switch (msg)
        {
            case "travel_0":
                Travel(0);
                break;
            case "travel_1":
                Travel(1);
                break;
            case "travel_2":
                Travel(2);
                break;
            case "travel_3":
                Travel(3);
                break;
            case "travel_4":
                Travel(4);
                break;
            default:
                break;
        }
    }

    public void Travel(int point)
    {            
        if(point != travelIndex)
        {
            if (GameManager_New.instance.fastTravelManager.GetTravelPointActive(point))
            {
                GameManager_New.instance.fastTravelManager.TravelNow(point);
            }
            else { dialogue.SetDialogue(dialogueCannotUse); }
        }
        else { dialogue.SetDialogue(dialogueAlreadyThere); }

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
            canTravel = true;
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
