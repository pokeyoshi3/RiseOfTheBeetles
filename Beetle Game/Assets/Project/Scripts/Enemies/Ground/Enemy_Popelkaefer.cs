using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Popelkaefer : IEnemy
{
    [Header("Popelkäfer Setup")]
    public float walkSpeed;
    [Range(0, 100)]
    public float boundaryLeft;
    [Range(0, 100)]
    public float boundaryRight;

    float startX;
    bool turned = false;
    int direction { get { return(turned ? -1 : 1); } }
    bool gotStomped = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        moveCon.RigidbodySetGravityScale(gravity);
        startX = transform.position.x;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager_New.instance.GetGameState() != eGameState.running)
            return;

        if ((transform.position.x > startX + boundaryRight && !turned || transform.position.x < startX - boundaryLeft && turned))
        {
            turned = !turned;
        }

        moveCon.Walk(walkSpeed * direction, 0.3f);
    }

    public void StompDamage(int damage)
    {
        if (!gotStomped)
        {
            ApplyDamage(damage);
            StartCoroutine(WaitForStompTime(0.2f));
            gotStomped = true;
        }
    }

    private IEnumerator WaitForStompTime(float time)
    {
        yield return new WaitForSeconds(time);
        gotStomped = false;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = new Color(1, 0, 0, 0.3f);
        UnityEditor.Handles.DrawSolidDisc(transform.position + Vector3.right * boundaryRight, transform.forward, 0.2f);
        UnityEditor.Handles.color = new Color(0, 1, 0, 0.3f);
        UnityEditor.Handles.DrawSolidDisc(transform.position + -Vector3.right * boundaryLeft, transform.forward, 0.2f);
#endif
    }
}
