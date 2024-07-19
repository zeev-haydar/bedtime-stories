using UnityEngine;
using System;
using Exceptions;
using Player;

namespace GameObjects
{
    public class HoleObject : MonoBehaviour, IAppliableObject
    {
        public void Apply(ItemObject item, PlayerObject player)
        {
            try
            {
                GetComponentInParent<BedObject>().bed.Apply(item.item);
                Destroy(item.gameObject);
                Destroy(gameObject);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        public bool CanApply(ItemObject item)
        {
            return item is BlanketObject;
        }
    }
}