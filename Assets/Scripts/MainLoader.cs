using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (!SceneManager.GetSceneByName("Main").isLoaded && !SceneManager.GetSceneByName("MainABTest").isLoaded)
            SceneManager.LoadScene("Main", LoadSceneMode.Additive);
    }

}
