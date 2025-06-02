using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public abstract void Attack();

    public abstract void ChargingAttack();

    public void SetPlayerInfo(Animator newAnim, Rigidbody2D newRb)
    {
        playerAnim = newAnim;
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
