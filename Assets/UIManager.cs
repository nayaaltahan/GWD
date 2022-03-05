using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Character Controller")]
    [SerializeField] private List<GameObject> charSelect;
    public static UIManager instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("More than one UI Manager in the scene");
    }

    public void TurnCharSelectUIOn(bool value)
    {
        for (int i = 0; i < charSelect.Count; i++)
        {
            charSelect[i].SetActive(value);
        }
    }
}
