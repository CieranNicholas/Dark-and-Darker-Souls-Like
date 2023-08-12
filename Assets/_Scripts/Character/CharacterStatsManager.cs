using LS;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;

    [Header("Stamina Regen")]
    [SerializeField] int staminaRegenAmount = 2;
    [SerializeField] float staminaRegenDelay = 2f;
    private float staminaRegenTimer = 0;
    private float staminaTickTimer = 0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    protected virtual void Start()
    {

    }

    public virtual void RegenStamina()
    {
        if (!character.IsOwner) return;
        if (character.characterNetworkManager.networkIsSpriting.Value) return;
        if (character.isPerformingAction) return;

        staminaRegenTimer += Time.deltaTime;

        if (staminaRegenTimer >= staminaRegenDelay)
        {
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;

                if (staminaTickTimer >= 0.1f)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenAmount;
                }
            }
        }
    }
    
    public virtual void ResetStaminaRegenTimer(float previousStamina, float currentStamina)
    {
        if(currentStamina < previousStamina)
        {
            staminaRegenTimer = 0;
        }
        
    }

    public int CalculateStaminaBasedOnEndurancelevel(int endurance)
    {
        float stamina = 0;

        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }

    public int CalculateHealthBasedOnVitalitylevel(int vitality)
    {
        int health = 0;

        health = vitality * 15;

        return Mathf.RoundToInt(health);
    }
}
