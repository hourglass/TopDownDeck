using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AnimationEventEntry
{
    public UnityEvent onTriggered;
    [Range(0f, 0.9f)] public float triggerTime;
    [HideInInspector] public bool hasTriggered;
}

public class AnimationEventController : MonoBehaviour
{
    public List<AnimationEventEntry> eventEntries;
    [HideInInspector] public GameObject subscriber;

    private Animator animator;
    private AnimatorStateInfo stateInfo;

    public void Initialize(Animator anim)
    {
        if (anim == null)
        {
            Debug.LogWarning("AnimationEventController: Animator is null");
            return;
        }

        animator = anim;
        subscriber = anim.gameObject;
    }

    public void LogicUpdate()
    {
        if (animator == null || animator.runtimeAnimatorController == null)
        {
            Debug.LogWarning("Animator or runtimeAnimatorController is null");
            return;
        }
        
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
       
        float currentTime = stateInfo.normalizedTime % 1;

        for (int i = 0; i < eventEntries.Count; i++)
        {
            // 현재 시간이 다음 triggerTime보다 작으면 종료
            if (currentTime < eventEntries[i].triggerTime)
            {
                break;
            }

            if (!eventEntries[i].hasTriggered && currentTime >= eventEntries[i].triggerTime)
            {
                eventEntries[i].onTriggered?.Invoke();
                eventEntries[i].hasTriggered = true;
            }
        }
    }

    public void ResetTrigger()
    {
        for (int i = 0; i < eventEntries.Count; i++)
        {
            eventEntries[i].hasTriggered = false;
        }
    }
}
