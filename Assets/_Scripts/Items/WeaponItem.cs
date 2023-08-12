using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class WeaponItem : Item
    {
        // ANIMATOR CONTROLLER OVERRIDE (Change attack anims based on weapon used)

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int lightningDamage = 0;
        public int holyDamage = 0;

        [Header("Weapon Base Poise Damage")]
        public float poiseDamage = 10;


        // WEAPON MODIFIERS
        // LIGHT MODIFIER
        // HEAVY MODIFIER
        // CRIT MODIFIER

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        // STAMINA MODIFIERS, E.G LIGHT, HEAVY SPRINTING
    
        // ITEM BASED ACTIONS (RB,RT LB, LT)
        //ASHES OF WAR
        // BLOCKING SOUNDS

    }
}
