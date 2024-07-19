using System.Collections;
using System.Collections.Generic;
using GameObjects;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BedHealth : MonoBehaviour
    {
        [SerializeField] private List<Sprite> spriteList;
        [SerializeField] private Image image;
        [SerializeField] private BedObject bedObject;

        private int health;

        // Update is called once per frame
        void Update()
        {
            health = bedObject.GetHealthNormalized();
            if (health >= 0 && health <= 5)
            {
                image.sprite = spriteList[health];
            }
        }
    }

}
