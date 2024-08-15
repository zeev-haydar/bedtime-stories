using Enemies;
using Managers;
using Player;
using UnityEngine;

namespace GameObjects
{
    public class MeleeEnemyObject : EnemyObject
    {
        [SerializeField] private float speed;
        [SerializeField] private float maxMoveRange;
        [SerializeField] private float attackInterval;
        [SerializeField] private GameObject indicator;

        public MeleeEnemy meleeEnemy;
        private bool dyingAnim;
        private float intervalTimer;
        private Vector2 targetPosition;
        private Bounds groundBounds;

        void Awake()
        {
            indicator.SetActive(false);
            meleeEnemy = new MeleeEnemy(6, speed);
            intervalTimer = attackInterval;
            Animator = GetComponent<Animator>();
            Sprite = GetComponent<SpriteRenderer>();
            Rb = GetComponent<Rigidbody>();
            groundBounds = BedObject.transform.Find("Bounds").GetComponent<SpriteRenderer>().bounds;
            SetTargetPosition();
        }

        private void Update()
        {
            //if (dyingAnim)
            //{
            //    if (!isDead) { return; }
            //    EnemyDeath();
            //}

            if (isAttacking || isHit) { return; }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, meleeEnemy.Speed * Time.deltaTime);

            if (transform.position.Equals(targetPosition))
            {
                SetTargetPosition();
            }

            if(intervalTimer < 1f)
            {
                indicator.SetActive(true);
                indicator.GetComponent<Animator>().Play("enemy_attack_ind");
            }

            if (intervalTimer > 0f) { intervalTimer -= Time.deltaTime; return; }
            Animator.Play("MeleeEnemyAttack");
            intervalTimer = attackInterval;
            indicator.SetActive(false);
        }
        private void SetTargetPosition()
        {
            Vector2 vector = new Vector2(transform.position.x + Random.Range(-maxMoveRange, maxMoveRange), transform.position.y + Random.Range(-maxMoveRange * 0.8f, maxMoveRange * 0.8f));
            targetPosition = new Vector3(Mathf.Clamp(vector.x, groundBounds.min.x + Sprite.bounds.extents.x, groundBounds.max.x - Sprite.bounds.extents.x), Mathf.Clamp(vector.y, groundBounds.min.y + Sprite.bounds.extents.y, groundBounds.max.y), transform.position.z);
        }

        public void HitBed()
        {
            BedObject.GetHit(transform.position);
        }

        override public void TakeHit(int damage)
        {
            Animator.Play("MeleeEnemyHit");
            intervalTimer = attackInterval;
            meleeEnemy.ReduceHealth(damage);
            if (meleeEnemy.Health == 0)
            {
                EnemyDeath();
            }
        }

        public void MeleeEnemySpawn()
        {
            transform.position = RandomTargetPoint(groundBounds);
            Animator.Play("MeleeEnemySpawn");
        }

        override public void EnemyDeath()
        {
            Animator.Play("MeleeEnemyDeath");
            EnemySpawner.Instance.RemoveFallenEnemy(gameObject);
        }

        public void ResetVelocity()
        {
            Rb.velocity = Vector3.zero;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy"))
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
            }
        }
    }
}
