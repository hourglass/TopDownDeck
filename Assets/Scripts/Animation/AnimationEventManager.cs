using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AnimationEventEntry
{
    public string eventName;
    [Range(0f, 1f)] public float triggerTime;
    [HideInInspector] public bool hasTriggered;
}

public class AnimationEventManager : MonoBehaviour
{
    public AnimationEventEntry[] eventEntries;

    public void RegisterAnimationEvent()
    {
        
    }

    private void Initialize()
    {
        
    }
}
