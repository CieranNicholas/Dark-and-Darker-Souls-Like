using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LS
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;

        public PlayerManager player;
        PlayerControls playerControls;

        [Header("Camera Movement Input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("Player Movement Input")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool lightAttackInput = false;
        [SerializeField] bool heavyAttackInput = false;

        [Header("Flags")]
        public bool comboFlag;

        public float moveAmount;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            } else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChange;
            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            // LOADING INTO OUR WORLD SCENE AND ENABLING PLAYER INPUT
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;
            }
            // OTHERWISE WE'RE AT A MENU SCNE, WE DISABLE PLAYER INPUT
            else
            {
                instance.enabled = false;
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.LightAttack.performed += i => lightAttackInput = true;
                playerControls.PlayerActions.HeavyAttack.performed += i => heavyAttackInput = true;

                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }
            playerControls.Enable();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleAttackInput();
        }

        #region MOVEMENT
        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            } else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1f;
            }
            // WHY DO WE PASS 0 ON THE HORIZONTAL? BECAUSE WE ONLY WANT NON STRAGING
            // WE USE HORIONTAL WHEN WE ARE STRAFING / LOCKED ON
            if (player == null)
                return;

            player.playerAnimatorManager.UpdateAnimtorMovementParameters(0, moveAmount, player.playerNetworkManager.networkIsSpriting.Value);
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
        #endregion

        #region ACTIONS
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                player.playerMovementManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                player.playerMovementManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.networkIsSpriting.Value = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                player.playerMovementManager.AttemptToPerformJump();
            }
        }

        private void HandleAttackInput()
        {
            if(lightAttackInput)
            {
                lightAttackInput = false;
                if(player.canDoCombo)
                {

                    comboFlag = true;
                    player.playerAttackManager.HandleWeaponCombo(player.playerInventoryManager.currentRightHandWeapon);
                    comboFlag = false;
                }
                else
                {
                    //if (player.isPerformingAction)
                        //return;
                    //if (player.canDoCombo)
                        //return;
                    player.playerAttackManager.HandleLightAttack(player.playerInventoryManager.currentRightHandWeapon);
                }
            }

            if(heavyAttackInput)
            {
                heavyAttackInput = false;

                if (player.isPerformingAction)
                    return;

                player.playerAttackManager.HandleHeavyAttack(player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        #endregion
    }

}