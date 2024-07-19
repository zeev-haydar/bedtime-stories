using UnityEngine;
using System;
using Items;
using Exceptions;

namespace GameObjects
{
    public class BlanketObject :ItemObject
    {


        public void Awake()
        {
            item = new Blanket();
        }


    }
} 