using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace LS
{
    public class CharacterNetworkManager : NetworkBehaviour
    {
        CharacterManager character;

        [Header("Position")]
        public NetworkVariable<Vector3> networkPoisition = new(Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Quaternion> networkRotation = new(Quaternion.identity, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public Vector3 networkPositionVelocity;
        public float networkPositionSmoothTime = 0.1f;
        public float networkRotationSmoothTime = 0.1f;

        [Header("Animator")]
        public NetworkVariable<float> networkAnimatorHorizontalParameter = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkAnimatorVerticalParameter = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<float> networkAnimatorMoveAmountParameter = new(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        [Header("Flags")]
        public NetworkVariable<bool> networkIsSpriting = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Resources")]
        public NetworkVariable<float> currentStamina = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxStamina = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> currentHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> maxHealth = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [Header("Stats")]
        public NetworkVariable<int> endurance = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<int> vitality = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public void CheckHP(int oldValue, int newValue)
        {
            if(currentHealth.Value <= 0)
            {
                StartCoroutine(character.ProcessDeathEvent());
            }
            // PREVENTS OVERHEALING
            if(character.IsOwner)
            {
                if(currentHealth.Value > maxHealth.Value)
                {
                    currentHealth.Value = maxHealth.Value;
                }
            }
        }

        // This is like a fiveM net event, register on the server, trigger on the client
        [ServerRpc]
        public void NotifyTheServerOfActionAnimationServerRpc(ulong ClientID, string animationID, bool applyRootMotion)
        {
            if(IsServer)
            {
                // TriggerClientEvent(-1, arg, arg)
                PlayActionAniamtionForAllClientsClientRpc(ClientID, animationID, applyRootMotion);
            }
        }

        // RegisterNetEvent aka register a client event. Sent to all clients from the server
        [ClientRpc]
        public void PlayActionAniamtionForAllClientsClientRpc(ulong ClientID, string animationID, bool applyRootMotion)
        {
            // make sure we dont run the anim on the client who send it
            if(ClientID != NetworkManager.Singleton.LocalClientId) 
            {
                PerfomActionAnimationFromServer(animationID, applyRootMotion);
            }
        }

        private void PerfomActionAnimationFromServer(string animationID, bool applyRootMotion)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(animationID, 0.2f);
        }
    }
}
