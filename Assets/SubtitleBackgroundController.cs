using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleBackgroundController : MonoBehaviour
{

    public float Alpha => image.color.a;
    public float timeAlive = 0.0f;
    private Image image;


    private bool isFading = false;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void EnableImage()
    {
        StopAllCoroutines();
        timeAlive = 0;
        isFading = false;
        DOTween.Kill(currentTween);
        
        StartCoroutine(EnableImageCoroutine());
    }

    private int currentTween;
    private IEnumerator EnableImageCoroutine()
    {
        yield return new WaitForEndOfFrame();
        currentTween = image.DOColor(Color.white, 0.5f).intId;

    }

    public void DisableImage()
    {
        StartCoroutine(DisableGameObject());
    }

    private IEnumerator DisableGameObject(float delay = 3)
    {
        yield return new WaitForSeconds(delay);
        currentTween = image.DOColor(new Color(255, 255, 255, 0.0f), 0.2f).intId;
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > 7 && !isFading)
        {
            isFading = true;
            StartCoroutine(DisableGameObject(0));
        }
    }
}
