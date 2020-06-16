
using UnityEngine;
using UnityEngine.UI;

public class BeatDot : MonoBehaviour
{
    [SerializeField]
    private BeatInput buttonToPress;
    [SerializeField]
    private Image buttonImage;
    [SerializeField]
    private RectTransform pos;  
    public BeatState state { get; private set; }
    //animator or image for color i dunno

    public void SetDotState(BeatState state)
    {
        this.state = state;
        print(this.gameObject.name + " is now " + state.ToString());
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

    public void MoveDotOneStep(float movePerStep)
    {
        if (pos == null)
        {
            print("i lost my recttrans like an idiot");
            return;
        }
        SetPos(new Vector2(pos.anchoredPosition.x - movePerStep, pos.anchoredPosition.y));
    }

    public void SetPos(Vector2 newPos)
    {
        pos.anchoredPosition = newPos;
        print(this.gameObject.name + " new Pos is " + pos.anchoredPosition);
    }

    public void EnteredTrigger()
    {
        SetDotState(BeatState.triggerable);
    }

    public void LeftTrigger()
    {
        BeatState newState = state != BeatState.right ? BeatState.wrong : BeatState.right;
        SetDotState(newState);
    }
}
