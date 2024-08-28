using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static BGMManager instance;
    private AudioSource audioSource;
    public List<AudioClip> clips;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += ChangeActiveScene;
            audioSource = GetComponent<AudioSource>();

        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeActiveScene(Scene current, Scene next)
    {
        string currentName = current.name;
        string nextName = next.name;

        if (nextName == "Testing")
        {
            PlayBattleBGM();
        }

        if (nextName == "Main Menu")
        {
            PlayMainBGM();
        }
    }

    void PlayMainBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = clips[0];
        audioSource.PlayDelayed(0);
    }

    void PlayBattleBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = clips[1];
        audioSource.PlayDelayed(0);
    }
}
