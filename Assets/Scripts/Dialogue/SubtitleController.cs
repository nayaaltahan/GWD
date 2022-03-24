using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{
    [SerializeField]
    private float timeToFade = 1.0f;

    private float duration, timeAlive;
    private TextMeshProUGUI subtitle;
    private bool isFading = false;


    private void OnEnable()
    {
        isFading = false;
        timeAlive = 0.0f;
        subtitle = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= duration && !isFading)
            StartCoroutine(DisableSubtitle());
    }

    private IEnumerator DisableSubtitle()
    {
        isFading = true;
        var color = subtitle.faceColor;
        subtitle.DOColor(new Color(color.r, color.g, color.b, 0.0f), timeToFade);
        yield return new WaitForSeconds(timeToFade);
        Destroy(gameObject);
        // TODO: Use object pooler
        //gameObject.SetActive(false);

    }

    private void OnDisable()
    {
        duration = 0.0f;
    }

    public void CreateSubtitle(string text, Color fontColor, float duration, Transform parent)
    {
        transform.SetParent(parent, false);
        subtitle.text = text;
        subtitle.faceColor = fontColor;
        this.duration = duration;
    }
}
