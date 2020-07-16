using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public MovementController MoveCon;
    public SpriteRenderer SpriteRend;
    public Transform model;
    public Animator animController;
    public float turnSpeed = 0.3f;

    private Vector3 modelRot = new Vector3(0, 90, 0);

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

        if(animController != null) 
        { 
            animController.SetFloat("VelocityX", MoveCon.RigidbodyGetVelocity().x);
            animController.SetFloat("VelocityY", MoveCon.RigidbodyGetVelocity().y);
            animController.SetBool("IsGrounded", MoveCon.Ground.grounded);
        }
    }

    private void FixedUpdate()
    {
        if(model)
        {
            model.localRotation = Quaternion.Slerp(model.localRotation, Quaternion.Euler(modelRot), turnSpeed);
        }
    }

    void SetFlip(float xSpeed)
    {
        if(xSpeed < 0)
        {
            SpriteRend.flipX = true;
            modelRot = new Vector3(0, -90, 0); 
        }
        else if (xSpeed > 0)
        {
            SpriteRend.flipX = false;
            modelRot = new Vector3(0, 90, 0); 
        }
    }
}
