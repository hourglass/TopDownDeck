using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver
{
    public Dictionary<string, List<AnimationEventEntry>> cachedEntries = new Dictionary<string, List<AnimationEventEntry>>();

    private Animator animator;
    private AnimatorStateInfo stateInfo;


    public void Initialize(Animator anim)
    {
        animator = anim;
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Animator or runtimeAnimatorController is null");
        }
    }

    public void RegisterEventEntries(string stateName, List<AnimationEventEntry> entries)
    {
        if (entries == null)
        {
            Debug.LogWarning($"RegisterEventEntries: Entries is null for state {stateName}");
            return;
        }

        if (!cachedEntries.ContainsKey(stateName))
        {
            cachedEntries[stateName] = entries;
        }
        else
        {
            cachedEntries[stateName].AddRange(entries);
        }

        // triggerTime 순으로 정렬
        cachedEntries[stateName].Sort((a, b) => a.triggerTime.CompareTo(b.triggerTime));
        for (int i = 0; i < cachedEntries[stateName].Count; i++)
        {
            cachedEntries[stateName][i].hasTriggered = false;
        }
    }

    public void LogicUpdate(string stateName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Animator or runtimeAnimatorController is null");
            return;
        }

        if (!cachedEntries.ContainsKey(stateName))
        {
            Debug.LogWarning($"State '{stateName}' not found in cachedEntries");
            return;
        }

        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float currentTime = stateInfo.normalizedTime % 1;

        for (int i = 0; i < cachedEntries[stateName].Count; i++)
        {
            // 현재 시간이 다음 triggerTime보다 작으면 종료
            if (currentTime < cachedEntries[stateName][i].triggerTime)
            {
                break;
            }

            if (!cachedEntries[stateName][i].hasTriggered && currentTime >= cachedEntries[stateName][i].triggerTime)
            {
                cachedEntries[stateName][i].onTriggered?.Invoke();
                cachedEntries[stateName][i].hasTriggered = true;
            }
        }
    }

    public void ResetTrigger(string stateName)
    {
        if (!cachedEntries.ContainsKey(stateName))
        {
            Debug.LogWarning($"State '{stateName}' not found in cachedEntries");
            return;
        }

        for (int i = 0; i < cachedEntries[stateName].Count; i++)
        {
            cachedEntries[stateName][i].hasTriggered = false;
        }
    }
}
