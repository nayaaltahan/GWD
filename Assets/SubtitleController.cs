using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{

    private float duration, timeAlive;
    private TextMeshProUGUI subtitle;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnEnable()
    {
        subtitle = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= duration)
            gameObject.SetActive(false);
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
