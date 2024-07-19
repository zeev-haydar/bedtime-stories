using System.Collections.Generic;
using BedMechanics;
using GameObjects;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AmmoCounter : MonoBehaviour
    {
        [SerializeField] private List<Sprite> spriteList;
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private CannonObject cannon;

        private int ammoCount = 0;

        // Update is called once per frame
        void Update()
        {
            ammoCount = Mathf.CeilToInt(5.0f * (float)cannon.cannon.Ammo / Cannon.REFILL_AMOUNT);
            if (ammoCount >= 0 && ammoCount <= 5)
            {
                renderer.sprite = spriteList[ammoCount];
            }
        }
    }

}
