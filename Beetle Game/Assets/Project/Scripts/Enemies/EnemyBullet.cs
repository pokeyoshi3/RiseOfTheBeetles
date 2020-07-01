using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    public float BulletDamage;
    public float BulletSpeed;
    public float LifeTime;
    public bool onlyPlayerDestroy;

    private Vector3 playerdir;
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerdir = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        playerdir.Normalize();

        rigid.AddRelativeForce(playerdir * BulletSpeed);

        Destroy(this.gameObject, LifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SendMessage("PlayerDamage", BulletDamage, SendMessageOptions.DontRequireReceiver);
        if (collision.transform.CompareTag("Player") && onlyPlayerDestroy || !onlyPlayerDestroy)
        {
            Destroy(this.gameObject);
        }
    }
}
