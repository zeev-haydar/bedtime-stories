using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemies;
using Managers;

namespace GameObjects
{
    public abstract class EnemyObject : MonoBehaviour
    {
        public Animator animator;
        private BedObject bedObject;
        private SpriteRenderer sprite;
        private Rigidbody rb;

        protected Animator Animator { get => animator; set => animator = value; }

        [HideInInspector] public bool isAttacking = false;
        [HideInInspector] public bool isHit = false;
        [HideInInspector] public bool isDead = false;
        protected BedObject BedObject 
        { 
            get 
            {
                if (!bedObject)
                {
                    bedObject = GameObject.FindGameObjectWithTag("Bed").GetComponent<BedObject>();
                }
                return bedObject;
            } 
        }
        protected SpriteRenderer Sprite { get => sprite; set => sprite = value; }
        public Rigidbody Rb { get => rb; set => rb = value; }

        public abstract void TakeHit(int damage);
        public abstract void EnemyDeath();

        protected Vector2 RandomTargetPoint(Bounds bounds)
        {
            return new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        }
    }
}
