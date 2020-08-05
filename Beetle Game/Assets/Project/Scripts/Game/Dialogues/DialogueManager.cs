using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

    public List<Dialogue> dialogues = new List<Dialogue>();
    public int startDialogue;
    public bool startOnAwake;
    public bool running;

    private bool didmessage;

    void Start() {
        if (startOnAwake)
        {
            SetDialogue(startDialogue);
        }
    }

    /*In case this trigger gets lost i'll keep it though for now..
    void OnEnable()
    {
        Start();
    }*/

    public void SetDialogue(int startPoint)
    {
        if (GameManager_New.instance.dialogueController != null)
        {
            GameManager_New.instance.dialogueController.active = true;
            GameManager_New.instance.dialogueController.SetList(dialogues, startPoint);
            GameManager_New.instance.SetGameState(eGameState.cutscene);        
        }
    }

    void Update()
    {
        if (GameManager_New.instance.dialogueController != null)
        {
            if (GameManager_New.instance.dialogueController.dialogues != dialogues)
            {
                running = false;
                return;
            }

            running = GameManager_New.instance.dialogueController.active;
        }

        if(running) { didmessage = false; }

        if (!running && !didmessage && GameManager_New.instance.dialogueController.ended && dialogues[GameManager_New.instance.dialogueController.currentDialogue].endAfter)
        {
            gameObject.SendMessageUpwards("OnDialogueExit", dialogues[GameManager_New.instance.dialogueController.currentDialogue].endDialogueCommand, SendMessageOptions.DontRequireReceiver);
            didmessage = true;
        }
    }
}
