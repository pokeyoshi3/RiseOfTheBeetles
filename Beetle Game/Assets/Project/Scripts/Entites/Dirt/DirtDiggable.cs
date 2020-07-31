using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtDiggable : MonoBehaviour
{
    public bool isDiggable;

    public void SetColliderTrigger(bool dig)
    {
        if(isDiggable)
        {
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (Collider2D c in cols)
            {
                c.isTrigger = dig;
            }
        }
    }
}
