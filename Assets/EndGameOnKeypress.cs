using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameOnKeypress : MonoBehaviour
{
    float exitTime = 20f;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(EndGame());
    }

    // Update is called once per frame
    void Update()
    {

    }


    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(exitTime);
        Debug.Log("End");

        Application.Quit();

    }
}
