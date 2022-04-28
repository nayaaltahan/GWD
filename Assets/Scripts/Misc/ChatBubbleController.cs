using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubbleController : MonoBehaviour
{
    private Image image;
    private CanvasGroup canvasGroup;

    private Color startColor;

    private Color fadedColor;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
            image = GetComponent<Image>();
            canvasGroup = GetComponent<CanvasGroup>();
            startColor = image.color;

            // fadedColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);
            fadedColor = new Color(0, 0, 0, 0);
            // image.color = fadedColor;
            image.rectTransform.localScale = Vector3.zero;
            rectTransform = GetComponent<RectTransform>();

    }




    public IEnumerator OnEnableCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        canvasGroup.alpha = 1;
        image.rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(DisableSpeechBubble(DialogueManager.instance.PlayerChoiceTimeLimit));
    }

    public IEnumerator ActivateSpeechBubble(float fadeInTime)
    {
        // For some reason we need this delay, or the components are not found, as the gameobject is still disabled for a little bit
        yield return new WaitForSeconds(0.1f);
        image.rectTransform.DOScale(Vector3.one, fadeInTime).SetEase(Ease.OutBounce);
        canvasGroup.DOFade(1, fadeInTime).SetEase(Ease.InSine);
    }

    public IEnumerator DisableSpeechBubble(float fadeOutTime, float fadeDelay = 0.0f, bool shouldShrink = false)
    {
        if (fadeDelay > 0.0f)
        {
            StopAllCoroutines();
            var killed = DOTween.KillAll();
            Debug.Log("Killed: " + killed);
            canvasGroup.alpha = 1;
            Debug.Log("Fade delay");
        }
        yield return new WaitForSeconds(fadeDelay);
        canvasGroup.DOFade(0, fadeOutTime);
        if (shouldShrink)
            image.rectTransform.DOScale(Vector3.zero, fadeOutTime).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(fadeOutTime);

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // image.color = fadedColor;
        canvasGroup.alpha = 0;
        image.rectTransform.localScale = Vector3.zero;
    }
}
