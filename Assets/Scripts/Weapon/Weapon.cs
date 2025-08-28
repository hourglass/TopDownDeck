using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public MotionSetData MeleeAttackMotionSet;

    public MotionController CurrentMotionController { get; private set; }

    public AnimationEventManager CurrentAnimationEventManager { get; private set; }


    private Player player;

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

        if (CurrentAnimationEventManager.subscriber == null)
        {
            Debug.LogError($"AnimationEventManager.subscriber가 설정되지 않았습니다.");
            return;
        }
        if (!CurrentAnimationEventManager.subscriber.TryGetComponent(out player))
        {
            Debug.LogError($"Player 컴포넌트가 {CurrentAnimationEventManager.subscriber.name}에 없습니다.");
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

    public void SetAnimValueByMouseDirection(Vector2 mouseDirection)
    {
        // 벡터의 내적을 통해 입력 값을 -1 ~ 1 범위로 변환(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        // 값을 애니메이터에 적용
        anim.SetFloat("mouseX", mouseX);
        anim.SetFloat("mouseY", mouseY);
    }

    public void AttackLauch()
    {
        Vector2 mouseDirection = player.GetMouseDirection();
        player.RB.drag = 100f;
        player.RB.AddForce(mouseDirection * 10f, ForceMode2D.Impulse);
    }

    public void ResetDrag()
    {
        player.RB.drag = 0f;
    }
}
