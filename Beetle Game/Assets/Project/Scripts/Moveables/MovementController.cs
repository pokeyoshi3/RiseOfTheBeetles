using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement Controller basically applies forces and basic input regulation.
/// May be called or changed by the PlayerController and different Player-Related scripts.
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class MovementController : MonoBehaviour
{
    [Header("Object References")]
    public Grounder Ground;           //A grounder object for our Player
    public Rigidbody2D rigid;           //main rigidbody for force application
    public Collider2D col;              //main collision component

    private Vector2 currentMovement;    //Force which is applied directly to the rigidbody
    private float movementX;            //Current walk speed (X Movement)
    private float currentSpeed;         //Variable set by Controllers to determine speed
    private float currentAcceleration;  //Variable set by Controllers to determine Acceleration Fluidity

    //[Header("Movement Setup")]          //basic movement settings    
    //public float GravityScale = 1f;
    //public float WalkSpeed = 9.0f;
    //public float JumpHeight = 10.0f;
    //public bool CanFly = true;
    //public float MaxFlyTime = 3f;
    //public bool CanSwim = true;
    //public float UnderWaterGravity = 2f;
    //public float UnderWaterWalkSpeed = 3f;

    //[Header("Atomic Modifiers")]
    //[Range(0.01f, 1), Tooltip("Acceleration Fluidity describes how fast the player will respond to the input action")]
    //public float AccelerationFluidity = 0.7f;
    //public float CoyoteJumpTime = 0.2f;
    //public float FlyWobble = 1f;
    //public float FlyWobbleSpeed = 1f;

    //[Header("Miscellaneous")]
    //public bool drawGizmos = true;      //wether to draw gizmos in editor mode or not

    //[Header("Debug (Do not Touch)")]

    //private bool disableInput;
    //private Vector2 inputAxis;          //input axis, read only    
    //private bool inputJump;             //input jump, read only


    //private Vector2 currentMovement;    //current applied movement vectors, read only
    //private float movementX;            //lerp buffer for x (horizontal) movement


    //private bool jumpRequest;           //jump call    
    //private bool jumped;
    //private bool wasGrounded;
    //private bool stopJumpRequest;
    //private bool stopJump;

    //private bool flying;                //flying call

    private float flyTime;        //used to generate fly wobble effect

    private Coroutine currentForce;
    List<PlayerForce> playerForceQueue = new List<PlayerForce>();
    private bool forceRunning = false;
    private bool canFlip = true;

    //private bool underWater = false;
    //private float currentGravity { get { return (underWater ? UnderWaterGravity : GravityScale); } }
    //private float currentSpeed { get { return (underWater ? UnderWaterWalkSpeed : WalkSpeed); } }


    private void Start()
    {
        //override gravityscale modifier from rigidbody
        //rigid.gravityScale = currentGravity;
        //Grounder.SetCoyoteTime(CoyoteJumpTime);
    }

    private void Update()
    {
        //InputDetection();
        //MoveAll();
        Forces();
    }

    public void Walk(float speed, float acceleration)
    {
        if (forceRunning)
        {
            movementX = 0;
            return;
        }

        //Walking: slowly fade into the given walking speed
        movementX = Mathf.MoveTowards(movementX, speed, acceleration);
        currentMovement = new Vector2(movementX, rigid.velocity.y);
        rigid.velocity = currentMovement;
    }

    public void Jump(float height)
    {
        if(forceRunning)
        {
            return;
        }

        //reset vertical velocity so jumps remain consistent
        RigidbodyResetVelocity(false, true);
        //Add force relative to the jump height and gravity modifier
        rigid.AddForce(Vector2.up * Mathf.Sqrt(-2.0f * Physics2D.gravity.y * rigid.gravityScale * height), ForceMode2D.Impulse);
    }

    public void Fly(float flyTime, float wobble, float wobbleSpeed)
    {
        if (forceRunning)
        {
            return;
        }

        rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Cos(flyTime * wobbleSpeed) * wobble);
    }

    //private void InputDetection()
    //{
    //    if(disableInput)    //Disable any input if requested
    //    {
    //        inputAxis = Vector2.zero;
    //        inputJump = false;
    //        jumpRequest = false;
    //        stopJump = false;
    //        return;
    //    }

    //    //Movement Axis, Horizontal and Vertical
    //    inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

    //    //Jump (+flying) button: inputJump is a constant input lookup, jumpRequest is a trigger and disables after action        
    //    if(Input.GetButtonDown("Jump")) { jumpRequest = true; }
    //    inputJump = Input.GetButton("Jump");
    //    if (Input.GetButtonUp("Jump")) { stopJumpRequest = true; }
    //}

    //private void MoveAll()
    //{
    //    if (!forceRunning)
    //    {
    //        Move_Walk();
    //        Move_Jump();
    //        Move_Fly();
    //    }
    //}

    //private void Move_Walk()
    //{
    //    //Walking: get the input axis values and use MoveTowards to slowly fade into the set walking speed
    //    movementX = Mathf.MoveTowards(movementX, inputAxis.x * currentSpeed, AccelerationFluidity);
    //    currentMovement = new Vector2(movementX, rigid.velocity.y);
    //    rigid.velocity = currentMovement;
    //}

    //void Move_Jump()
    //{       
    //    //Jumping: Wait for button press to call the Jump method
    //    if (jumpRequest)
    //    {
    //        //Jump or fly
    //        if ((Grounder.grounded || Grounder.CanCoyote()) && !jumped && !underWater) //single-jump
    //        {
    //            jumped = true;
    //            Jump(JumpHeight);
    //        }
    //        else if (jumped  && !underWater)
    //        {
    //            flying = CanFly;     //set flying to be true, if flying is allowed
    //        }
    //        else if (CanSwim && underWater && rigid.velocity.y <= 0)
    //        {
    //            jumped = true;
    //            stopJump = false;
    //            Jump(JumpHeight);
    //        }

    //        jumpRequest = false; //(IMPORTANT) Action done, End jump call
    //    }

    //    //Variable jumping height
    //    if (stopJumpRequest)
    //    {
    //        if (!flying && jumped && !stopJump || CanSwim && underWater) 
    //        { 
    //            rigid.velocity = new Vector2(rigid.velocity.x, 0); 
    //            stopJump = true; 
    //        }
    //        stopJumpRequest = false;
    //    }

    //    //Reset Jumping when standing on the ground
    //    if(wasGrounded != Grounder.grounded)
    //    {
    //        if(Grounder.grounded){ jumped = false; stopJump = false; }
    //    }
    //    wasGrounded = Grounder.grounded;   
    //}

    //void Move_Fly()
    //{
    //    //flying-------------------------------------------
    //    if (inputJump && flying && !underWater && !Grounder.grounded && flyTime < MaxFlyTime)
    //    {
    //        flyTime += Time.deltaTime;
    //        rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Cos(flyTime * FlyWobbleSpeed) * FlyWobble);
    //    }
    //    else
    //    {
    //        flying = false;
    //    }

    //    //Set fly time to be 0 if grounded or under water
    //    flyTime = (Grounder.grounded || underWater) ? 0 : flyTime;

    //    //Set gravity scale to be 0 when flying
    //    rigid.gravityScale = (flying) ? 0 : currentGravity;
    //}


    private void Forces()   //FIFO Queue for forces...
    {
        if(playerForceQueue.Count > 0 && currentForce == null && !forceRunning)
        {
            currentForce = StartCoroutine(AddForceToRigidBody(playerForceQueue[0]));
        }

        if(playerForceQueue.Count > 0 && currentForce != null && !forceRunning)
        {
            StopCurrentPlayerForce();
        }
    }

    public void AddPlayerForceNow(PlayerForce pf)
    {
        StopCurrentPlayerForce();
        playerForceQueue.Insert(0, pf);
    }

    public void AddPlayerForceToQueue(PlayerForce pf)
    {
        playerForceQueue.Add(pf);
    }

    public void StopCurrentPlayerForce()
    {
        if (currentForce == null)
            return;

        StopCoroutine(currentForce);
        currentForce = null;
        forceRunning = false;

        playerForceQueue.RemoveAt(0);
    }

    private IEnumerator AddForceToRigidBody(PlayerForce pf)
    {
        forceRunning = true;
        canFlip = pf.flip;
        //Vector2 savedVel = rigid.velocity;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(pf.force, pf.forceMode);
        yield return new WaitForSeconds(pf.time);
        //rigid.velocity = savedVel;
        canFlip = true;
        forceRunning = false;
    }



    //MoveCon Calls
    public Vector2 GetSpeed() { return currentMovement; }   //function that returns the current speed of the player
    public bool IsApplyingForce() { return forceRunning; }


    //Rigidbody Calls
    public Vector2 RigidbodyGetVelocity() { return rigid.velocity; }
    public void RigidbodySetGravityScale(float gravity)
    {
        rigid.gravityScale = gravity;
    }    
    public void RigidbodyResetVelocity(bool X, bool Y)
    {
        rigid.velocity = new Vector2(X ? 0 : rigid.velocity.x, Y ? 0 : rigid.velocity.y);
    }


    //public bool CanFlip() { return canFlip;}
    //public void SetUnderWater(bool water) { underWater = water; } //function used on triggers to specify if object is under water
    //public bool GetUnderWater() { return underWater; }


//    //Draw gizmos in editor to see how variable changes impact the player
//    private void OnDrawGizmosSelected()
//    {
//#if UNITY_EDITOR
//        float thickness = 10.0f;
//        Color color = new Color(1, 0, 0, 0.7f);
//        Vector3 start = transform.position - Vector3.up * (col.size.y / 2);
//        Vector3 walk = start + Vector3.right * WalkSpeed / 2;
//        Vector3 jump = start + Vector3.up * JumpHeight;

//        //draw two lines for jump height and speed
//        if (drawGizmos)
//        {
//            UnityEditor.Handles.DrawBezier(start, jump, start, jump, color, null, thickness);
//            UnityEditor.Handles.DrawBezier(start, walk, start, walk, color, null, thickness);
//        }
//#endif
//    }
}
public struct PlayerForce
{
    public Vector2 force;
    public ForceMode2D forceMode;
    public float time;
    public bool flip;

    public PlayerForce(Vector2 force, ForceMode2D forceMode, float time, bool flip)
    {
        this.force = force;        
        this.forceMode = forceMode;
        this.time = time;
        this.flip = flip;
    }
};