using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Attack();

    public abstract void ChargingAttack();

    public void SetPlayerAnim(Animator newAnim)
    {
        playerAnim = newAnim;
    }

    public void SetPlayerRb(Rigidbody2D newRb)
    {
        playerRb = newRb;
    }

    public void SetMouseDirection(Vector3 newDirection)
    {
        mouseDirection = newDirection;
    }

    protected Animator playerAnim;
    protected Rigidbody2D playerRb;

    protected Vector3 mouseDirection;
}
