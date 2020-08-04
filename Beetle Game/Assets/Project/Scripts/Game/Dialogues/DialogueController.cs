using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {

    [Header("Dialogue Setup")]
    public AudioSource aud;
    public List<Dialogue> dialogues = new List<Dialogue>();
    public int currentDialogue;
    public List<DialoguePanel> panels = new List<DialoguePanel>();
    public bool active;
    public bool ingame;

    [Header("UI Setup")]
    public string currentText;
    public bool stopKey;
    public bool ended;
    bool command; //skip for commands
    bool commandend;
    bool skipNext; //skip for \n \b etc.

    //public static DialogueController instance;
    IEnumerator co;

    void Awake()
    {
      //  instance = this;
    }

    // Use this for initialization
    void Start () {
        //instance = this;

        //if (ingame)
        //    aud = PlayerController.me.sfxSource;

        GetComponent<CanvasGroup>().alpha = (active ? 1 : 0);

        if (dialogues == null || !active)
            return;

        panels[dialogues[currentDialogue].panel].dialogueText.text = currentText;
	}

	// Update is called once per frame
	void Update () {

        //if (PlayerController.me != null)
        //        PlayerController.me.moveCont.disableMovement = active;

        GetComponent<CanvasGroup>().alpha = (active ? 1 : 0);

        if (dialogues == null || !active)
            return;

        panels[dialogues[currentDialogue].panel].dialogueText.text = currentText;

        if (Input.GetButton("Jump") && dialogues[currentDialogue].canBreak)
            ended = true;

        if(Input.GetButtonDown("Fire1") && ended && dialogues[currentDialogue].canMoveOn)
        {
            if (dialogues[currentDialogue].endAfter)
            {
                active = false;
                GameManager_New.instance.RevertGameState();
                SendMessageUpwards("OnDialogueExit", dialogues[currentDialogue].endDialogueCommand, SendMessageOptions.DontRequireReceiver);
                return;
            }

            SetDialogue(dialogues[currentDialogue].nextDialogue);
        }
	}

    public void SetList(List<Dialogue> list, int startDiag)
    {
        dialogues = list;
        SetDialogue(startDiag);
    }

    void SetDialogue(int dia)
    {
        if(co != null)
            StopCoroutine(co);
        ended = false;
        currentDialogue = dia;

        if (dia < dialogues.Count)
        {
            SetPanel(dialogues[currentDialogue].panel);

            if (panels[dialogues[currentDialogue].panel].hasImage)
                panels[dialogues[currentDialogue].panel].characterImage.sprite = dialogues[currentDialogue].charTexture;

            if (panels[dialogues[currentDialogue].panel].anim != null && dialogues[currentDialogue].charanimation != "")
                panels[dialogues[currentDialogue].panel].anim.Play(dialogues[currentDialogue].charanimation);

            co = DialogueReader();
            StartCoroutine(co);
        }
        else
            currentText = "<color=\"red\">Error: out of Range</color>";
    }

    public void SetPanel(int pan)
    {
        foreach (DialoguePanel p in panels)
        {
            p.dialogueText.text = "";
            p.panelObject.SetActive((p == panels[pan]));
        }

        if (panels[dialogues[currentDialogue].panel].hasOptions)
        {
            for (int i = 0; i < panels[dialogues[currentDialogue].panel].options.Length; i++)
            {
                panels[dialogues[currentDialogue].panel].options[i].GetComponentInChildren<Text>().text = dialogues[currentDialogue].optionString[i];
            }
        }
    }

    public void SendOption(int option)
    {
        ended = true;
        SetDialogue(dialogues[currentDialogue].optionJump[option]);
    }

    IEnumerator DialogueReader()
    {
        currentText = "";
        panels[dialogues[currentDialogue].panel].dialogueText.fontSize = dialogues[currentDialogue].fontSize;

        string chain = dialogues[currentDialogue].text;

        foreach (char c in chain)
        {
            if (ended)
                break;

            if (c == '<')
                command = true;

            if (c == '/')
                commandend = true;

            if (c == '\\')
                skipNext = true;

            if (c == '>' && commandend)
            {
                command = false;
                commandend = false;
            }

            currentText += c;

            if (!command && !skipNext)
            {
                yield return new WaitForSeconds(dialogues[currentDialogue].speed);
                if (dialogues[currentDialogue].voice != null)
                    aud.PlayOneShot(dialogues[currentDialogue].voice);
            }

            skipNext = false;
        }

        currentText = dialogues[currentDialogue].text;

        command = false;
        commandend = false;
        ended = true;
        yield return null;
    }
}

[System.Serializable]
public class Dialogue
{
    [Header("Dialogue Setup")]
    public string text;
    public int panel = 0;
    public float speed = 0.05f;
    public int fontSize = 55;

    [Header("Dialogue End")]
    public bool canBreak = true;
    public bool canMoveOn = true;
    public bool endAfter = false;    
    public int nextDialogue;
    public string endDialogueCommand;

    [Header("Object References")]
    public Sprite charTexture;
    public AudioClip voice;

    [Header("Misc")]
    public string charanimation;

    [Header("Options Dialogue")]
    public string[] optionString;
    public int[] optionJump;
}

[System.Serializable]
public class DialoguePanel
{
    public GameObject panelObject;
    public Text dialogueText;
    public Image characterImage;
    public bool hasImage;
    public Button[] options;
    public bool hasOptions;
    public Animator anim;
}