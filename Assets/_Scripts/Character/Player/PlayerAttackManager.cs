using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class PlayerAttackManager : MonoBehaviour
    {
        PlayerManager player;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (weapon.itemID == WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                Debug.Log("Unarmed, no attack anim");
                return;
            }
            player.playerAnimatorManager.PlayerTargetActionAnimation(weapon.OH_Light_Attack_1, true, true, false);
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (weapon.itemID == WorldItemDatabase.Instance.unarmedWeapon.itemID)
            {
                Debug.Log("Unarmed, no attack anim");
                return;
            }
            player.playerAnimatorManager.PlayerTargetActionAnimation(weapon.OH_Heavy_Attack_1, true, true, false);
        }
    }
}