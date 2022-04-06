using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct LevelData
{
    public string _name;
    public float _completionTime;
}

public class GoogleSheets : MonoBehaviour
{
    private const string ColletedBy = "";
    private const string CompletionTime = "";

    private string url = "https://forms.gle/qsbLoCrpoCYkpTQdA";
    void SubmitData(string fieldName, string data)
    {
        StartCoroutine(ISubmitData(fieldName, data));
    }

    IEnumerator ISubmitData(string fieldName, string data)
    {
        
        yield return null;
    }

}
