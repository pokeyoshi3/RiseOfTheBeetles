using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_New : MonoBehaviour
{
    public static GameManager_New instance;
    private eGameState gameState = eGameState.paused;

    [Header("Setup")]
    public CameraController mainCamera;
    public PlayerUI ui;
    public Transform playerSpawn;
    public GameObject playerPrefab;

    public PlayerController playerInstance { get { return playerObject.GetComponent<PlayerController>(); } }
    private GameObject playerObject;
    private int ResourcesInBase;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        GameStateUpdate();
    }

    void GameStateUpdate()
    {
        switch (gameState)
        {
            case eGameState.running:
                break;
            case eGameState.paused:
                break;
            case eGameState.loading:
                break;
            case eGameState.minigame:
                break;
            case eGameState.cutscene:
                break;
        }
    }

    public void SpawnPlayer()
    {
        //Spawn a player instance
        if(playerObject == null)
        {
            playerObject = Instantiate(playerPrefab, playerSpawn.transform.position, Quaternion.identity);
            mainCamera.SetCameraLookAt(playerObject.transform, true);
        }
    }

    public void SetGameState(eGameState state)
    {
        gameState = state;
    }
}
