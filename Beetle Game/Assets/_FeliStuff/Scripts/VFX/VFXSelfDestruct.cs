using UnityEngine;

public class VFXSelfDestruct : MonoBehaviour
{
    [SerializeField]
    private bool isTimed;
    [SerializeField]
    private float time;
    void Update()
    {
        if (!isTimed)
            Destroy(this.gameObject, GetComponentInChildren<ParticleSystem>().main.duration);
        else
            Destroy(this.gameObject, time);
    }
}
