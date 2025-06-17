using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int numberOfAttacks;

    public int CurrentAttackCounter
    {
        get => currentAttackCounter;
        private set => currentAttackCounter = value > numberOfAttacks ? 0: value;
    }


    private Animator anim;

    private GameObject baseGameObject;

    private int currentAttackCounter;


    private void Awake()
    {
        baseGameObject = transform.Find("Base").gameObject;

        anim = baseGameObject.GetComponent<Animator>();

        currentAttackCounter = 0;
    }

    public void Enter()
    {
        anim.SetBool("active", true);
        anim.SetInteger("counter", CurrentAttackCounter);
    }

    public void Exit()
    {
        anim.SetBool("active", false);

        CurrentAttackCounter++;
    }

    public void SetAnimValueByMouseDirection(Vector2 mouseDirection)
    {
        // ������ ������ ���� �Է� ���� -1 ~ 1 ������ ��ȯ(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        anim.SetFloat("mouseX", mouseX);
        anim.SetFloat("mouseY", mouseY);
    }
}
