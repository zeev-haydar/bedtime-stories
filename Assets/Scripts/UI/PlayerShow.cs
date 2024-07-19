using System.Collections;
using System.Collections.Generic;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class PlayerShow : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI charNameLabel;
        [SerializeField] private TextMeshProUGUI descriptionLabel;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private int playerNumber;

        [SerializeField] private string charName;
        [SerializeField] private string description;

        // Update is called once per frame
        void Update()
        {
            if (playerManager.CurrentPlayerCount >= playerNumber)
            {
                image.gameObject.SetActive(true);
                charNameLabel.text = charName;
                descriptionLabel.text = description;
            }
            else
            {
                image.gameObject.SetActive(false);
                charNameLabel.text = "???";
                descriptionLabel.text = "?????";
            }
        }
    }

}
