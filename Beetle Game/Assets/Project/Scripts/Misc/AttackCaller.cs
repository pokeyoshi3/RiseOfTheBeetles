using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCaller : MonoBehaviour
{
    // Start is called before the first frame update

    public int Health;
    public GameObject DamageParticle;
    public GameObject DeathParticle;

    public void ApplyDamage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Death();
        }
        else
        {
            Instantiate(DamageParticle, transform.position + Vector3.up * 2, Quaternion.identity);
        }
    }

    public void Death()
    {
        Instantiate(DeathParticle, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
