using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used for Managing Player ablilities, like horn and claws
/// Claw is used for digging, Horn for attacking forward
/// This class calls multiple Method of the Movement Controller.
/// </summary>

[RequireComponent(typeof(MovementController))]
public class PlayerController : MonoBehaviour
{
    [Header("Object References")]
    public MovementController MoveCon;
    public AnimationController AnimCon;

    [Header("Player Setup")]
    public int MaxHealth;

    [Header("Base Movement")]
    public float PlayerGravityScale;
    public float WalkSpeed;    

    public float JumpHeight;

    [Header("Horns")]
    public bool HasHorns;
    public int HornDamage;
    public int HornDistance;

    [Header("Claws")]
    public bool HasClaws;
    public float ClawDistance;
    public float ClawDamage;

    [Header("Wings")]
    public bool HasWings;
    public float WingsMaxFlyTime = 3f;
    public float WingsBonusJumpHeight = 1f;

    [Header("Fins")]
    public bool HasFins;
    public float FinsGravityMultiplier;
    public float FinsSpeedMultiplier;

    [Header("Atomic Values")]
    public LayerMask CanHitObjects;
    [Range(0, 1)]
    public float AccelerationFluidity;
    public float CoyoteJumpTime;
    public float FlyWobble;
    public float FlyWobbleSpeed;
    //************* PRIVATE VARIABLES *************

    //input
    private Vector2 inputAxis;  //movement axis constant input check
    private bool inputJump = false;             //jump button constant input check
    private bool inputRequestJump = false;      //jump button press trigger
    private bool inputRequestStopJump = false;  //jump button release trigger
    private bool inputAction = false;           //action button press trigger

    //jumping
    [SerializeField]
    private bool jumped = false;
    [SerializeField]
    private bool jumpStopped = false;

    //flying
    [SerializeField]
    private bool flying = false;
    private float flyTime;

    //swimming
    private bool underWater = false;

    //shortcut variables
    private bool inputPointingDown { get { return (inputAxis.y < 0); } }    //check if axis input is pointing down
    private float currentGravity { get { return (underWater ? PlayerGravityScale * FinsGravityMultiplier : PlayerGravityScale); } }
    private float currentJumpHeight { get { return (HasWings ? JumpHeight + WingsBonusJumpHeight : JumpHeight); } }
    private float currentMoveSpeed { get { return (underWater ? WalkSpeed * FinsSpeedMultiplier : WalkSpeed); } }

    //misc
    [SerializeField]
    private bool wasGrounded = false;   //Trigger variable for isGrounded changes
    
    //*********************************************


    // Start is called before the first frame update
    void Start()
    {
        MoveCon.Ground.SetCoyoteTime(CoyoteJumpTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == eGameState.running)
        {
            InputDetection();
            Abilities();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.GameState == eGameState.running)
        {
            Movement_Basic();
        }
    }

    private void InputDetection()
    {
        inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool pointingDown = (Input.GetAxisRaw("Vertical") < 0);

        if(Input.GetButtonDown("Fire1"))
        {
            inputAction = true;
        }

        //Jump (+flying) button: inputJump is a constant input lookup, jumpRequest is a trigger and disables after action        
        if (Input.GetButtonDown("Jump")) { inputRequestJump = true; }
        inputJump = Input.GetButton("Jump");
        if (Input.GetButtonUp("Jump")) { inputRequestStopJump = true; }
    }

    private void Movement_Basic()
    {
        MoveCon.RigidbodySetGravityScale((flying) ? 0 : currentGravity);

        Move_Walk();
        Move_Jump();
        Move_Fly();
    }

    private void Move_Walk()
    {
        MoveCon.Walk(inputAxis.x * currentMoveSpeed, AccelerationFluidity);
    }

    private void Move_Jump()
    {
        //Reset Jumping when standing on the ground
        if (wasGrounded != MoveCon.Ground.grounded)
        {
            if (MoveCon.Ground.grounded) { jumped = false; jumpStopped = false; }
        }
        wasGrounded = MoveCon.Ground.grounded;

        //Jumping: Wait for button press to call the Jump method
        if (inputRequestJump)
        {
            //Jump or fly
            if ((MoveCon.Ground.grounded || MoveCon.Ground.CanCoyote()) && !jumped && !underWater) //single-jump
            {
                jumped = true;
                MoveCon.Jump(currentJumpHeight);
            }
            else if (HasWings && jumped && !underWater)
            {
                flying = HasWings;     //set flying to be true, if flying is allowed
            }
            else if (HasFins && underWater && MoveCon.RigidbodyGetVelocity().y <= 0)
            {
                jumped = true;
                jumpStopped = false;
                MoveCon.Jump(currentJumpHeight);
            }

            inputRequestJump = false; //(IMPORTANT) Action done, End jump call
        }

        //Variable jumping height
        if (inputRequestStopJump)
        {
            if (MoveCon.RigidbodyGetVelocity().y > 0 && (!flying && jumped && !jumpStopped || HasFins && underWater))
            {
                MoveCon.RigidbodyResetVelocity(false, true);
                jumpStopped = true;
            }

            if(MoveCon.Ground.grounded)
            {
                jumped = false;
            }

            inputRequestStopJump = false;
        }
    }

    void Move_Fly()
    {
        //Flying triggered by jump function
        if (inputJump && flying && !underWater && !MoveCon.Ground.grounded && flyTime < WingsMaxFlyTime)
        {
            flyTime += Time.fixedDeltaTime;
            MoveCon.Fly(flyTime, FlyWobble, FlyWobbleSpeed);
        }
        else
        {
            flying = false;
        }

        //Set fly time to be 0 if grounded or under water
        flyTime = (MoveCon.Ground.grounded || underWater) ? 0 : flyTime;
    }

    private void Abilities()
    {
        if(inputAction)
        {
            if (!MoveCon.IsApplyingForce() && MoveCon.Ground.grounded)
            {
                if (inputPointingDown && HasClaws)  { Action_Claws(); }
                else if (HasHorns)                  { Action_Horns(); }
            }
            inputAction = false;
        }
    }

    public void Action_Horns()
    {
        float flipDir = AnimCon.SpriteRend.flipX ? -1 : 1;
        MoveCon.AddPlayerForceToQueue(new PlayerForce(new Vector2(flipDir, 0) * 10, ForceMode2D.Impulse, 0.10f, true));
        SendMessageOverRayCast(0.10f, new Vector2(flipDir, 0), HornDistance, "ApplyDamage", HornDamage);
        MoveCon.AddPlayerForceToQueue(new PlayerForce(new Vector2(-flipDir, 0) * 9, ForceMode2D.Impulse, 0.10f, false));
    }

    public void Action_Claws()
    {
        MoveCon.AddPlayerForceToQueue(new PlayerForce(new Vector2(0, 1) * 10, ForceMode2D.Impulse, 0.30f, true));
        SendMessageOverRayCast(0.35f, Vector2.down, ClawDistance, "ApplyDamage", ClawDamage);
        MoveCon.AddPlayerForceToQueue(new PlayerForce(new Vector2(0, -1) * 30, ForceMode2D.Impulse, 0.05f, true));
    }

    public void SendMessageOverRayCast(float afterTime, Vector2 dir, float distance, string message, object value)
    {
        StartCoroutine(RaycastMessage(afterTime, dir, distance, message, value));
    }
    
    private IEnumerator RaycastMessage(float time, Vector2 dir, float distance, string message, object value)
    {
        yield return new WaitForSeconds(time);

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, distance, CanHitObjects);

        foreach (RaycastHit2D hit in hits)
        {            
            hit.transform.SendMessageUpwards(message, value);
        }
    }

    public void SetAbilities(bool horns, bool claws, bool wings, bool fins)
    {
        HasHorns = horns;
        HasClaws = claws;
        HasWings = wings;
        HasFins = fins;
    }

    public void SetUnderWater(bool water) { underWater = water; } //function used on triggers to specify if object is under water
}
