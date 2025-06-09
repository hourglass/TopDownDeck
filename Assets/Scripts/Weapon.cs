using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Attack();

    public abstract void ChargingAttack();

    public void SetAnim(Animator newAnim) => Anim = newAnim;

    public void SetPlayerRb(Rigidbody2D newRb) => playerRb = newRb;

    public void SetMouseDirection(Vector3 newDirection) => mouseDirection = newDirection;

    private void Awake()
    {
       
    }

    protected Animator Anim;
    protected Rigidbody2D playerRb;

    protected Vector3 mouseDirection;

    private WeaponAnimationHandler eventHandler;
}
