using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoGoal : MonoBehaviour
{
    public CanvasGroup BlackScreen;
    public GameObject EndText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>())
        {
            StartCoroutine(DemoEnd());
        }
    }

    private IEnumerator DemoEnd()
    {
        GameManager_New.instance.SetGameState(eGameState.paused);

        while (BlackScreen.alpha < 1.0f)
        {
            BlackScreen.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        EndText.SetActive(true);
    }
}
