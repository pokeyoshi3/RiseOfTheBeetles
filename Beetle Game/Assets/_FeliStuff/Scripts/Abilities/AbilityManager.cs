using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private Ability[] abilities;
    
    public int activeAbilities { get; protected set; }

    private void Awake()
    {
        abilities = new Ability[4];
        SetAbilityCounter(0);
        WriteAbilityList();
    }

    private void Start()
    {
        //player = FindObjectOfType<PlayerController>();
        //playerSprite = player.GetComponentInChildren<SpriteRenderer>();
        //playerSprite.sprite = GumbaPlayer;
        //player.HasWings = player.HasHorns = player.HasFins = player.HasClaws = false;
        //Board.OnGameEnd += ToggleAbilityActive;
    }

    public void WriteAbilityList()
    {
        abilities[(int)eAbility.wings] = new Ability(eAbility.wings, false, false);
        abilities[(int)eAbility.horn] = new Ability(eAbility.horn, false, false);
        abilities[(int)eAbility.water] = new Ability(eAbility.water, false, false);
        abilities[(int)eAbility.claw] = new Ability(eAbility.claw, false, false);
    }

    public void SetAbilityCounter(int newCount)
    {
        activeAbilities = newCount;
    }

    public bool IsAbilityActive(eAbility ability)
    {
        if (abilities[(int)ability].active)
            return true;

        return false;
    }

    public bool IsAbilityUnlocked(eAbility ability)
    {
        if (abilities[(int)ability].unlocked)
            return true;

        return false;
    }

    public void UnlockAbility(eAbility ability)
    {
        abilities[(int)ability].Unlock();
    }

    public void ToggleAbilityActive(eAbility ability, bool active)
    {
        abilities[(int)ability].ToggleActive(active);
        SetAbilityCounter(activeAbilities + 1);

        switch (ability)
        {
            case eAbility.wings:
                GameManager_New.instance.GetPlayerInstance().HasWings = true;
                break;
            case eAbility.horn:
                GameManager_New.instance.GetPlayerInstance().HasHorns = true;
                break;
            case eAbility.water:
                GameManager_New.instance.GetPlayerInstance().HasFins = true;
                break;
            case eAbility.claw:
                GameManager_New.instance.GetPlayerInstance().HasClaws = true;
                break;
            default:
                break;
        }

        //PlayerFunc---> Update Look!
        GameManager_New.instance.GetPlayerInstance().UpdatePlayerLook();
    }

    //public void SetPassiveAbility(Color newCol)
    //{
    //    playerSprite.color = newCol;
    //}


    //private void ToggleAbilityLock(Ability ability, bool isUnlocked)
    //{
    //    if (!allAbilities.ContainsKey(ability))
    //    {
    //        print(ability.AbilityName + " does not exist in dictionary D:");
    //        return;
    //    }
    //    allAbilities[ability] = isUnlocked;
    //}

    //private bool IsAbilityUnlocked(Ability ability)
    //{
    //    if (allAbilities[ability])
    //        return true;

    //    return false;
    //}

    //private void ToggleAbilityActive(Ability ability, bool isActive)
    //{
    //    if (!IsAbilityUnlocked(ability))
    //        return;

    //    if (ActiveAbilities.Contains(ability))
    //        return;
    //    else if (ActiveAbilities.Count == 2)
    //        return;
    //    else
    //    {
    //        ActiveAbilities.Add(ability);
    //        print(ability.AbilityName + " is now in use");
    //        //UpdateParts(ability.AbilityType, true);
    //    }
    //}

    //private void UpdateParts(eAbility ability, bool active)
    //{
    //    switch (ability)
    //    {
    //        case eAbility.wings:
    //            foreach(GameObject o in wings)
    //            {
    //                o.SetActive(active);
    //            }

    //            break;
    //        case eAbility.horn:
    //            foreach (GameObject o in horn)
    //            {
    //                o.SetActive(active);
    //            }


    //            break;
    //        case eAbility.water:
    //            foreach (GameObject o in water)
    //            {
    //                o.SetActive(active);
    //            }

    //            break;
    //        case eAbility.claw:
    //            foreach (GameObject o in claw)
    //            {
    //                o.SetActive(active);
    //            }


    //            break;
    //        default:
    //            break;
    //    }
    //}
}

