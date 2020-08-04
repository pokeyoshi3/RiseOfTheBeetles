using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

    public List<Dialogue> dialogues = new List<Dialogue>();
    public int startDialogue;
    public bool startOnAwake;
    bool started;

    void Start() {
        if (startOnAwake)
        {
            SetDialogue();
        }
    }

    /*In case this trigger gets lost i'll keep it though for now..
    void OnEnable()
    {
        Start();
    }*/

    public void SetDialogue()
    {
        if (GameManager_New.instance.dialogueController != null)
        {
            GameManager_New.instance.dialogueController.active = true;
            GameManager_New.instance.dialogueController.SetList(dialogues, startDialogue);
            GameManager_New.instance.SetGameState(eGameState.cutscene);
            started = true;
        }
    }

    void Update()
    {
        if(started && GameManager_New.instance.dialogueController.ended)
        {
            SendMessage("OnDialogueExit", SendMessageOptions.DontRequireReceiver);
            started = false;
        }
    }
}
