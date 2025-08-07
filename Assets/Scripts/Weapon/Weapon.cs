using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public MotionSetData VFXMotionSet;

    public MotionController WeaponMotionController { get; private set; }

    public Animator Anim { get; private set; }

    private bool isMeleeAttack;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        WeaponMotionController = new MotionController(Anim);
        WeaponMotionController.Initialize("VFX", VFXMotionSet);
    }

    public void Enter()
    {
        isMeleeAttack = true;
        Anim.SetBool("meleeAttack", isMeleeAttack);
    }

    public void Exit()
    {
        isMeleeAttack = false;
        Anim.SetBool("meleeAttack", isMeleeAttack);
    }

    public void SetAnimValueByMouseDirection(Vector2 mouseDirection)
    {
        // ������ ������ ���� �Է� ���� -1 ~ 1 ������ ��ȯ(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        // ���� �ִϸ����Ϳ� ����
        Anim.SetFloat("mouseX", mouseX);
        Anim.SetFloat("mouseY", mouseY);
        Anim.SetFloat("inputX", mouseX);
        Anim.SetFloat("inputY", mouseY);
    }
}
