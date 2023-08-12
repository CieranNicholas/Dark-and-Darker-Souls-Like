using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class CharacterMovementManager : MonoBehaviour
    {
        CharacterManager character;



        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity; // force at which our character is pulled up or down
        [SerializeField] protected float groundedYVelocity = -20; // Force which our character is stickingh to the ground while grounded
        [SerializeField] protected float fallStartYVelocity = -5;
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>(); 
        }

        protected virtual void Update()
        {
            HandleGroundCheck();
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
            if(character.isGrounded )
            {
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if(!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }
            // ALWAYS BE SOME FORCE PULLING CHARACTER TOWARDS THE GROUND
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void OnDrawGizmosSelected()
        {
            if (character == null) return;
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}
