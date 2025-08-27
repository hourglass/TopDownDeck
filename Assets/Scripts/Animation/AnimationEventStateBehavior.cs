using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationEventStateBehavior : StateMachineBehaviour
{
    public string stateName;

    private AnimationEventReceiver receiver;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.TryGetComponent(out receiver))
        {
            Debug.LogWarning($"AnimationEventReciver not found on {animator.gameObject.name}");
            return;
        }

        if (!receiver.cachedEntries.ContainsKey(stateName))
        {
            Debug.LogWarning($"State '{stateName}' not found in AnimationEventReciver.cachedEntries on {animator.gameObject.name}");
            return;
        }

        for (int i = 0; i < receiver.cachedEntries[stateName].Length; i++)
        {
            receiver.cachedEntries[stateName][i].hasTriggered = false;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (receiver == null) return;

        float currentTime = stateInfo.normalizedTime % 1;

        for (int i = 0; i < receiver.cachedEntries[stateName].Length; i++)
        {
            // 현재 시간이 다음 triggerTime보다 작으면 종료
            if (currentTime < receiver.cachedEntries[stateName][i].triggerTime)
            {
                break;
            }

            if (!receiver.cachedEntries[stateName][i].hasTriggered && currentTime >= receiver.cachedEntries[stateName][i].triggerTime)
            {
                /*
                EM.OnAnimationEventTriggered(reciver.cachedEntries[stateName][i].eventName);
                Debug.Log($"Event triggered: {reciver.cachedEntries[stateName][i].eventName} at {reciver.cachedEntries[stateName][i].triggerTime * 100f}%");
                */
                receiver.cachedEntries[stateName][i].hasTriggered = true;
            }
        }
    }
}
