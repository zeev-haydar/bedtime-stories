using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    public Wave wave;
    
    public void LoadLevel()
    {
        PlayerManager.Instance.StartGame();
        LevelManager.Instance.LoadLevel(wave);
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
