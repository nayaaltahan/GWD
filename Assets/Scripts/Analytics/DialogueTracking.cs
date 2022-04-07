using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class DialogueTracking 
{
    public enum DialogueTrackingEvent
    {
        SessionStarted,
        DialogueOptionChosen,
        DialogueTriggered
    }
    
    public static void SendTrackingEvent(DialogueTrackingEvent eventType, IDictionary<string, string> data)
    {
        string contentString = eventType switch
        {
            DialogueTrackingEvent.SessionStarted => "enterClub",
            DialogueTrackingEvent.DialogueOptionChosen => "exitClub",
            DialogueTrackingEvent.DialogueTriggered => "npcInteraction",
            _ => throw new ArgumentOutOfRangeException(nameof(eventType), eventType, null)
        };

        string domainName;
        if (Debug.isDebugBuild)
        {
            domainName = "debug.dialogue";
        }
        else
        {
            domainName = "live.dialogue";
        }
        var analyticsResult = Analytics.CustomEvent($"{domainName}.{contentString}.{data}");
        Debug.Log($"Dialogue tracking event results: {analyticsResult} ");
    }
}
