using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Queen))]
public class QueenContact : MonoBehaviour
{
    private Queen queen;
    private AbilityManager AM;
    private GameManager_New GM;
    private void Awake()
    {
        queen = GetComponent<Queen>();
    }

    private void Start()
    {
        AM = AbilityManager.Instance;
        GM = GameManager_New.instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject.name + " has touched " + this.gameObject.name);

        if (other.GetComponent<PlayerController>())
        {
            queen.ShowInfo(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            queen.ShowInfo(false);
        }
    }    
}
