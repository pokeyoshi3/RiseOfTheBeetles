using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    /// <summary>
    /// this script fades the alpha of a canvasgroup, used for black transition screen
    /// </summary>

    public CanvasGroup canvasGroup;
    public bool blendActive;
    public float blendSpeed = 1;

    // Update is called once per frame

    void Update()
    {
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, blendActive ? 1 : 0, blendSpeed * Time.deltaTime);
    }
}
