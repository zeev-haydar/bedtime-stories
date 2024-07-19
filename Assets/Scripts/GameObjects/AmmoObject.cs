using UnityEngine;
using Items;
using Exceptions;
using System.Collections.Generic;

namespace GameObjects
{
    public class AmmoObject :ItemObject
    {
        public List<Sprite> possibleSprites;
        public void Awake()
        {
            item = new Ammo();
            GetComponent<SpriteRenderer>().sprite = possibleSprites[Random.Range(0, possibleSprites.Count)];
        }
        

    }
} 