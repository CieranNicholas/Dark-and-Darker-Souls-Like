using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class PlayerMovementManager : CharacterMovementManager
    {
        PlayerManager player;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2f;
        [SerializeField] float runningSpeed = 5f;
        [SerializeField] float sprintingSpeed = 7f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Jump")]
        [SerializeField] float jumpHeight = 4;
        [SerializeField] float jumpStaminaCost = 25;
        [SerializeField] Vector3 jumpDirection;
        [SerializeField] float jumpForwardSpeed = 5f;
        [SerializeField] float freeFallingSpeed = 2f;

        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 25;

        

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if(player.IsOwner)
            {
                player.characterNetworkManager.networkAnimatorHorizontalParameter.Value = horizontalMovement;
                player.characterNetworkManager.networkAnimatorVerticalParameter.Value = verticalMovement;
                player.characterNetworkManager.networkAnimatorMoveAmountParameter.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.networkAnimatorVerticalParameter.Value;
                horizontalMovement = player.characterNetworkManager.networkAnimatorHorizontalParameter.Value;
                moveAmount = player.characterNetworkManager.networkAnimatorMoveAmountParameter.Value;

                player.playerAnimatorManager.UpdateAnimtorMovementParameters(0, moveAmount, player.playerNetworkManager.networkIsSpriting.Value);
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
            HandleJumpingForwardMovement();
            HandleFreeFallMovement();
        }

        private void GetMovementInputs()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
            // CLAMP THE MOVEMENTS
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove) return;
            GetMovementInputs();
            // MOVEMENT DIRECTION IS BASED OFF CAMERA FACING PERSPECTIVE / MOVEMENT INPUTS
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.y = 0;
            moveDirection.Normalize();

            if(player.playerNetworkManager.networkIsSpriting.Value)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    // MOVE AT A RUNNING SPEED
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    // MOVE AT WALKING SPEED
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }
        }
   
        private void HandleJumpingForwardMovement()
        {
            if(player.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if(!player.isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallingSpeed * Time.deltaTime);
            }
        }
       
        private void HandleRotation()
        {
            if (!player.canRotate) return;

            Vector3 targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.y = 0;
            targetRotationDirection.Normalize();

            if (targetRotationDirection == Vector3.zero)
                targetRotationDirection = transform.forward;

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
    
        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction) return;

            if (player.playerNetworkManager.currentStamina.Value <= 0) return;

            if (PlayerInputManager.instance.moveAmount > 0)
                //ROLL IF MOVING
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                player.playerAnimatorManager.PlayerTargetActionAnimation("Roll_Forward_01", true, true);
            }
            // BACKSTEP / DODGE IF NOT MOVING
            else
            {
                player.playerAnimatorManager.PlayerTargetActionAnimation("Back_Step_01", true, true);
            }

            player.playerNetworkManager.currentStamina.Value -= dodgeStaminaCost;
        }
   
        public void HandleSprinting()
        {
            if(player.isPerformingAction)
            {
                player.playerNetworkManager.networkIsSpriting.Value = false;
            }

            if (player.playerNetworkManager.currentStamina.Value <= 0)
            {
                player.playerNetworkManager.networkIsSpriting.Value = false;
                return;
            }

            if(moveAmount >= 0.5f)
            {
                player.playerNetworkManager.networkIsSpriting.Value = true;
            }
            else
            {
                player.playerNetworkManager.networkIsSpriting.Value = false;
            }

            if(player.playerNetworkManager.networkIsSpriting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction) return;

            if (player.playerNetworkManager.currentStamina.Value <= 0) return;

            if (player.isJumping) return;

            if (!player.isGrounded) return;

            // if we are two handing out weapon play 2h anim, else play 1h anim
            player.playerAnimatorManager.PlayerTargetActionAnimation("Main_Jump_01", false, true);

            player.isJumping = true;

            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;

            if(jumpDirection != Vector3.zero)
            {
                if (player.playerNetworkManager.networkIsSpriting.Value)
                {
                    jumpDirection *= 1;
                }
                else if (PlayerInputManager.instance.moveAmount > 0.5)
                {
                    jumpDirection *= 0.5f;
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }
        }

        public void ApplyJumpingVelocity() 
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}
