using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public MotionSetData MeleeAttackMotionSet;

    public MotionController CurrentMotionController { get; private set; }


    private Animator anim;

    private bool isMeleeAttack;


    private void Awake()
    {
        CurrentMotionController = new MotionController();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();

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
        // ������ ������ ���� �Է� ���� -1 ~ 1 ������ ��ȯ(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        // ���� �ִϸ����Ϳ� ����
        anim.SetFloat("mouseX", mouseX);
        anim.SetFloat("mouseY", mouseY);
    }
}
