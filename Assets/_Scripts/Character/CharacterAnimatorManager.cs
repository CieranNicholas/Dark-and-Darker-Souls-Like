using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace LS
{
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }
        
        protected virtual void OnAnimatorMove()
        {

        }
        
        public void UpdateAnimtorMovementParameters(float horizontalValue, float verticalValue, bool isSprinting)
        {
            float horizontalAmount = horizontalValue;
            float verticalAmount = verticalValue;

            if (isSprinting)
            {
                verticalAmount = 2f;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime); ;
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
        }

        public virtual void PlayerTargetActionAnimation(string targetAnimation, bool 
            isPerformingAction, 
            bool applyRootMotion, 
            bool canRotate = false, 
            bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            // TELL THE SERER /HOST WE PLAYED AN ANIMATION kinda like triggerServerEvent in five<
            // THE SERVER WILL THEN TELL ALL OTHER CLIENTS TO DO THE ANIM
            character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(
                NetworkManager.Singleton.LocalClientId, 
                targetAnimation, 
                applyRootMotion);
        }

        public void EnableCombo()
        {
            character.animator.SetBool("canDoCombo", true);
            Debug.Log("Combo Enabled");
        }

        public void DisableCombo()
        {
            character.animator.SetBool("canDoCombo", false);
            Debug.Log("Combo Disabled");
        }
    }

}