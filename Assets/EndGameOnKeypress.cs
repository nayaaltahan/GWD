using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameOnKeypress : MonoBehaviour
{
    float exitTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(exitTime);

        Application.Quit();

    }
}
