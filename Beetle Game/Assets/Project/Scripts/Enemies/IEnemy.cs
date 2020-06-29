using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController), typeof(AudioSource), typeof(Rigidbody2D))]
public class IEnemy : MonoBehaviour
{
    protected MovementController moveCon;
    protected AudioSource audioSrc;

    [Header("Enemy Setup")]
    public int maxHealth;
    public int damageOnAttack;
    public float distanceTrigger;
    public float gravity;

    [Header("Prefabs")]
    public AudioClip audioTookDamage;
    public GameObject particlesDamage;
    public GameObject prefabDeath;

    [SerializeField]
    protected int health;

    protected Transform player;

    protected void Awake()
    {
        moveCon = GetComponent<MovementController>();
        audioSrc = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = maxHealth;
    }

    // Can be overridden by new enemy type
    protected virtual void Start(){ moveCon.RigidbodySetGravityScale(gravity); }
    protected virtual void Update(){}
    protected virtual void FixedUpdate() {}

    public virtual void ApplyDamage(int damage)
    {
        health -= damage;

        Debug.Log("Enemy took damage: " + damage);

        if(particlesDamage != null) { Instantiate(particlesDamage, transform.position, Quaternion.identity); }

        if (audioTookDamage != null) { audioSrc.PlayOneShot(audioTookDamage); }

        if(health <= 0)
        {
            if(prefabDeath != null) { Instantiate(prefabDeath, transform.position, Quaternion.identity); }
            Destroy(gameObject);
        }
    }

    protected bool playerIsInRange(float range)
    {
        return (Mathf.Abs(Vector2.Distance(transform.position, player.position)) <= range);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("Player") && !collision.otherCollider.CompareTag("EnemyHead"))
        {
            collision.transform.SendMessage("PlayerDamage", damageOnAttack, SendMessageOptions.DontRequireReceiver);
        }
    }
}
