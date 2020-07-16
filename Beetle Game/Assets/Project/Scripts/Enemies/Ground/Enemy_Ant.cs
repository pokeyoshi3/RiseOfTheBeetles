using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ant : IEnemy
{
    [Header("Popelkäfer Setup")]
    public float walkSpeed;
    public float jumpHeight;
    [Range(0, 100)]
    public float boundaryLeft;
    [Range(0, 100)]
    public float boundaryRight;

    [Header("Projectile Setup")]
    public float spawnTime;
    public Transform projectileSpawn;
    public GameObject projectile;


    float startX;
    bool turned = false;
    int direction { get { return (turned ? -1 : 1); } }
    bool gotStomped = false;

    public float projectileTimer;


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

        if (playerIsInRange(distanceTrigger))
        {
            PlayerController player = GameManager_New.instance.GetPlayerInstance();

            if (player.transform.position.x > transform.position.x)
                turned = false;
            else if (player.transform.position.x > transform.position.x)
                turned = true;
        }
        else
        {
            if ((transform.position.x > startX + boundaryRight && !turned || transform.position.x < startX - boundaryLeft && turned))
            {
                turned = !turned;
            }
        }

        moveCon.Walk(walkSpeed * direction, 0.3f);

        if(playerIsInRange(distanceTrigger))
        {
            Shoot();

            if(moveCon.RigidbodyGetVelocity() == Vector2.zero && moveCon.Ground.grounded)
            {
                if(checkRay(Vector2.right * direction, 1f, "Untagged", LayerMask.GetMask("Ground")))
                moveCon.Jump(jumpHeight);
            }
        }
    }
    void Shoot()
    {
        projectileTimer += Time.deltaTime;

        if (projectileTimer >= spawnTime)
        {
            GameObject bullet = Instantiate(projectile, projectileSpawn.position, Quaternion.identity, null) as GameObject;
            projectileTimer = 0;
        }
    }

    private GameObject checkRay(Vector2 dir, float distance, string tag, LayerMask layer)
    {
        Vector2 dn = dir.normalized;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, dn, distance, layer);

        if (Physics2D.Raycast(transform.position, dn, distance, layer))
        {
            if (ray.transform.tag.Equals(tag)) { return ray.transform.gameObject; }
        }

        return null;
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
        UnityEditor.Handles.color = new Color(0, 1, 1, 0.3f);
        UnityEditor.Handles.DrawSolidDisc(projectileSpawn.position, transform.forward, 0.2f);
#endif
    }
}
