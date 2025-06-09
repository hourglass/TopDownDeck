using System;
using UnityEngine;

public class WeaponAnimationHandler : MonoBehaviour
{
    public event Action OnFinish;

    private void AnimationFinishedTrigger() => OnFinish?.Invoke();
}
