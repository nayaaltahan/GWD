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




    public IEnumerator ActivateSpeechBubble(float fadeInTime)
    {
        // For some reason we need this delay, or the components are not found, as the gameobject is still disabled for a little bit
        yield return new WaitForSeconds(0.1f);
        image.rectTransform.DOScale(Vector3.one, fadeInTime).SetEase(Ease.OutBounce);
        canvasGroup.DOFade(1, fadeInTime);
    }

    public IEnumerator DisableSpeechBubble(float fadeOutTime, float fadeDelay = 0.0f)
    {
        yield return new WaitForSeconds(fadeDelay);
        canvasGroup.DOFade(0, fadeOutTime);
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
