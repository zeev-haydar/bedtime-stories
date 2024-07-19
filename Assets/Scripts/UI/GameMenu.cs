using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    private GameObject[] players;
    private bool paused = false;
    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        players = players.Where(player => player.GetComponent<PlayerInput>() != null).ToArray();
        foreach (var player in players)
        {
            player.GetComponent<PlayerInput>().currentActionMap.FindAction("Pause").canceled += HandlePause;
        }
        HandleContinue();
    }
    private void OnDestroy()
    {
        foreach (var player in players)
        {
            player.GetComponent<PlayerInput>().currentActionMap.FindAction("Pause").canceled -= HandlePause;
        }
    }
    public void HandleToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void HandleContinue()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        paused = false;
    }
    public void HandlePause(InputAction.CallbackContext context)
    {
        Debug.Log("Pencet escape");
        if (context.canceled)
        {
            if (paused)
            {
                HandleContinue();
            }
            else
            {
                Time.timeScale = 0; 
                gameObject.SetActive(true);
                paused = true;
            }
        }
    }
}