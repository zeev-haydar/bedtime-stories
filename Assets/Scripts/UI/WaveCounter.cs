using System.Collections;
using System.Collections.Generic;
using GameObjects;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WaveCounter : MonoBehaviour
    {
        [SerializeField] private List<Sprite> spriteList;
        [SerializeField] private Image image;
        [SerializeField] private WaveManager waveManager;

        private int waveCount = 0;

        // Update is called once per frame
        void Update()
        {
            waveCount = waveManager.CurrWave;
            if (waveCount > 0 && waveCount <= 5)
            {
                image.sprite = spriteList[waveCount];
            }
        }
    }

}
