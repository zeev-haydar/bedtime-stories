using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InteractHinter : MonoBehaviour
    {
        [SerializeField]
        public GameObject hintPrefab;

        private GameObject interactable = null;
        private GameObject hint = null;

        private Vector3 translate = new Vector3(-6.22f, 4.5f, 0);

        public enum InteractType {
            PickUp,
            Use,
            Fill,
            Fix,
        }

        public void SetInteract(GameObject newInteractable, InteractType type)
        {
            if (interactable != newInteractable)
            {
                if (interactable != null && hint != null)
                {
                    Destroy(hint); hint = null;
                }

                if (newInteractable != null)
                {
                    hint = Instantiate(hintPrefab, newInteractable.transform);
                    hint.transform.localScale = new Vector3(
                        hint.transform.localScale.x / newInteractable.transform.lossyScale.x,
                        hint.transform.localScale.y / newInteractable.transform.lossyScale.y,
                        hint.transform.localScale.z / newInteractable.transform.lossyScale.z);
                    hint.transform.localPosition = new Vector3(
                        translate.x * hint.transform.localScale.x,
                        translate.y * hint.transform.localScale.y,
                        translate.z * hint.transform.localScale.z);

                    string buttonKeyName = GetButtonKeyName();
                    string interactionType = "";

                    switch (type)
                    {
                        case InteractType.PickUp:
                            interactionType = "Pick Up";
                            break;
                        case InteractType.Use:
                            interactionType = "Use";
                            break;
                        case InteractType.Fill:
                            interactionType = "Fill";
                            break;
                        case InteractType.Fix:
                            interactionType = "Fix";
                            break;
                    }

                    hint.GetComponent<ItemHint>().SetText($"{interactionType} ({buttonKeyName})");

                }

                interactable = newInteractable;
            }
        }

        private string GetButtonKeyName()
        {
            var readyButtonBinding = GetComponent<PlayerInput>().actions.FindAction("Pickup").bindings[0];
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
