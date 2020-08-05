﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the player relevant ingame UI with health, abilites and resources
/// </summary>
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Ref to the Health Petals; [0] = 1 life, [5] = 6 lives")]
    private HP_Petal[] petalsForHP;
    [SerializeField]
    [Tooltip("Image for the ability UI")]
    private Image abilityIconOne;
    [SerializeField]
    [Tooltip("Image for the ability UI")]
    private Image abilityIconTwo;
    [SerializeField]
    private Text resOnPlayer;
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Sprite[] abilitySprites;
    [SerializeField]
    private Canvas mainCanv;

    private int UIhealth = 6;

    private void Update()
    {
        mainCanv.enabled = (GameManager_New.instance.GetGameState() == eGameState.running || GameManager_New.instance.GetGameState() == eGameState.cutscene);
    }

    ////HACK for testing purposes
    //int testMaxHP = 6;    
    //public void TestHP(bool lose)
    //{
    //    if (lose)
    //    {
    //        if (testMaxHP == 0)
    //            print("no HP left");
    //        else
    //        {
    //            ChangeHP(testMaxHP, testMaxHP - 1);
    //            testMaxHP -= 1;
    //        }
    //    }
    //    else
    //    {
    //        if (testMaxHP == 6)
    //            print("full hp reached");
    //        else
    //        {
    //            ChangeHP(testMaxHP, testMaxHP + 1);
    //            testMaxHP += 1;
    //        }
    //    }
    //}

    public void Player_SetName(string n)
    {
        playerName.text = n;
    }

    public void Player_SetResources(int resources)
    {
        resOnPlayer.text = resources.ToString();
    }

    public void Player_SetHealth(int health)
    {
        ChangeHP(UIhealth, health);

        UIhealth = health;
        if (UIhealth <= 0) { UIhealth = 0; }
    }

    public void SetAbilitySprites()
    {
        bool foundfirst = false;
        bool foundsecond = false;
        int first = 0;

        //first one
        for (int i = 0; i < 4; i++)
        {
            if (GameManager_New.instance.abilityManager.IsAbilityActive((eAbility)(i)))
            {
                first = i;
                foundfirst = true;
                abilityIconOne.sprite = abilitySprites[i];
                break;
            }
        }
        //second one
        for (int i = 0; i < 4; i++)
        {
            if (GameManager_New.instance.abilityManager.IsAbilityActive((eAbility)(i)) && i != first)
            {
                foundsecond = true;
                abilityIconTwo.sprite = abilitySprites[i];
                break;
            }
        }

        abilityIconOne.enabled = foundfirst;
        abilityIconTwo.enabled = foundsecond;
    }
    /// <summary>
    /// Changes HP UI according to health loss/gain
    /// Currently assumes that you lose only one petal per call
    /// </summary>
    private void ChangeHP(int prevHP, int newHP)
    {      
        //lost health
        if (prevHP > newHP)
        {
            int lost = prevHP - newHP;

            for (int i = 0; i < lost; i++)
            {
                if (newHP > 0)
                {
                    petalsForHP[newHP + i].Fall();
                }
            }

            //if (prevHP-newHP == 1)
            //    petalsForHP[newHP].Fall();
        }
        //regained health
        else if (prevHP < newHP)
        {
            int gained = newHP - prevHP;

            for(int i = 0; i < gained; i++)
            {
                if (prevHP + i < 6)
                {
                    petalsForHP[prevHP + i].Regain();
                }
            }
        }
        //else: no hp change -> no ui change
    }

    /// <summary>
    /// Changes Ability Icon 1 if first is true, else changes Ability Icon 2
    /// </summary>
    /// <param name="first"></param>
    private void SetAbilityIcons(Sprite abilityIcon, bool first)
    {
        if (first)
            abilityIconOne.sprite = abilityIcon;
        else
            abilityIconTwo.sprite = abilityIcon;
    }
}
