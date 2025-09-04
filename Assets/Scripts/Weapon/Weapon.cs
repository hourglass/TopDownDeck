using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public MotionSetData MeleeAttackMotionSet;

    public MotionController CurrentMotionController { get; private set; }

    public AnimationEventController CurrentAnimationEventController { get; private set; }

    public Vector2 CurrentDirection { get; set; }


    private Rigidbody2D subscriberRB;

    private Animator anim;

    private bool isMeleeAttack;


    private void Awake()
    {
        CurrentMotionController = new MotionController();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        CurrentAnimationEventController = GetComponent<AnimationEventController>();

        CurrentMotionController.Initialize(anim);
        CurrentMotionController.RegisterMotionSet("MeleeAttack", MeleeAttackMotionSet);

        if (CurrentAnimationEventController.subscriber == null)
        {
            Debug.LogError($"AnimationEventController.subscriber가 설정되지 않았습니다.");
            return;
        }
        if (!CurrentAnimationEventController.subscriber.TryGetComponent(out subscriberRB))
        {
            Debug.LogError($"컴포넌트가 {CurrentAnimationEventController.subscriber.name}에 없습니다.");
        }
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

    public void SetAnimValueByDirection()
    {
        // 벡터의 내적을 통해 입력 값을 -1 ~ 1 범위로 변환(cos t)
        float dirX = Vector2.Dot(Vector2.right, CurrentDirection);
        float dirY = Vector2.Dot(Vector2.up, CurrentDirection);

        // 값을 애니메이터에 적용
        anim.SetFloat("mouseX", dirX);
        anim.SetFloat("mouseY", dirY);
    }

    public void AttackLauch()
    {
        subscriberRB.drag = 100f;
        subscriberRB.AddForce(CurrentDirection * 20f, ForceMode2D.Impulse);
    }
}
