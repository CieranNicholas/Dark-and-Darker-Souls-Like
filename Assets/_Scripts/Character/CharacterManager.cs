using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;
namespace LS
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("Status")]
        public NetworkVariable<bool> isDead = new(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [HideInInspector] public CharacterNetworkManager characterNetworkManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool isGrounded = true;
        public bool isJumping = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
            // IF THIS CHRACTER IS BEING CONTROLLED FROM OUR SIDE, ASSIGN ITS NET POSITION TO OUR TRANSFORM
            if (IsOwner)
            {
                characterNetworkManager.networkPoisition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;

            }
            else // IF THIS CAHRACTERS IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALY BY THE POS OF ITS NET TRANSFORM
            {

                // POSITION
                //transform.position = Vector3.SmoothDamp
                //    (transform.position, 
                //    characterNetworkManager.networkPoisition.Value,
                //    ref characterNetworkManager.networkPositionVelocity, 
                //    characterNetworkManager.networkPositionSmoothTime);
                transform.position = Vector3.Lerp(transform.position, characterNetworkManager.networkPoisition.Value, characterNetworkManager.networkPositionSmoothTime);
                //ROTATION
                transform.rotation = Quaternion.Slerp
                    (transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }
    
        protected virtual void LateUpdate()
        {

        }
    
        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if(IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;

                // RESET ANY FLAGS HERE THAT NEED TO BE RESET

                // IF WE ARE NOT GROUNDED PLAY AN AERIAL DEATH ANIMATION
                // CHANGE TO RAGDOLL DOWN THE LINE BECAUSE ITS FUNNY

                if(!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayerTargetActionAnimation("Dead_01", true, true);
                }
            }

            // PLAY SOME DEATH SFX MAYBE

            yield return new WaitForSeconds(5);

            // AWARD PLAYER WITH RUNES / XP

        }
    
        public virtual void ReviveCharacter()
        {

        }
    }
}
