using UnityEngine;

public class GameDebugger : MonoBehaviour
{
    #if UNITY_EDITOR
    [SerializeField] private float speedMultiplier = 20f;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Time.timeScale = speedMultiplier;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            Time.timeScale = 1f;
        }
    }
    #endif
}