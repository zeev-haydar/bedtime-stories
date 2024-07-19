using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class Enemy
    {
        private int maxHealth, health;

        public int MaxHealth { get => maxHealth; set { if (value > 0) { maxHealth = value; } else { maxHealth = 1; } } }
        public int Health { get => health; set { health = Mathf.Clamp(value,0,maxHealth); } }

        public void ReduceHealth(int damage)
        {
            Health -= damage;
        }
    }

}
