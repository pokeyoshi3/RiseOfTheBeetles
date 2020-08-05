
//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//public class BeatDot : MonoBehaviour
//{
//    [SerializeField]
//    private float triggerStartX = -1050.0f;
//    [SerializeField]
//    private float triggerEndX = -1120.0f;
//    [SerializeField]
//    private BeatInput buttonToPress;
//    [SerializeField]
//    private Image buttonImage;
//    [SerializeField]
//    private RectTransform pos;  
//    public int ID { get; private set; }
//    public BeatState state { get; private set; }
//    //animator or image for color i dunno

//    public static event System.Action<int> NextDotInLine = delegate { };

//    public void SetDotState(BeatState state)
//    {
//        this.state = state;
//        print(this.gameObject.name + " is now " + state.ToString());
//        switch (state)
//        {
//            case BeatState.wrong:
//                buttonImage.color = Color.red;
//                break;
//            case BeatState.right:
//                buttonImage.color = Color.green;
//                break;
//            case BeatState.pending:
//                buttonImage.color = Color.white;
//                break;
//            case BeatState.triggerable:
//                //HACK: just for testing
//                //buttonImage.color = Color.yellow;
//                break;
//            default:
//                break;
//        }
//    }

//    public string ButtonToPress()
//    {
//        return buttonToPress.ToString();
//    }

//    public int ButtonImage()
//    {
//        return (int)buttonToPress;
//    }

//    public void DotSetup(int ID, Sprite buttonImage)
//    {
//        this.ID = ID;
//        this.buttonImage.sprite = buttonImage;
//    }

//    public void MoveDotOneStep(float movePerStep)
//    {
//        SetPos(new Vector2(pos.anchoredPosition.x - movePerStep, pos.anchoredPosition.y));
//    }

//    public void SetPos(Vector2 newPos)
//    {
//        pos.anchoredPosition = newPos;
//        //print(this.gameObject.name + " new Pos is " + pos.anchoredPosition);
//        if (newPos.x <= triggerStartX && state == BeatState.pending)
//            EnteredTrigger();
//        else if (newPos.x <= triggerEndX)
//            LeftTrigger();
//    }

//    private void EnteredTrigger()
//    {
//        SetDotState(BeatState.triggerable); 
//    }

//    private void LeftTrigger()
//    {
//        BeatState newState = state == BeatState.right ? BeatState.right : BeatState.wrong;
//        SetDotState(newState);
//        StartCoroutine(SetNextDot());
//    }

//    private IEnumerator SetNextDot()
//    {
//        yield return new WaitForSeconds(0.1f);
//        NextDotInLine(ID + 1);
//    }

//}
