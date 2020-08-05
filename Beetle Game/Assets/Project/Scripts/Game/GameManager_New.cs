using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_New : MonoBehaviour
{
    public static GameManager_New instance;
    private eGameState gameState = eGameState.running;

    [Header("Setup")]
    public CameraController mainCamera;
    public PlayerUI ui;
    public Transform playerSpawn;
    public GameObject playerPrefab;
    public CanvasFader canvasFader;

    public PlayerController playerInstance { get { return playerObject != null ? playerObject.GetComponent<PlayerController>() : null; } }
    private GameObject playerObject;

    public AbilityManager abilityManager;

    public MinigameManager minigameManager;

    public DialogueController dialogueController; //Actually supposed to be named dialogueManager but who cares it's one day before goldmaster and i'm tired

    public FastTravelManager fastTravelManager;

    private int ResourcesInBase;
    private int BugsInBase;

    private eGameState lastGameState;
    private int crystalCount;

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
        Cheats();
    }

    void GameStateUpdate()
    {
        switch (gameState)
        {
            case eGameState.running:
                if (playerObject == null)
                {
                    SpawnPlayer();
                }
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

    int healthtest = 6;

    public void SpawnPlayer()
    {
        //Spawn a player instance
        if(playerObject == null)
        {
            playerObject = Instantiate(playerPrefab, playerSpawn.transform.position, Quaternion.identity);
            mainCamera.SetCameraLookAt(playerObject.transform, true);
        }
    }

    public void Cheats()
    {
        //GIF ME ALL THE PRINGELS
        if (Input.GetKeyDown(KeyCode.F1))
        {
            abilityManager.UnlockEverything();
        }

        //GIF ME MAHNEY
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GetPlayerInstance().AddResourcesToPlayer(100);
        }
    }

    public void UI_Update_Player(string name, int health, int resources)
    {
        ui.Player_SetName(name);
        ui.Player_SetResources(resources);
        ui.Player_SetHealth(health);
        ui.SetAbilitySprites();
    }

    public void SetGameState(eGameState state)
    {
        if(state != gameState)
        {
            lastGameState = gameState;
            gameState = state;
        }
    }

    public eGameState GetGameState()
    {
        return gameState;
    }

    public void RevertGameState()
    {
        gameState = lastGameState;
    }

    public PlayerController GetPlayerInstance()
    {
        return (playerInstance != null) ? playerInstance : null;
    }

    public void GetCrystal()
    {
        crystalCount++;
    }
}
