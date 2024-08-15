using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tutorialGoblin : MonoBehaviour
{
    public GameObject gameObject, gameManager, hintSprite;
    public string[] hintTexts;

    public int index = 0;

    [SerializeField] private TMP_Text hintText;
    private Animator animator;
    //[SerializeField] private AudioSource textSoundEffect;
    private const float dialogueSpeed = 0.1f;
    private Coroutine dialogueCoroutine, scaleCoroutine;

    private void Awake()
    {
        hintSprite.transform.localScale = Vector3.zero;
        scaleCoroutine = StartCoroutine(ScaleDialog(false));
        dialogueCoroutine = StartCoroutine(PlayDialog());
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueCoroutine != null)
            {
                StopAllCoroutines();
                hintText.text = hintTexts[index];
                dialogueCoroutine = null;
            }
            else
            {
                if (index < hintTexts.Length - 1)
                {
                    hintText.text = "";
                    index++;
                    dialogueCoroutine = StartCoroutine(PlayDialog());
                }
                else
                {
                    scaleCoroutine = StartCoroutine(ScaleDialog(true));
                    Time.timeScale = 1f;
                }
            }
        }
    }

    IEnumerator ScaleDialog(bool toZero)
    {
        if (toZero)
        {
            animator.SetTrigger("FinishTutorial");
            while (hintSprite.transform.localScale.x > 0)
            {
                hintSprite.transform.localScale = Vector3.MoveTowards(hintSprite.transform.localScale, Vector3.zero, 0.1f);
                yield return null;
            }
        } else
        {
            while (hintSprite.transform.localScale.x < 3)
            {
                hintSprite.transform.localScale = Vector3.MoveTowards(hintSprite.transform.localScale, Vector3.one * 3, 0.1f);
                yield return null;
            }
        }
    }

    IEnumerator PlayDialog()
    {
        foreach (char c in hintTexts[index].ToCharArray())
        {
            //textSoundEffect.pitch = Random.Range(1f, 1.2f);
            //textSoundEffect.Play();
            hintText.text += c;
            yield return new WaitForSecondsRealtime(dialogueSpeed);
        }
        dialogueCoroutine = null;
    }

    void KillGoblin()
    {
        gameManager.SetActive(true);
        Destroy(gameObject);
    }
}
