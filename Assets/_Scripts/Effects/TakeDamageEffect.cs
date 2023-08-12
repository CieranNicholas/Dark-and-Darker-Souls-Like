using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; // stores the character the damage the character comes from

        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Final Damage")]
        private int finalDamageDealt = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("Animation")]
        public bool playDamageAnimatin = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")]
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX;

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;
        public Vector3 contactPoint;

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);

            if (character.isDead.Value)
                return;

            // CHECK FOR "INVULNERABILITY' I.E. I FRAMES

            CalculateDamage(character);
            //CHECK FOR DIRECTION DAMAGE CAME FROM
            //PLAY DAMAGE ANIM
            //CHECK FOR BUILD UPS (POISON, BLEED ETC)
            //PLAY DAMAGE SFX
            // PLAY DAMAGE VFX (BLOOD)

            // IF CHAR IS AI, CHECK FOR THE NEW TARGET IF CHARACTER CAUSING DAMAGE IS PRESENT

            
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;

            if (characterCausingDamage != null) 
            { 
                // CHECK FOR DAMAGE MODIFIERS AND MODIFY BASE DAMAGE
            }

            // CHECK CHARACTER FOR FLAT DEFENCES, DAMAGE REDUCTION ETC AND REDUCE FROM DAMAGE
            // CHECK FOR ARMOR ABSORPTIONS
            // ADD ALL OF THE DAMAGE TYPES TOGETHER
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

            if(finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }

            Debug.Log("Final Damage Dealt: " +  finalDamageDealt);

            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;

        }
    }
}
