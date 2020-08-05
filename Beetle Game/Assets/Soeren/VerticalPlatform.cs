using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private CompositeCollider2D col;

    //public float waitTime;
    public bool insideTrigger = false;
    public bool fallDown = false;
    private bool didPressDown = false;

    void Start()
    {
        col = GetComponent<CompositeCollider2D>();
    }
    
    void Update()
    {
        didPressDown = (Input.GetAxisRaw("Vertical") < 0 && !insideTrigger);

        col.isTrigger = (insideTrigger && fallDown || didPressDown || Input.GetButton("Jump"));

        if(didPressDown && !fallDown)
        {
            fallDown = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            insideTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            insideTrigger = false;
            fallDown = false;
        }
    }
}
