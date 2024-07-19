using Enemies;
using GameObjects;
using Items;
using UnityEngine;

namespace Player
{
    class Player
    {
        private float speed;
        private int attack;
        private Item item;
        private float attackRange;

        public float AttackRange {  get { return attackRange; } }

        public float Speed { 
            get => speed;
            set {
                speed = value; 
            } 
        }

        public Player(float speed, int attack, float attackRange)
        {
            this.speed = speed;
            this.attack = attack;
            this.item = null;
            this.attackRange = attackRange;
        }

        public void Attack(EnemyObject enemy)
        {
            enemy.TakeHit(attack);
        }
        public void Pickup(Item item)
        {
            this.item = item;
        }
        public void Drop()
        {
            this.item = null;
        }
        public void Put(IApplicable applicable)
        {
            applicable.Apply(this.item);
            this.item = null;
        }

        public bool IsHoldingItem() 
        { 
            return this.item != null; 
        }
    }
}