using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class MeleeEnemy : Enemy
    {
        private float speed;
        //public int Attack { get => attack; set => attack = value; }
        public float Speed { get => speed; set => speed = value; }

        public MeleeEnemy(int maxHealth, float speed)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            //Attack = attack;
            Speed = speed;
        }
    }
}
