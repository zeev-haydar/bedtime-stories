using Enemies;
using GameObjects;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameObjects
{
    public class RangedEnemyObject : EnemyObject
    {
        public enum RangedPosition
        {
            Top,
            Bottom
        }

        [Header("References")]
        [SerializeField] private GameObject projectilePrefab;
        [Header("Attributes")]
        [SerializeField] private float speed;
        [SerializeField] private float attackInterval;
        [SerializeField] private float projectileAirtime;

        public RangedEnemy rangedEnemy;
        private float intervalTimer;
        private Bounds bedBounds, upperBounds, lowerBounds, currBounds;
        private Vector2 targetPosition;

        public bool test;

        private void Awake()
        {
            rangedEnemy = new RangedEnemy(4, speed);
            intervalTimer = attackInterval;
            Animator = GetComponent<Animator>();
            Sprite = GetComponent<SpriteRenderer>();
            Rb = GetComponent<Rigidbody>();
            bedBounds = BedObject.transform.Find("Bounds").GetComponent<SpriteRenderer>().bounds;
            upperBounds = BedObject.transform.Find("Upper Bounds").GetComponent<SpriteRenderer>().bounds;
            lowerBounds = BedObject.transform.Find("Lower Bounds").GetComponent<SpriteRenderer>().bounds;
        }
        private void Update()
        {
            if (isAttacking || isHit) { return; }

            if (test)
            {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, rangedEnemy.Speed * Time.deltaTime);
            }

            if (transform.position.Equals(targetPosition))
            {
                targetPosition = RandomTargetPoint(currBounds);
            }

            if (intervalTimer > 0f) { intervalTimer -= Time.deltaTime; return; }
            Animator.Play("RangedEnemyAttack");
            intervalTimer = attackInterval;
        }

        public void ShootProjectile()
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.GetComponent<EnemyProjectileObject>().Launch(transform.position, RandomTargetPoint(bedBounds),projectileAirtime);
        }

        //private Vector2 RandomTargetPoint(Bounds bounds)
        //{
        //    return new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        //}

        public override void TakeHit(int damage)
        {
            
            Animator.Play("RangedEnemyHit");
            intervalTimer = attackInterval;
            rangedEnemy.ReduceHealth(damage);
            Debug.LogWarning("Kena hit.health sekarang = " + rangedEnemy.Health);
            if (rangedEnemy.Health <= 0)
            {
                
                EnemyDeath();
                Animator.Play("RangedEnemyDeath");
            }
        }

        public void RangedEnemySpawn(RangedPosition position)
        {
            if(position == RangedPosition.Top)
            {
                currBounds = upperBounds;
                Sprite.sortingLayerName = "Bed Back";
            }
            else
            {
                currBounds = lowerBounds;
                Sprite.sortingLayerName = "Bed Front";
            }
            transform.position = RandomTargetPoint(currBounds);
            targetPosition = RandomTargetPoint(currBounds);
            Animator.Play("RangedEnemySpawn");
        }

        public override void EnemyDeath()
        {
            EnemySpawner.Instance.RemoveFallenEnemy(gameObject);
        }

        public void ResetVelocity()
        {
            Rb.velocity = Vector3.zero;
        }
    }
}
