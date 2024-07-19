using System;
using Enemies;
using Exceptions;
using Items;
using UnityEngine;

namespace BedMechanics
{
    public class Cannon : IApplicable
    {
        private int ammo = 0;
        public const int REFILL_AMOUNT = 5;

        public float ProjectileSpeed {get;}
        public int Ammo { get => ammo; set => ammo = value; }

        public Cannon()
        {
            ProjectileSpeed = 12;
        }

        public void Apply(Item item)
        {
            if (!CanApply(item)) throw new CannotApplyException(item.name, this.GetType().Name);
            Debug.Log("Applied this item to Cannon");
            Refill();

        }

        public bool CanApply(Item item)
        {
            Debug.Log(item);
            return item is Ammo || item == null;
        }

        public void Refill()
        {
            Ammo = REFILL_AMOUNT;
            Debug.Log("Ammo refilled");
        }        

    }
}