using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class InstantCharacterEffect : ScriptableObject
    {
        [Header("Effect ID")]
        public int isntantEffectID;

        public virtual void ProcessEffect(CharacterManager character)
        {

        }
    }
}
