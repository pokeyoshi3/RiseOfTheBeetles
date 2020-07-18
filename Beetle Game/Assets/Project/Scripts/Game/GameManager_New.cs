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

    public PlayerController playerInstance { get { return playerObject != null ? playerObject.GetComponent<PlayerController>() : null; } }
    private GameObject playerObject;

    public AbilityManager abilityManager;

    private int ResourcesInBase;
    private int BugsInBase;

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
            abilityManager.UnlockAbility(eAbility.claw);
            abilityManager.ToggleAbilityActive(eAbility.claw, true);
            abilityManager.UnlockAbility(eAbility.horn);
            abilityManager.ToggleAbilityActive(eAbility.horn, true);
            abilityManager.UnlockAbility(eAbility.water);
            abilityManager.ToggleAbilityActive(eAbility.water, true);
            abilityManager.UnlockAbility(eAbility.wings);
            abilityManager.ToggleAbilityActive(eAbility.wings, true);

            GetPlayerInstance().SetAbilities();
        }

        //
        if(Input.GetKeyDown(KeyCode.F2))
        {
            AddResourcesToBase(1);
        }
    }

    public void UI_Update()
    {
        ui.Base_SetResources(ResourcesInBase);
        ui.Base_SetBug(BugsInBase);
    }

    public void UI_Update_Player(string name, int health, int resources)
    {
        ui.Player_SetName(name);
        ui.Player_SetResources(resources);
        ui.Player_SetHealth(health);
    }

    public void SetGameState(eGameState state)
    {
        gameState = state;
    }

    public eGameState GetGameState()
    {
        return gameState;
    }

    public void AddResourcesToBase(int amount)
    {
        ResourcesInBase += amount;
        UI_Update();
    }

    public int GetResourcesInBase()
    {
        return ResourcesInBase;
    }

    public PlayerController GetPlayerInstance()
    {
        return (playerInstance != null) ? playerInstance : null;
    }
}
