using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color highlightColor = Color.yellow;
    private Image buttonImage;
    // Start is called before the first frame update
    void Start()
    {
        buttonImage = GetComponent<Image>();
    }

    public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData) {
        // buttonImage.alphaHitTestMinimumThreshold = 0.1f;
        Debug.Log("Enter angin");
    }

    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData) {
        // buttonImage.alphaHitTestMinimumThreshold = 0.5f;
        Debug.Log("Exit angin");
    }
}
