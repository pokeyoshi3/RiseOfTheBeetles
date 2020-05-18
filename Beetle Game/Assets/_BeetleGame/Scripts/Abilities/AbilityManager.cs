using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : Singleton<AbilityManager>
{
    protected AbilityManager() {}

    protected Ability[] abilities;
    private PlayerController player;
    public int activeAbilities { get; protected set; }

    //HACK temp for gate 1
    public CanvasGroup BlackScreen;
    private SpriteRenderer playerSprite;
    public Sprite GumbaPlayer;
    public Sprite WingPlayer;
    public Sprite WingHornPlayer;

    //private Ability[] abilities = new Ability[4];
    ////bool represents if the ability has been unlocked in the village
    //private Dictionary<Ability, bool> allAbilities = new Dictionary<Ability, bool>();
    ////List of abilities that are currently active 
    //private HashSet<Ability> ActiveAbilities = new HashSet<Ability>();
    //[SerializeField]
    //GameObject[] wings;
    //[SerializeField]
    //GameObject[] horn;
    //[SerializeField]
    //GameObject[] claw;
    //[SerializeField]
    //GameObject[] water;

    private void Awake()
    {
        abilities = new Ability[4];
        SetAbilityCounter(0);
        WriteAbilityList();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        playerSprite = player.GetComponentInChildren<SpriteRenderer>();
        playerSprite.sprite = GumbaPlayer;
        player.HasWings = player.HasHorns = player.HasFins = player.HasClaws = false;
        Board.OnGameEnd += ToggleAbilityActive;
    }

    public void WriteAbilityList()
    {
        abilities[(int)eAbility.claw] = new Ability(eAbility.claw, false, false);
        abilities[(int)eAbility.wings] = new Ability(eAbility.wings, false, false);
        abilities[(int)eAbility.horn] = new Ability(eAbility.horn, false, false);
        abilities[(int)eAbility.water] = new Ability(eAbility.water, false, false);
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
                player.HasWings = true;
                break;
            case eAbility.horn:
                player.HasHorns = true;
                break;
            case eAbility.water:
                player.HasFins = true;

                break;
            case eAbility.claw:
                player.HasClaws = true;

                break;
            default:
                break;
        }

        StartCoroutine(SetSprite());
    }

    //HACK temp shit for gate 1
    private IEnumerator SetSprite()
    {
        switch (activeAbilities)
        {
            case 0:
                playerSprite.sprite = GumbaPlayer;
                break;
            case 1:
                playerSprite.sprite = WingPlayer;
                break;
            case 2:
                playerSprite.sprite = WingHornPlayer;
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(0.25f);

        while (BlackScreen.alpha > 0.0f)
        {
            BlackScreen.alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        GameManager.Instance.ChangeGameState(eGameState.running);
    }

    public void SetPassiveAbility(Color newCol)
    {
        playerSprite.color = newCol;
    }


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

