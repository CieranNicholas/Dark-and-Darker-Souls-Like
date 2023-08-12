using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
            CalculateHealthBasedOnVitalitylevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEndurancelevel(player.playerNetworkManager.vitality.Value);
        }
    }

}