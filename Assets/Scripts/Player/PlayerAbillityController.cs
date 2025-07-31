using System.Collections.Generic;
using UnityEngine;

public class PlayerAbillityController : MonoBehaviour
{
    public MotionSetData attackMotionSet;
    public MotionSetData chargeMotionSet;
    public MotionSetData skillMotionSet;

    private Animator animator;
    private AnimatorOverrideController overrideController;
    private Dictionary<string, Dictionary<int, AnimationClip[]>> cachedAnimations = new Dictionary<string, Dictionary<int, AnimationClip[]>>();
    private Dictionary<string, int> currentMotionSteps = new Dictionary<string, int>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator.runtimeAnimatorController != null)
        {
            overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (overrideController == null)
            {
                overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
                animator.runtimeAnimatorController = overrideController;
            }
        }
        else
        {
            Debug.LogError("Animator의 runtimeAnimatorController가 설정되지 않았습니다!");
        }

        // 초기 캐싱
        cachedAnimations["Attack"] = new Dictionary<int, AnimationClip[]>();
        cachedAnimations["Charge"] = new Dictionary<int, AnimationClip[]>();
        cachedAnimations["Skill"] = new Dictionary<int, AnimationClip[]>();

        InitAnimations("Attack", attackMotionSet);
        InitAnimations("Charge", chargeMotionSet);
        InitAnimations("Skill", skillMotionSet);
    }

    public void InitAnimations(string motionType, MotionSetData motionSet)
    {
        if (motionSet == null) return;
        cachedAnimations[motionType].Clear();
        for (int i = 0; i < motionSet.motions.Length; i++)
        {
            cachedAnimations[motionType][i + 1] = attackMotionSet.motions[i].animations;
        }
        currentMotionSteps[motionType] = 1;
        UpdateAnimations(motionType);
    }

    public void UpdateAnimations(string motionType)
    {
        if (!cachedAnimations.ContainsKey(motionType) || cachedAnimations[motionType].Count == 0)
        {
            // 기본 클립으로 복구
            overrideController[$"{motionType}_E"] = null;
            overrideController[$"{motionType}_N"] = null;
            overrideController[$"{motionType}_NE"] = null;
            overrideController[$"{motionType}_S"] = null;
            overrideController[$"{motionType}_SE"] = null;
            return;
        }

        // motionStep 증가 및 초기화
        int currentStep = currentMotionSteps[motionType];
        int maxStep = cachedAnimations[motionType].Count;
        if (currentStep > maxStep)
        {
            currentStep = 1;
        }
        currentMotionSteps[motionType] = currentStep + 1;

        // 클립 동적 교체
        AnimationClip[] clips = cachedAnimations[motionType][currentStep];
        overrideController[$"{motionType}_E"] = clips[0];
        overrideController[$"{motionType}_N"] = clips[1];
        overrideController[$"{motionType}_NE"] = clips[2];
        overrideController[$"{motionType}_S"] = clips[3];
        overrideController[$"{motionType}_SE"] = clips[4];
    }
}