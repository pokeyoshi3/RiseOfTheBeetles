using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBullet : MonoBehaviour
{
    public float BulletDamage;
    public float BulletSpeed;

    private Vector3 playerdir;
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerdir = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        playerdir.Normalize();

        rigid.AddRelativeForce(playerdir * BulletSpeed);

        Destroy(this.gameObject, 5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SendMessage("PlayerDamage", BulletDamage, SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
    }
}
