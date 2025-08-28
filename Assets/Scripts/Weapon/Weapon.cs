using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public MotionSetData MeleeAttackMotionSet;

    public MotionController CurrentMotionController { get; private set; }

    public AnimationEventManager CurrentAnimationEventManager { get; private set; }


    private Animator anim;

    private bool isMeleeAttack;


    private void Awake()
    {
        CurrentMotionController = new MotionController();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        CurrentAnimationEventManager = GetComponent<AnimationEventManager>();

        CurrentMotionController.Initialize(anim);
        CurrentMotionController.RegisterMotionSet("MeleeAttack", MeleeAttackMotionSet);
    }

    public void Enter()
    {
        isMeleeAttack = true;
        anim.SetBool("meleeAttack", isMeleeAttack);
        CurrentMotionController.UpdateAnimations("MeleeAttack");
    }

    public void Exit()
    {
        isMeleeAttack = false;
        anim.SetBool("meleeAttack", isMeleeAttack);
    }

    public void SetAnimValueByMouseDirection(Vector2 mouseDirection)
    {
        // 벡터의 내적을 통해 입력 값을 -1 ~ 1 범위로 변환(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        // 값을 애니메이터에 적용
        anim.SetFloat("mouseX", mouseX);
        anim.SetFloat("mouseY", mouseY);
    }


    public void Test()
    {
        Debug.Log("Weapon.Test: Called");
    }

    public void Test2()
    {
        Debug.Log("Weapon.Test2: Called");
    }
}
