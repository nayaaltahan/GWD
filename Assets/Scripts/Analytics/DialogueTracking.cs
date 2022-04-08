using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public enum DialogueTrackingEvent
{
    SessionStarted,
    DialogueOptionChosen,
    DialogueTriggered,
    DialogueDidNotChoose
}

public class DialogueTracking
{
    private static string sessionID = Guid.NewGuid().ToString();

    public static void SendTrackingEvent(DialogueTrackingEvent eventType, IDictionary<string, object> data = null)
    {
        string contentString = eventType switch
        {
            DialogueTrackingEvent.SessionStarted => "SessionStarted",
            DialogueTrackingEvent.DialogueOptionChosen => "DialogueOptionChosen",
            DialogueTrackingEvent.DialogueTriggered => "DialogueTriggered",
            DialogueTrackingEvent.DialogueDidNotChoose => "DialogueDidNotChoose",
            _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
        };
        
        data = SetupDataDictionary(data);

        Events.CustomData($"{DomainName}.{contentString}", data);
    }

    private static IDictionary<string, object> SetupDataDictionary(IDictionary<string, object> data)
    {
        if (data == null)
        {
            data = new Dictionary<string, object>();
        }

        data["SessionID"] = sessionID;
        data["Timestamp"] = DateTime.Now;
        data["KnotName"] = DialogueManager.CurrentKnotName?? "No KnotName";;
        
        return data;
    }

    private static string DomainName
    {
        get
        {
            string domainName;
            if (Debug.isDebugBuild)
            {
                domainName = "debug.dialogue";
            }
            else
            {
                domainName = "live.dialogue";
            }

            return domainName;
        }
    }
}
