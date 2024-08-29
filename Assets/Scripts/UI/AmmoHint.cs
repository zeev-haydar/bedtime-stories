using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class AmmoHint : MonoBehaviour
{
    [SerializeField] private float initialYOffset = -0.1f;
    [SerializeField] private float appearDuration = 0.3f;
    [SerializeField] private float stayDuration = 1f;

    private Coroutine appearCoroutine;
    private Vector3 initialPos;
    private CanvasGroup canvasGroup;

    private void Awake() {
        initialPos = transform.localPosition;

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void Show() {
        if (appearCoroutine != null) {
            StopCoroutine(appearCoroutine);
        }

        appearCoroutine = StartCoroutine(AppearCoroutine());
    }

    private IEnumerator AppearCoroutine() {
        float t = 0f;
        while (t < appearDuration) {
            t += Time.deltaTime;
            transform.localPosition = initialPos + Vector3.up * Mathf.Lerp(initialYOffset, 0f, t / appearDuration);
            canvasGroup.alpha = t / appearDuration;
            yield return null;
        }

        yield return new WaitForSeconds(stayDuration);

        t = 0f;
        while (t < appearDuration) {
            t += Time.deltaTime;
            transform.localPosition = initialPos + Vector3.up * Mathf.Lerp(0f, initialYOffset, t / appearDuration);
            canvasGroup.alpha = 1f - t / appearDuration;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        appearCoroutine = null;
    }
}
