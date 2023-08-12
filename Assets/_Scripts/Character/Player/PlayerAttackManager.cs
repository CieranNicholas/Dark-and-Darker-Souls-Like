using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class PlayerAttackManager : MonoBehaviour
    {
        PlayerManager player;
        public string lastAttack;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
  

            if (PlayerInputManager.instance.comboFlag)
            {
                player.animator.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    player.playerAnimatorManager.PlayerTargetActionAnimation(weapon.OH_Light_Attack_2, true, true);
                }
            }

        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (weapon.itemID == WorldItemDatabase.Instance.unarmedWeapon.itemID)
                return;
            player.playerAnimatorManager.PlayerTargetActionAnimation(weapon.OH_Light_Attack_1, true, true);
            lastAttack = weapon.OH_Light_Attack_1;
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (weapon.itemID == WorldItemDatabase.Instance.unarmedWeapon.itemID)
                return;
            player.playerAnimatorManager.PlayerTargetActionAnimation(weapon.OH_Heavy_Attack_1, true, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }
    }
}