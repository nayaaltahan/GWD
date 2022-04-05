using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    void Start()
    {
        if (!SceneManager.GetSceneByName("Swamp").isLoaded)
            SceneManager.LoadScene("Swamp", LoadSceneMode.Additive);
    }
}
