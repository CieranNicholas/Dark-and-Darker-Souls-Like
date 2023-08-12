using LS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug Delete Later")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] bool prcoessEffect = false;

        private void Update()
        {
            if(prcoessEffect) 
            {
                prcoessEffect = false;
                // WE INSTANTIATE A COPY SO THAT IF WE CHANGE THE VALUES OF THE EFFECT, THE ORIGINAL SCRIPTABLE OBJECT WILL RETAIN ITS VALUE FOR NEXT TIME
                TakeStaminaDamageEffect effect = Instantiate(effectToTest) as TakeStaminaDamageEffect;
                effect.staminaDamage = 50;
                ProcessInstantEffect(effect);
            }
        }
    }
}
