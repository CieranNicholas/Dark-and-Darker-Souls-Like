using LS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // USE FOR APPLYING DMG MULTIPLERS
    }
}
