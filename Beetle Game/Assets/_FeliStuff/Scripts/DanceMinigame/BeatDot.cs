using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BeatDot : MonoBehaviour
{
    [SerializeField]
    private BeatInput buttonToPress;
    [SerializeField]
    private Image buttonImage;

    public BeatState state { get; private set; }
    //animator or image for color i dunno

    public void SetDotState(BeatState state)
    {
        this.state = state;
    }

    public string ButtonToPress()
    {
        return buttonToPress.ToString();
    }

    public int ButtonImage()
    {
        return (int)buttonToPress;
    }

    public void SetButtonImage(Sprite buttonImage)
    {
        this.buttonImage.sprite = buttonImage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        state = BeatState.triggerable;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        state = state != BeatState.right ? BeatState.wrong : BeatState.right;
    }
}
