using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    protected VFXManager() { }

    public ParticleSystem[] VFX_List;

    public void SpawnVFX(eVFX VFX)
    {
        ParticleSystem spawnedVFX = Instantiate(VFX_List[(int)VFX]);
    }
}

public enum eVFX
{
    playerHurt = 0,
    blockDestroyed = 1,
    somethingElse = 2
}
