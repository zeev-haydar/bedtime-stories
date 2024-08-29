using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class PlayerShow : MonoBehaviour
    {
        [Header("Character Info")]
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI charNameLabel;
        [SerializeField] private TextMeshProUGUI descriptionLabel;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private int playerNumber;

        [SerializeField] private string charName;
        [SerializeField] private string description;

        [Header("Ready Indicator")]
        [SerializeField] private TextMeshProUGUI readyStatusLabel;
        [SerializeField] private TextMeshProUGUI readyHintLabel;
        [SerializeField] private Color readyColor;
        [SerializeField] private Color notReadyColor;

        // Update is called once per frame
        void Update()
        {
            if (playerManager.CurrentPlayerCount >= playerNumber)
            {
                if (playerManager.PlayerReadyStatus[playerNumber - 1])
                {
                    readyStatusLabel.text = "Ready";
                    readyStatusLabel.color = readyColor;

                    readyHintLabel.text = $"Press {GetReadyButtonName()} to unready";
                }
                else
                {
                    readyStatusLabel.text = "Not Ready";
                    readyStatusLabel.color = notReadyColor;

                    readyHintLabel.text = $"Press {GetReadyButtonName()} to ready";
                }

                image.gameObject.SetActive(true);
                charNameLabel.text = charName;
                descriptionLabel.text = description;

                readyStatusLabel.gameObject.SetActive(true);
                readyHintLabel.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
                charNameLabel.text = "???";
                descriptionLabel.text = "?????";
                readyStatusLabel.gameObject.SetActive(false);
                readyHintLabel.gameObject.SetActive(false);
            }
        }

        private string GetReadyButtonName()
        {
            GameObject player = PlayerManager.Instance.Players[playerNumber - 1];
            var readyButtonBinding = player.GetComponent<PlayerInput>().actions.FindAction("Pickup").bindings[0];
            string readyButtonName = readyButtonBinding.ToDisplayString();

            if (readyButtonBinding.path.StartsWith("<Keyboard>"))
            {
                readyButtonName = readyButtonName.Replace("Keyboard/", "");
            }
            else if (readyButtonBinding.path.StartsWith("<Gamepad>"))
            {
                readyButtonName = readyButtonName.Replace("Gamepad/", "");
            }

            return readyButtonName;
        }
    }
}
