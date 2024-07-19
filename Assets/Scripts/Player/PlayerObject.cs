using GameObjects;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerObject : MonoBehaviour
    {
        // test
        public bool test;

        private Collider2D col;
        private Rigidbody rb;
        private SpriteRenderer sprite;
        private Transform transformParent;
        private PlayerInput input;
        private BoxCollider attackBox;
        private GameObject hand;
        private Animator animator;
        private InteractHinter hinter;

        //public Camera cam;
        //public GameObject platform;
        public float itemSearchRange = 2f;
        public float itemSearchCooldown = 0.2f;
        [SerializeField]
        public float attackCooldown = 0.5f;
        public float knockbackModifier = 25f;
        [SerializeField]
        private float attackRange = 3f;

        private Player playerData;
        private ItemObject pickableItem;
        private ItemObject pickedItem;
        private IAppliableObject appliableObject;
        private Transform appliableTransform;
        private int pickableMask;
        private int interactableMask;
        private float itemSearchTimer;
        private float interactableTimer;
        private int canMove;
        private Outline glowingObject;
        private float attackTimer;
        private Bounds bedBounds;

        public Vector3 moveAmount;
        public Vector3 moveDirection;
        public bool inGameScene = false;
        public bool isMount, isAttacking = false;

        private AudioSource audio;

        [HideInInspector] public bool HasItem { get => pickedItem != null; }

        void Start()
        {
            //col = GetComponent<Collider2D>();
            rb = GetComponent<Rigidbody>();
            // Player components
            sprite = GetComponent<SpriteRenderer>();
            input = GetComponent<PlayerInput>();
            transformParent = GetComponentInParent<Transform>();
            hand = transformParent.Find("Hand").gameObject;
            playerData = new Player(1.5f, 2, attackRange);
            animator = GetComponent<Animator>();
            hinter = GetComponent<InteractHinter>();
            audio = GetComponent<AudioSource>();

            // Timer
            itemSearchTimer = 0f;
            attackTimer = attackCooldown;

            // AttackBox components
            moveAmount = Vector3.zero;
            moveDirection = Vector3.down;
            attackBox = transformParent.Find("AttackBox").GetComponent<BoxCollider>();
            attackBox.size = Vector3.one * playerData.AttackRange;
            ChangeAttackBoxPosition();

            // Global
            pickableMask = LayerMask.GetMask("Pickable");
            interactableMask = LayerMask.GetMask("Interactable");


            //cam.transform.position = new Vector3(cam.transform.position.x, platform.transform.position.y, cam.transform.position.z);
            Physics.gravity = new Vector3(0f, 0f, 1f);

            if (test)
                UnlockMove();
        }

        // Update is called once per frame
        void Update()
        {
            // Update timer
            itemSearchTimer += Time.deltaTime;
            interactableTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;

            // Do timed operation
            if (moveAmount != Vector3.zero)
            {
                rb.velocity = Vector3.zero;
                bool itemIsChanged = false;
                if (itemSearchTimer >= itemSearchCooldown)
                {
                    ChangePickableItem();
                    itemIsChanged = true;
                }
                if (interactableTimer >= itemSearchCooldown)
                {
                    ChangeInteractableItem();
                    itemIsChanged = true;
                }


                if (itemIsChanged)
                {
                    changeGlowingItem();
                }
            }
            else
            {
                SetAnimatorBool("walking", false);
            }

            if(pickedItem == null)
            {
                SetAnimatorBool("holding", false);
            }
            else
            {
                SetAnimatorBool("holding", true);
            }

            // Move
            if (transform == null) return;
            transform.Translate(moveAmount * Time.deltaTime, Space.World);
            transform.position = ClampVector3(transform.position, BedObject.Instance.transform.Find("Bounds (1)").GetComponent<SpriteRenderer>().bounds);
            //rb.velocity = moveAmount;
        }

        private Vector3 ClampVector3(Vector3 vector3, Bounds bounds)
        {
            float newx = Mathf.Clamp(vector3.x, bounds.min.x, bounds.max.x);
            float newy = Mathf.Clamp(vector3.y, bounds.min.y, bounds.max.y);
            return new Vector3(newx, newy, transform.position.z);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (canMove == 0) return;

            moveAmount = context.ReadValue<Vector2>() * playerData.Speed;
            if (context.ReadValue<Vector2>() != Vector2.zero)
            {
                moveDirection = context.ReadValue<Vector2>();
                SetAnimatorBool("walking", true);
                if (moveDirection.x > 0) { SetAnimatorBool("right", true); }
                if (moveDirection.x < 0) { SetAnimatorBool("right", false); }
            }
            ChangeAttackBoxPosition();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            // Attack logic here
            // melee -> call attack to all enemy within attack range (?)

            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }

            if (attackBox == null)
            {
                return;
            }

            if (attackTimer < attackCooldown)
            {
                return;
            }

            if (pickedItem != null)
            {
                return;
            }

            if (isAttacking || isMount)
            {
                return;
            }

            SetAnimatorTrigger("attack");

            Collider[] colliders = Physics.OverlapBox(attackBox.bounds.center, attackBox.bounds.extents, Quaternion.identity, ((1 << 30) - 1) ^ (1 << 7));
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                //GameObject objInCollider = col.transform.gameObject;
                if (rb != null)
                {
                    audio.Play();
                    EnemyObject enemyObject = col.GetComponent<EnemyObject>();
                    if (enemyObject != null)
                    {
                        Vector2 force = (enemyObject.transform.position - transform.position) * knockbackModifier;
                        rb.AddForce(force);
                        attackTimer = 0;
                        playerData.Attack(enemyObject);
                        Debug.LogWarning("Attack " + enemyObject.name);
                    }
                    else{
                        if (col.CompareTag("Enemy")){
                            Debug.LogWarning("Attack " + col.name);
                            if (transform.root.TryGetComponent(out EnemyObject targetedEnemyObject)) {
                                playerData.Attack(targetedEnemyObject);
                            }else{
                                Debug.LogError("Can't find Enemy Object");
                            }
                        }
                    }
                }
            }
        }

        public void Pickup(ItemObject item)
        {
            if (pickedItem == null)
            {
                pickedItem = item;
            }
            pickedItem.transform.parent = transform;
            pickedItem.transform.position = hand.transform.position;
            pickableItem = null;
        }

        public void OnPickup(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Started)
            {
                return;
            }

            // Pickup logic here
            // Pick pickableItem if not null
            if (pickableItem == null)
            {
                return;
            }
            if (pickedItem != null)
            {
                return;
            }

            if (pickableItem.pick())
            {

                pickedItem = pickableItem;
                pickedItem.transform.parent = transform;
                pickedItem.transform.position = hand.transform.position;
                pickableItem = null;
            }
        }
        private void ChangePickableItem()
        {
            itemSearchTimer = 0f;

            // Change pickable item
            if (playerData.IsHoldingItem())
            {
                return;
            }

            Collider[] res = Physics.OverlapSphere(transform.position + transform.forward * itemSearchRange / 2, itemSearchRange, pickableMask);
            res = res.Where(c => c.GetComponent<ItemObject>().isPickable()).ToArray();

            if (res.Length > 0)
            {
                // get nearest pickable
                Collider nearest = res[0];
                float nearestDistanceSqr = ((Vector2)nearest.transform.position - (Vector2)transform.position).sqrMagnitude;
                for (int i = 1; i < res.Length; ++i)
                {
                    float curDistSqr = (res[i].transform.position - transform.position).sqrMagnitude;
                    if (curDistSqr < nearestDistanceSqr)
                    {
                        nearestDistanceSqr = curDistSqr;
                        nearest = res[i];
                    }
                }
                pickableItem = nearest.GetComponent<ItemObject>();
            }
            else
            {
                pickableItem = null;
            }

            Debug.Log("Pickable item: " + pickableItem);
        }

        public void changeGlowingItem()
        {
            GameObject newGlowingObject = null;
            if (appliableObject == null && pickableItem == null)
            {
                newGlowingObject = null;
            }
            else if (appliableObject == null)
            {
                newGlowingObject = pickableItem.gameObject;
            }
            else if (pickableItem == null)
            {
                newGlowingObject = appliableTransform.gameObject;
            }
            else if ((appliableTransform.position - transform.position).sqrMagnitude < (pickableItem.transform.position - transform.position).sqrMagnitude)
            {
                newGlowingObject = appliableTransform.gameObject;
            }
            else
            {
                newGlowingObject = pickableItem.gameObject;
            }

            hinter.SetInteract(newGlowingObject);

            Debug.Log(glowingObject + " | " + newGlowingObject);
            //if (glowingObject != newGlowingObject)
            //{
            //    if (glowingObject != null)
            //    {
            //        glowingObject.enabled = false;
            //    }
            //    if (newGlowingObject != null)
            //    {
            //        newGlowingObject.enabled = true;
            //    }
            //    glowingObject = newGlowingObject;
            //}
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Started)
            {
                return;
            }

            // Interact logic here
            if (appliableObject == null)
            {
                return;
            }
            if (pickedItem)
            {
                pickedItem.Use(appliableObject, this);
            }
            else
            {
                appliableObject.Apply(null, this);
            }

        }
        public void OnPickupOrInteract(InputAction.CallbackContext context)
        {
            if (appliableObject == null)
            {
                // Try Pickup
                OnPickup(context);
                return;
            }
            if (pickableItem == null)
            {
                // Try Interact
                OnInteract(context);
                return;
            }
            if ((appliableTransform.position - transform.position).sqrMagnitude < (pickableItem.transform.position - transform.position).sqrMagnitude)
            {
                // Try Interact
                OnInteract(context);
                return;
            }
            else
            {
                // Try Pickup
                OnPickup(context);
                return;
            }
        }

        private void ChangeInteractableItem()
        {
            interactableTimer = 0f;


            Collider[] res = Physics.OverlapSphere(transform.position + transform.forward * itemSearchRange / 2, itemSearchRange, interactableMask);
            res = res.Where(x => x.GetComponent<IAppliableObject>() != null && x.GetComponent<IAppliableObject>().CanApply(pickedItem)).ToArray();
            if (res.Length > 0)
            {
                // get nearest pickable
                Collider nearest = res[0];
                float nearestDistanceSqr = ((Vector2)nearest.transform.position - (Vector2)transform.position).sqrMagnitude;
                for (int i = 1; i < res.Length; ++i)
                {
                    float curDistSqr = ((Vector2)res[i].transform.position - (Vector2)transform.position).sqrMagnitude;
                    if (curDistSqr < nearestDistanceSqr)
                    {
                        nearestDistanceSqr = curDistSqr;
                        nearest = res[i];
                    }
                }
                appliableObject = nearest.GetComponent<IAppliableObject>();
                appliableTransform = nearest.transform;
            }
            else
            {
                appliableObject = null;
                appliableTransform = null;
            }
            Debug.Log("Appliable object: " + appliableObject);
        }

        public void OnDrop(InputAction.CallbackContext context)
        {
            if (!context.started) { return; }
            if (pickedItem != null)
            {
                pickedItem.drop();
                pickedItem.transform.position = transform.position;
                pickedItem.transform.parent = null;
                pickedItem = null;
            }
        }

        private void ChangeAttackBoxPosition()
        {
            attackBox.center = playerData.AttackRange *
                new Vector3(
                    moveDirection.x == 0 ? 0 : Mathf.Sign(moveDirection.x),
                    moveDirection.y == 0 ? 0 : 0 /*Mathf.Sign(moveDirection.y)*/,
                    moveDirection.z == 0 ? 0 : Mathf.Sign(moveDirection.z)
                ) / 2;
        }

        public void UnlockMove()
        {
            canMove = 1;
        }
        public void LockMove()
        {
            moveAmount = Vector3.zero;
            moveDirection = Vector3.zero;
            canMove = 0;
        }

        /// <summary>
        /// Hit and stun player
        /// </summary>
        public void TakeHit()
        {
            SetAnimatorTrigger("Hit");
            StartCoroutine(Stun(0.8f));
        }

        private IEnumerator Stun(float time)
        {
            LockMove();
            yield return new WaitForSeconds(time);
            UnlockMove();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
            }
        }

        private void SetAnimatorBool(string name, bool state)
        {
            if (!inGameScene) { return; }
            animator.SetBool(name, state);
        }

        private void SetAnimatorTrigger(string name)
        {
            if (!inGameScene) { return; }
            animator.SetTrigger(name);
        }
    }

}
