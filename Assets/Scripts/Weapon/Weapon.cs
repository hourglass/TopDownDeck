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
        // 벡터의 내적을 통해 입력 값을 -1 ~ 1 범위로 변환(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        // 값을 애니메이터에 적용
        Anim.SetFloat("mouseX", mouseX);
        Anim.SetFloat("mouseY", mouseY);
        Anim.SetFloat("inputX", mouseX);
        Anim.SetFloat("inputY", mouseY);
    }
}
