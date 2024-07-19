using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialGoblin : MonoBehaviour
{
    public GameObject gameObject, gameManager;
    // Update is called once per frame
    void KillGoblin()
    {
        gameManager.SetActive(true);
        Destroy(gameObject);
    }
}
