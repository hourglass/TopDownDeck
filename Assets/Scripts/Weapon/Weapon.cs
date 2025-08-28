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
            Debug.LogError($"AnimationEventManager.subscriber�� �������� �ʾҽ��ϴ�.");
            return;
        }
        if (!CurrentAnimationEventManager.subscriber.TryGetComponent(out player))
        {
            Debug.LogError($"Player ������Ʈ�� {CurrentAnimationEventManager.subscriber.name}�� �����ϴ�.");
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
        // ������ ������ ���� �Է� ���� -1 ~ 1 ������ ��ȯ(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        // ���� �ִϸ����Ϳ� ����
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
