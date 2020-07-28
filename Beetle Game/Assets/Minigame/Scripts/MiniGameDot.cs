using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameDot : MonoBehaviour
{

    [Header("Setup")]
    public float speed = 1f;
    public Transform sprite;
    
    private MinigameManager manager;
    public bool run;
    public BeatInput direction;

    private RectTransform trans;
    private BeatInput gotInput;

    private void Awake()
    {
        trans = GetComponent<RectTransform>();

        //debug hack
        //SetupValues(null, 200, direction);
    }

    void FixedUpdate()
    {
        if(!run) { return; }

        //MOVE MOVE MOVE
        transform.localPosition -= Vector3.right * Time.fixedDeltaTime * speed;
    }
    public void SetupValues(MinigameManager manager, float speed, BeatInput direction)
    {
        //STARTUP FUNCTION
        this.manager = manager;
        this.speed = speed;
        this.direction = direction;

        SetRotation();

        run = true;
        sprite.gameObject.SetActive(true);
    }

    public void StopBeatDot(BeatInput inp)
    {
        //MINIGAME MANAGER CALLS THIS TO GET DOT STATE
        run = false;
        sprite.gameObject.SetActive(false);
        gotInput = inp;
    }

    public float GetCurrentPos()
    {
        return GetComponent<RectTransform>().anchoredPosition.x;
    }

    public BeatInput GetInputValue()
    {
        return gotInput;
    }

    void SetRotation()
    {
        //SET SPRITE ROTATION
        switch (direction)
        {
            case BeatInput.up:
                sprite.localEulerAngles = Vector3.zero;
                break;
            case BeatInput.down:
                sprite.localEulerAngles = Vector3.forward * 180;
                break;
            case BeatInput.left:
                sprite.localEulerAngles = Vector3.forward * 90;
                break;
            case BeatInput.right:
                sprite.localEulerAngles = Vector3.forward * -90;
                break;
            default:
                break;
        }
    }
}
