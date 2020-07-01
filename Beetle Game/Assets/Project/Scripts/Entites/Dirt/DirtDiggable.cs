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
            GetComponent<Collider2D>().isTrigger = dig;
        }
    }
}
