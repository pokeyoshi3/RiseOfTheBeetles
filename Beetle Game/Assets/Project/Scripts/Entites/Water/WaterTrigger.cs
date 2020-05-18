using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("Player"))
        {
            collision.transform.SendMessage("SetUnderWater", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.tag.Equals("Player")) 
        { 
            collision.transform.SendMessage("SetUnderWater", false);
        }
    }
}
