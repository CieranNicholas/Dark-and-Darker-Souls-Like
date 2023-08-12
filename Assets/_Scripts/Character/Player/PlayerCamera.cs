using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera instance;
        public Camera cameraObject;
        public PlayerManager player;
        [SerializeField] Transform cameraPivotTransform;

        // CHANGE THESE TO TWEAK CAMERA PERFORMANCE
        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1f; // bigger the number the longer the latency between you and the camera pos is.
        [SerializeField] private float upAndDownRotationSpeed = 220f;
        [SerializeField] private float leftAndRightRotationSpeed = 220f;
        [SerializeField] private float minimumPivot = -30f; // Lowest point you can look down
        [SerializeField] private float maximumPivot = 60f; // highest point you can look up
        [SerializeField] private float cameraCollisionRadius = 0.2f;
        [SerializeField] LayerMask collideWithLayers;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition;
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition;
        private float targetCameraZPosition;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            cameraZPosition = cameraObject.transform.localPosition.z;
        }
    
        public void HandleAllCameraActions()
        {
            if(player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPos = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPos;
        }

        private void HandleRotations()
        {
            // IF LOCKED ON, FORCE ROTATION TOWARDS TARGET
            //ELSE ROTATE NORMALY

            // ROTATE LEFT & RIGHT BASED ON HORIONZTAL MOVEMENT ON MOUSE / RIGHT JOYSTICK
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
            // ROTATE UP & DOWN BASED ON HORIONZTAL MOVEMENT ON MOUSE / RIGHT JOYSTICK
            upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;
            // CLAMP THE UP AND DOWN BETWEEN MIN AND MAX VALUE
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targeRotation;

            // ROTATE THIS GAME OBJECT LEFT AND RIGHT
            cameraRotation.y = leftAndRightLookAngle;
            targeRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targeRotation;

            // ROTATE THE PIVOT GAMEOBJECT UP AND DOWN
            cameraRotation = Vector3.zero;
            cameraRotation.x = upAndDownLookAngle;
            targeRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targeRotation;
        }
   
        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                float distanceFromHit = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromHit - cameraCollisionRadius);
            }
            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
            cameraObject.transform.localPosition = cameraObjectPosition;
        }
    }

}