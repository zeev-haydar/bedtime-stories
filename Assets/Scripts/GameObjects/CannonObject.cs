using System;
using System.Collections;
using System.Collections.Generic;
using BedMechanics;
using Exceptions;
using Items;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameObjects
{
    enum CannonState
    {
        Unused,
        Mounted
    }

    public class CannonObject : MonoBehaviour, IAppliableObject
    {
        public Cannon cannon;
        public GameObject projectilePrefab;

        public const float ROTATION_SPEED = 100f;
        private float currentSpeed = 0f;
        private Transform vfxObjectTransform, triangleTransform;
        private CannonState state = CannonState.Unused;
        private PlayerObject user;

        [SerializeField] private float initialDegree;
        [SerializeField] private bool inverted;
        private float currentRotation;

        //private bool mounted;
        private void OnValidate()
        {
            transform.Find("VFX").rotation = Quaternion.Euler(0, 0, initialDegree);
        }
        private void Awake()
        {
            vfxObjectTransform = transform.Find("VFX");
            triangleTransform = vfxObjectTransform.Find("Triangle");
            currentRotation = initialDegree;
            cannon = new Cannon();
        }

        // Update is called once per frame
        void Update()
        {
            if (state == CannonState.Unused) return;
            ChangeDirection(Time.deltaTime * ROTATION_SPEED * currentSpeed);
        }

        public void Apply(ItemObject itemObject, PlayerObject player)
        {
            if (itemObject == null)
            {
                InputActionMap actionMap = player.GetComponent<PlayerInput>().currentActionMap;
                if (state == CannonState.Unused)
                {
                    // mount player
                    state = CannonState.Mounted;
                    player.isMount = true;
                    player.transform.parent = transform;
                    player.transform.localPosition = Vector3.zero;
                    player.LockMove();
                    user = player;


                    // subscribe cannon movement to player/move
                    actionMap.FindAction("Move").performed += OnMove;
                    actionMap.FindAction("Move").canceled += OnMove;
                    actionMap.FindAction("Shoot").started += Shoot;
                }
                else if (state == CannonState.Mounted && player == user)
                {
                    state = CannonState.Unused;
                    player.UnlockMove();
                    player.transform.SetParent(null);
                    player.isMount = false;
                    user = null;

                    // unsubscribe from player/move
                    actionMap.FindAction("Move").performed -= OnMove;
                    actionMap.FindAction("Move").canceled -= OnMove;
                    actionMap.FindAction("Shoot").started -= Shoot;
                    currentSpeed = 0f;
                }

            }
            else
            {
                try
                {
                    cannon.Apply(itemObject.item);
                    Destroy(itemObject.gameObject);
                }
                catch (CannotApplyException e)
                {
                    Debug.Log(e.Message);

                }

            }

        }

        public bool CanApply(ItemObject item)
        {
            return item == null || item is AmmoObject;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                currentSpeed = 0f;
            }
            if (context.performed)
            {
                currentSpeed = -context.ReadValue<Vector2>().x * (inverted ? -1 : 1);
            }
        }

    



        /// <summary>
        /// Shoot the cannon based on the current trajectory
        /// </summary>
        public void Shoot(InputAction.CallbackContext context)
        {
            if (cannon.Ammo <= 0)
            {
                // play the "nembak angin" sound 
                return;
            }
            // summon projectile prefab here
            GameObject projectile = Instantiate(projectilePrefab, triangleTransform.position, Quaternion.identity);
            projectile.GetComponent<ProjectileObject>().Launch(currentRotation);
            // shot the projectile based on current trajectory of cannon (based on vfXObject)
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate the velocity based on the direction the cannon is facing
                Vector2 velocity = vfxObjectTransform.up * cannon.ProjectileSpeed;

                // Apply the velocity to the projectile
                rb.velocity = velocity;
                cannon.Ammo--; 
            }
            else
            {
                Debug.LogError("Projectile prefab does not have a Rigidbody2D component.");
            }
        }

        public void ChangeDirection(float degree)
        {
            float newRotation = currentRotation + degree;
            SetCurrentRotation(newRotation);
        }

        private void SetCurrentRotation(float rot)
        {
            currentRotation = Mathf.Clamp(rot, -90+initialDegree, 90+initialDegree);
            vfxObjectTransform.rotation = Quaternion.Euler(0, 0, rot);
        }
    }

}

