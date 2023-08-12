using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        // PROCESS INSTANT EFFECTS (DAMAGE, HEALING ETC)

        // DOTS, OVERT TIME EFFECTS (BLEEDING, POISON)

        // STATIC EFFECTS (BUFFS FROM GEAR ETC)

        CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(character);
        }
    }
}
