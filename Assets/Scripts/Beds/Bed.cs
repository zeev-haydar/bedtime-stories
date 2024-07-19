using System;
using System.Collections;
using System.Collections.Generic;
using Exceptions;
using Items;
using UnityEngine;

namespace BedMechanics
{
    public class Bed : IApplicable
    {
        public int mainHealth;
        public int maxMainHealth;


        public string cannon;

        public Bed(int maxMainHealth)
        {
            this.maxMainHealth = maxMainHealth;
            mainHealth = maxMainHealth;
        }

        public void ReduceHealth()
        {
            mainHealth--;
        }


        public void Repair()
        {
            mainHealth++;
        }


        private void DestroyBed(){
            if (mainHealth <= 0){
                Debug.Log("Bed Destroyed");
            }
        }

        public void Apply(Item item)
        {
            if (!CanApply(item)) throw new CannotApplyException(item.name, this.GetType().Name);

            Repair();
            Debug.Log("Bed Repaired");
        }

        public bool CanApply(Item item)
        {
            return item is Blanket;
        }
    }
}
