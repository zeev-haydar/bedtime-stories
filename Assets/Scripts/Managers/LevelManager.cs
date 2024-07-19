using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        private Slider loadingSlider;

        private Wave levelWave;
        private static LevelManager instance;
        public Wave LevelWave { get => levelWave; }
        public static LevelManager Instance { get => instance; }

        private void Start()
        {
            loadingSlider = loadingScreen.GetComponentInChildren<Slider>();
            loadingSlider.maxValue = 0.9f;
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this);
        }

        public void LoadLevel(Wave wave)
        {
            levelWave = wave;
            StartCoroutine(LoadLevelScene());
        }

        private IEnumerator LoadLevelScene()
        {
            loadingScreen.SetActive(true);
            AsyncOperation loadScene = SceneManager.LoadSceneAsync("Testing");
            while (!loadScene.isDone)
            {
                loadingSlider.value = loadScene.progress;
                yield return null;
            }
            loadingScreen.SetActive(false);
        }
    }
}
