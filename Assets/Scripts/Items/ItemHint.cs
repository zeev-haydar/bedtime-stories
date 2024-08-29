using TMPro;
using UnityEngine;

public class ItemHint : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textMain;
    [SerializeField] private TextMeshProUGUI textShadow;

    public void SetText(string text) {
        textMain.text = text;
        textShadow.text = text;
    }
}