using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    private int value = 1;
    public static event System.Action<int, bool> OnRessourceCollect = delegate { };
    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name + " has touched " + this.gameObject.name);
        if (other.GetComponent<PlayerController>())
        {
            OnRessourceCollect(value, true);
            Destroy(this.gameObject, 0.15f);
        }
    }
}
