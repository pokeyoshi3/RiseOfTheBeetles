using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimOnStart : MonoBehaviour
{
    public Animator anim;
    public string clip;

    void Start()
    {
        anim.Play(clip, 0, 0.0f);
    }
}
