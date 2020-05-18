using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{    
    //HACK temporary ui bullshit for gate 1
    [SerializeField]
    private UnityEngine.UI.Text playerResValue;
    [SerializeField]
    private UnityEngine.UI.Text baseResValue;

    protected GameManager() { }
    public eGameState GameState { get; protected set; }
    public int ResourcesOnPlayer { get; protected set; }
    public int ResourcesInBase { get; protected set; }

    private void Awake()
    {
        ChangeGameState(eGameState.paused);
    }

    private void Start()
    {
        Collectible.OnRessourceCollect += UpdatePlayerResources;
        ChangeGameState(eGameState.running);
    }

    public void UpdateBaseResources()
    {
        ResourcesInBase += ResourcesOnPlayer;
        ResourcesOnPlayer = 0;
        UpdateResourceUI();
    }

    public void UpdatePlayerResources(int value, bool gain)
    {
        if (gain)
            ResourcesOnPlayer += value;
        else
            ResourcesOnPlayer -= value;

        UpdateResourceUI();
    }

    public void ChangeGameState(eGameState newGameState)
    {
        GameState = newGameState;
    }

    private void UpdateResourceUI()
    {
        playerResValue.text = ResourcesOnPlayer.ToString();
        baseResValue.text = ResourcesInBase.ToString();
    }
}

