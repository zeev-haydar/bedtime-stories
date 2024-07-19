using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class InteractHinter : MonoBehaviour
    {
        [SerializeField]
        public GameObject hintPrefab;

        private GameObject interactable = null;
        private GameObject hint = null;

        private Vector3 translate = new Vector3(-6.22f, 4.5f, 0);

        public void SetInteract(GameObject newInteractable)
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
                }
                interactable = newInteractable;
            }
        }
    }

}
