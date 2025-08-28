using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationEventEntry
{
    public UnityEvent onTriggered;
    [Range(0f, 1f)] public float triggerTime;
    [HideInInspector] public bool hasTriggered;
}

public class AnimationEventManager : MonoBehaviour
{
    public List<AnimationEventEntry> eventEntries;
}
