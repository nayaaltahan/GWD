using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public enum DialogueTrackingEvent
{
    SessionStarted,
    DialogueOptionChosen,
    DialogueDidNotChoose,
    SessionID
}

public class DialogueTracking
{
    public static void SendTrackingEvent(DialogueTrackingEvent eventType, IDictionary<string, object> data = null)
    {
        string contentString = eventType switch
        {
            DialogueTrackingEvent.SessionStarted => "sessionStarted",
            DialogueTrackingEvent.DialogueOptionChosen => "optionChosen",
            DialogueTrackingEvent.DialogueDidNotChoose => "noChoice",
            DialogueTrackingEvent.SessionID => "savedSessionID",
            _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
        };
        
        data = SetupDataDictionary(data);

        foreach (KeyValuePair<string, object> kvp in data)
            Debug.Log ($"dialog_{contentString}: " + $"{kvp.Key}: {kvp.Value}");
        
        Events.CustomData($"dialog_{contentString}", data);
        Events.Flush();
    }

    private static IDictionary<string, object> SetupDataDictionary(IDictionary<string, object> data)
    {
        if (data == null)
        {
            data = new Dictionary<string, object>();
        }

        return data;
    }
}
