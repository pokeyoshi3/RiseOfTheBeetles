using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public MovementController MoveCon;
    public SpriteRenderer SpriteRend;

    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (!MoveCon.IsApplyingForce()) 
        { 
            SetFlip(MoveCon.GetSpeed().x);
        }
    }

    void SetFlip(float xSpeed)
    {
        if(xSpeed < 0)
        {
            SpriteRend.flipX = true;
        }
        else if (xSpeed > 0)
        {
            SpriteRend.flipX = false;
        }
    }
}
