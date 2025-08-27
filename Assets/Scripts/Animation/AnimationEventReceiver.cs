using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public Dictionary<string, AnimationEventEntry[]> cachedEntries = new Dictionary<string, AnimationEventEntry[]>();

    public void RegisterEventEntries(string stateName, AnimationEventEntry[] entries)
    {
        cachedEntries[stateName] = entries;
    }
}
