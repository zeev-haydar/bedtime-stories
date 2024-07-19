using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class RangedEnemy : Enemy
    {
        private float speed;
        public float Speed { get => speed; set => speed = value; }

        public RangedEnemy(int maxHealth, float speed)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
            Speed = speed;
        }
    }
}
