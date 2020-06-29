using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Grounder : MonoBehaviour
{
    [Header("Grounder Setup")]
    public float grounderSize = 1.0f;
    public LayerMask ignoreCollision;

    [Header("Debug (Do not Touch)")]
    public Collider2D[] groundedObjects;
    public bool grounded;

    [SerializeField]
    private float coyoteTime = 0.0f;
    private Coroutine coyote;
    private bool wasGrounded = false;
    public bool coyoteRunning = false;

    private void Update()
    {
        DetectCollision();
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        transform.localScale = grounderSize * Vector2.one;

        DetectCollision();

        UnityEditor.Handles.color = grounded ? new Color(0, 1, 0, 0.3f) : new Color(1, 0, 0, 0.3f);
        UnityEditor.Handles.DrawSolidDisc(transform.position, transform.forward, grounderSize);
#endif
    }

    private void DetectCollision()
    {
        groundedObjects = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x, ignoreCollision);
        grounded = (groundedObjects.Length > 0);

        if (grounded != wasGrounded) //did something change?
        {
            if (!grounded) //yes, we left the ground
            {
                coyote = StartCoroutine(WaitCoyoteTime(coyoteTime));
            }
            else //yes, we hit the ground
            {
                if (coyote != null)
                {
                    StopCoroutine(coyote);
                }
                coyoteRunning = false;
            }
        }

        wasGrounded = grounded;
    }

    private IEnumerator WaitCoyoteTime(float time)
    {
        coyoteRunning = true;
        yield return new WaitForSeconds(time);
        coyoteRunning = false;
    }

    public void SetCoyoteTime(float time) { coyoteTime = time; }
    public bool CanCoyote() { return coyoteRunning; }
}