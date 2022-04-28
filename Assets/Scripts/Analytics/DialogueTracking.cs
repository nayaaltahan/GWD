using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public enum DialogueTrackingEvent
{
    SessionStarted,
    DialogueOptionChosen,
    DialogueDidNotChoose
}

public class DialogueTracking
{
    private static string sessionID = Guid.NewGuid().ToString();

    public static void SendTrackingEvent(DialogueTrackingEvent eventType, IDictionary<string, object> data = null)
    {
        string contentString = eventType switch
        {
            DialogueTrackingEvent.SessionStarted => "sessionStarted",
            DialogueTrackingEvent.DialogueOptionChosen => "optionChosen",
            DialogueTrackingEvent.DialogueDidNotChoose => "noChoice",
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

    private static string DomainName
    {
        get
        {
            string domainName;
            if (Debug.isDebugBuild)
            {
                domainName = "debug_dialogue";
            }
            else
            {
                domainName = "live_dialogue";
            }

            return domainName;
        }
    }
}
