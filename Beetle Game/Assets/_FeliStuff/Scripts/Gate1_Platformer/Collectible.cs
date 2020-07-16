using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name + " has touched " + this.gameObject.name);
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.AddResourcesToPlayer(value);
            Destroy(this.gameObject);
        }
    }
}
