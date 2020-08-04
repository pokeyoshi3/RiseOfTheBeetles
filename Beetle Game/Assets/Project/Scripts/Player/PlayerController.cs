using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public NameGenerator nameGenerator;

    [Header("Player Setup")]
    public int MaxHealth;

    [Header("Base Movement")]
    public float PlayerGravityScale;
    public float WalkSpeed;
    public float JumpHeight;
    public int JumpDamage;

    [Header("Horns")]
    public bool HasHorns;
    public GameObject HornMesh;
    public int HornDamage;
    public float HornDistance;
    public float HornRadius;
    public GameObject HornParticle;

    [Header("Claws")]
    public bool HasClaws;
    public GameObject ClawsMesh;
    public float ClawDistance;
    public float ClawDamage;

    [Header("Wings")]
    public bool HasWings;
    public GameObject WingsMesh;
    public float WingsMaxFlyTime = 3f;
    public float WingsBonusJumpHeight = 1f;

    [Header("Fins")]
    public bool HasFins;
    public GameObject FinsMesh;
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
    //
    [SerializeField]
    private int health;
    private int resourcesOnPlayer;
    private string bugName;

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

    //claws
    private bool digging = false;

    //shortcut variables
    private bool inputPointingDown { get { return (inputAxis.y < 0); } }    //check if axis input is pointing down
    private float currentGravity { get { return (underWater ? PlayerGravityScale * FinsGravityMultiplier : PlayerGravityScale); } }
    private float currentJumpHeight { get { return (HasWings ? JumpHeight + WingsBonusJumpHeight : JumpHeight); } }
    private float currentMoveSpeed { get { return (underWater ? WalkSpeed * FinsSpeedMultiplier : WalkSpeed); } }

    //misc
    [SerializeField]
    private bool wasGrounded = false;   //Trigger variable for isGrounded changes
    [SerializeField]
    private bool didJumpDamage = false;
    //*********************************************


    // Start is called before the first frame update
    void Start()
    {
        //Coyote Setup
        MoveCon.Ground.SetCoyoteTime(CoyoteJumpTime);

        //AbilitySetup
        SetAbilities();
        
        //Stats setup
        health = MaxHealth;
        bugName = nameGenerator.NewBugName();
        resourcesOnPlayer = 0;

        UpdateUIToGameManager();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager_New.instance.GetGameState() == eGameState.running)
        {
            InputDetection();
            Abilities();
            JumpDamaging();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager_New.instance.GetGameState() == eGameState.running)
        {
            Movement_Basic();
        }
    }

    private void InputDetection()
    {
        inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool pointingDown = (Input.GetAxisRaw("Vertical") < 0);

        if (Input.GetButtonDown("Fire1"))
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
        MoveCon.RigidbodySetGravityScale((flying || digging) ? 0 : currentGravity);

        Move_Walk();
        Move_Jump();
        Move_Fly();
    }

    private void Move_Walk()
    {
        MoveCon.Walk(inputAxis.x * currentMoveSpeed, AccelerationFluidity);
        AnimCon.animController.SetBool("Swimming", underWater);
    }

    private void Move_Jump()
    {
        GameObject groundedOn;

        //Reset Jumping when standing on the ground
        if (wasGrounded != MoveCon.Ground.grounded)
        {
            if (!jumped && !MoveCon.Ground.grounded)
            {
                AnimCon.animController.Play("Fall", 0, 0);
            }

            groundedOn = (MoveCon.Ground.grounded ? MoveCon.Ground.groundedObjects[0].gameObject : null);
            if (MoveCon.Ground.grounded && !groundedOn.CompareTag("EnemyHead")) { jumped = false; jumpStopped = false; }
        }
        wasGrounded = MoveCon.Ground.grounded;

        //Jumping: Wait for button press to call the Jump method
        if (inputRequestJump)
        {
            groundedOn = (MoveCon.Ground.grounded ? MoveCon.Ground.groundedObjects[0].gameObject : null);
            //Jump or fly
            if ((MoveCon.Ground.grounded || MoveCon.Ground.CanCoyote()) && !jumped && !underWater) //single-jump
            {
                if (groundedOn != null && !groundedOn.CompareTag("EnemyHead") || MoveCon.Ground.CanCoyote())
                {
                    jumped = true;
                    MoveCon.Jump(currentJumpHeight);

                    AnimCon.animController.Play("Jump", 0, 0);
                }
            }
            else if (HasWings && jumped && !underWater)
            {
                flying = HasWings;     //set flying to be true, if flying is allowed
            }
            else if (HasFins && underWater && MoveCon.RigidbodyGetVelocity().y <= 0) //underwater jumping
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

            if(flying)
            {
                flyTime = WingsMaxFlyTime;
            }

            if (MoveCon.Ground.grounded)
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
        AnimCon.animController.SetBool("Flying", flying);
    }

    private void Abilities()
    {
        if (inputAction)
        {
            if (!MoveCon.IsApplyingForce())
            {
                GameObject wallfront = checkRay(inputAxis, ClawDistance, "Dirt", LayerMask.GetMask("Ground"));
                if (HasClaws && wallfront != null) { Action_Claws(wallfront, inputAxis); }
                else if (HasHorns) { Action_Horns(); }
            }
            inputAction = false;
        }
    }

    private GameObject checkRay(Vector2 dir, float distance, string tag, LayerMask layer)
    {
        Vector2 dn = dir.normalized;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, dn, distance, layer);

        if(Physics2D.Raycast(transform.position, dn, distance, layer))
        {
            if (ray.transform.tag.Equals(tag)) { return ray.transform.gameObject; }
        }
        
        return null;
    }

    private bool checkCircle(float radius, string tag, LayerMask layer)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, radius, layer);

        foreach(Collider2D col in cols)
        {
            if (col.transform.tag.Equals(tag)) { return true; }
        }

        return false;
    }

    private void JumpDamaging()
    {
        if (MoveCon.Ground.grounded)
        {
            GameObject stoodOn = MoveCon.Ground.groundedObjects[0].gameObject;

            if (stoodOn.CompareTag("EnemyHead") && MoveCon.RigidbodyGetVelocity().y < 0)
            {
                stoodOn.SendMessageUpwards("StompDamage", JumpDamage, SendMessageOptions.DontRequireReceiver);
                MoveCon.Jump(JumpHeight);
                AnimCon.animController.Play("Jump", 0, 0);
                jumped = true;
            }
        }
    }

    public void Action_Horns()
    {
        float hornDelay = 0.1f;
        float hornCool = 0.1f;

        Instantiate(HornParticle, transform.position, Quaternion.Euler(0, 0, Mathf.Atan2(inputAxis.y, inputAxis.x) * Mathf.Rad2Deg));

        MoveCon.AddPlayerForceToQueue(new PlayerForce(Vector2.zero, ForceMode2D.Impulse, hornDelay, true, false));
        SendMessageOverRayCastCircle(0.10f, inputAxis * HornDistance, HornRadius, "ApplyDamage", HornDamage);
        MoveCon.AddPlayerForceToQueue(new PlayerForce(Vector2.zero, ForceMode2D.Impulse, hornCool, false, false));
    }

    public void Action_Claws(GameObject dirt, Vector2 dir)
    {
        Debug.Log("Wall detected and called");
        //MoveCon.AddPlayerForceToQueue(new PlayerForce(inputAxis * 10, ForceMode2D.Impulse, 0.30f, true));
        //SendMessageOverRayCast(0.35f, Vector2.down, ClawDistance, "ApplyDamage", ClawDamage);

        if(!digging)
        { 
            StartCoroutine(Dig(dirt, dir));
        }
    }

    IEnumerator Dig(GameObject dirt, Vector2 dir)
    {
        Vector2 force = dir * 7;
        float time = 0.2f;
        bool failed = false;

        digging = true;
        dirt.GetComponent<DirtDiggable>().SetColliderTrigger(true);

        while(digging)
        {
            MoveCon.AddPlayerForceToQueue(new PlayerForce(force * (failed ? -0.1f : 0.1f), ForceMode2D.Impulse, time, false, false));
            MoveCon.AddPlayerForceToQueue(new PlayerForce(force * (failed ? -1 : 1), ForceMode2D.Impulse, time, false, false));
            yield return new WaitForSeconds(time * 2);
            digging = checkCircle(0.3f, "Dirt", LayerMask.GetMask("Ground"));

            if(Mathf.Abs(MoveCon.RigidbodyGetVelocity().magnitude) <= 0.1f)
            {
                failed = true;
            }
        }

        dirt.GetComponent<DirtDiggable>().SetColliderTrigger(false);
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
            hit.transform.SendMessageUpwards(message, value, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void SendMessageOverRayCastCircle(float afterTime, Vector2 dir, float distance, string message, object value)
    {
        StartCoroutine(RaycastMessageCircle(afterTime, dir, distance, message, value));
    }

    private IEnumerator RaycastMessageCircle(float time, Vector3 point, float radius, string message, object value)
    {
        yield return new WaitForSeconds(time);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + point, radius, CanHitObjects);

        foreach (Collider2D hit in hits)
        {
            hit.transform.SendMessageUpwards(message, value, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void PlayerDamage(int damage)
    {
        health -= damage;

        MoveCon.AddPlayerForceToQueue(new PlayerForce(-inputAxis * 4, ForceMode2D.Impulse, 0.2f, false, false));

        /*if (particlesDamage != null) { Instantiate(particlesDamage, transform.position, Quaternion.identity); }

        if (audioTookDamage != null) { audioSrc.PlayOneShot(audioTookDamage); }
        */

        UpdateUIToGameManager();

        if (health <= 0) { Destroy(this.gameObject); }
    }

    public void SetAbilities()
    {
        HasHorns = GameManager_New.instance.abilityManager.IsAbilityUnlocked(eAbility.horn);
        HasClaws = GameManager_New.instance.abilityManager.IsAbilityUnlocked(eAbility.claw);
        HasWings = GameManager_New.instance.abilityManager.IsAbilityUnlocked(eAbility.wings);
        HasFins = GameManager_New.instance.abilityManager.IsAbilityUnlocked(eAbility.water);

        UpdatePlayerLook();
    }
    public void SetUnderWater(bool water) { underWater = water; } //function used on triggers to specify if object is under water
    public void AddResourcesToPlayer(int amount) 
    {
        resourcesOnPlayer += amount; 
        UpdateUIToGameManager();
    }
    public void UpdateUIToGameManager()
    {
        GameManager_New.instance.UI_Update_Player(bugName, health, resourcesOnPlayer);
    }

    public void DepleteResourcesToBase()
    {
        GameManager_New.instance.AddResourcesToBase(resourcesOnPlayer);
        resourcesOnPlayer = 0;

        UpdateUIToGameManager();
    }

    public void UpdatePlayerLook()
    {
        HornMesh.SetActive(HasHorns);
        WingsMesh.SetActive(HasWings);
        ClawsMesh.SetActive(HasClaws);
        FinsMesh.SetActive(HasFins);
    }
}
