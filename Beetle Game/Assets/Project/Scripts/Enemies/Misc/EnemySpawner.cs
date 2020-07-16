using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// this class keeps one enemy spawned all the time
    /// </summary>

    public GameObject prefab;
    public float time;

    private GameObject instance;
    private float respawnTimer;


    // Update is called once per frame
    void Update()
    {
        if(instance == null)
        {
            respawnTimer += Time.deltaTime;

            if (respawnTimer >= time) { instance = Instantiate(prefab, transform.position, Quaternion.identity); }
        }
        else { respawnTimer = 0; }
    }
}
