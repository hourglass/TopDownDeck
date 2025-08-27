using System.Collections.Generic;
using UnityEngine;

public class MotionController
{
    private Animator animator;
    private AnimatorOverrideController overrideController;
    private Dictionary<string, Dictionary<int, AnimationClip[]>> cachedAnimations = new Dictionary<string, Dictionary<int, AnimationClip[]>>();
    private Dictionary<string, int> currentMotionSteps = new Dictionary<string, int>();


    public void Initialize(Animator anim)
    {
        animator = anim;
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
            Debug.LogError("Animator�� runtimeAnimatorController�� �������� �ʾҽ��ϴ�!");
        }
    }

    public void RegisterMotionSet(string motionType, MotionSetData motionSet)
    {
        // MotionSet ���
        cachedAnimations[motionType] = new Dictionary<int, AnimationClip[]>();
        SetAnimations(motionType, motionSet);
    }

    public void SetAnimations(string motionType, MotionSetData motionSet)
    {
        if (motionSet == null) return;

        // MotionSet ĳ��
        cachedAnimations[motionType].Clear();
        for (int i = 0; i < motionSet.motions.Length; i++)
        {
            cachedAnimations[motionType][i] = motionSet.motions[i].animations;
        }
        currentMotionSteps[motionType] = 0;
        UpdateAnimations(motionType);
    }

    public void UpdateAnimations(string motionType)
    {
        if (!cachedAnimations.ContainsKey(motionType) || cachedAnimations[motionType].Count == 0)
        {
            // �⺻ Ŭ������ ����
            overrideController[$"{motionType}_E"] = null;
            overrideController[$"{motionType}_N"] = null;
            overrideController[$"{motionType}_NE"] = null;
            overrideController[$"{motionType}_S"] = null;
            overrideController[$"{motionType}_SE"] = null;
            return;
        }

        // motionStep ���� �� �ʱ�ȭ
        int currentStep = currentMotionSteps[motionType];
        int maxStep = cachedAnimations[motionType].Count - 1;
        if (currentStep > maxStep)
        {
            currentStep = 0;
        }
        currentMotionSteps[motionType] = currentStep + 1;

        // Ŭ�� ���� ��ü
        AnimationClip[] clips = cachedAnimations[motionType][currentStep];

        overrideController[$"{motionType}_E"] = clips[0];
        overrideController[$"{motionType}_N"] = clips[1];
        overrideController[$"{motionType}_NE"] = clips[2];
        overrideController[$"{motionType}_S"] = clips[3];
        overrideController[$"{motionType}_SE"] = clips[4];
    }
}