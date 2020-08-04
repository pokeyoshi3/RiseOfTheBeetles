using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Movement Controller basically applies forces and basic input regulation.
/// May be called by many scripts that require a generated movement, such as a player or an enemy.
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

    private float flyTime;        //used to generate fly wobble effect

    private Coroutine currentForce;
    List<PlayerForce> playerForceQueue = new List<PlayerForce>();
    private bool forceRunning = false;
    private bool canFlip = true;

    private void Update()
    {        
        if(GameManager_New.instance.GetGameState() != eGameState.running)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            return; 
        }

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
        if (forceRunning)
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

    private void Forces()   //FIFO Queue for forces...
    {
        if (playerForceQueue.Count > 0 && currentForce == null && !forceRunning)
        {
            currentForce = StartCoroutine(AddForceToRigidBody(playerForceQueue[0]));
        }

        if (playerForceQueue.Count > 0 && currentForce != null && !forceRunning)
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
        Vector2 savedVel = rigid.velocity;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(pf.force, pf.forceMode);
        yield return new WaitForSeconds(pf.time);
        if (pf.preserveMovement) { rigid.velocity = savedVel; }
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
    public bool preserveMovement;

    public PlayerForce(Vector2 force, ForceMode2D forceMode, float time, bool flip, bool preserveMovement)
    {
        this.force = force;
        this.forceMode = forceMode;
        this.time = time;
        this.flip = flip;
        this.preserveMovement = preserveMovement;
    }
};