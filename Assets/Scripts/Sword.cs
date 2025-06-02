using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    Sword()
    {
        maxComboIndex = 1;
        currentComboIndex = 0;
    }

    public override void Attack()
    {
        playerAnim.SetInteger("ComboIndex", currentComboIndex);

        playerRb.AddForce(mouseDirection * attackForce, ForceMode2D.Impulse);
        playerRb.drag = attackDrag;

        currentComboIndex++;
        if (currentComboIndex > maxComboIndex)
        {
            currentComboIndex = 0;
        }
    }

    public override void ChargingAttack()
    { 
        
    }

    [SerializeField]
    private float attackForce;

    [SerializeField]
    private float attackDrag;

    private int maxComboIndex;
    private int currentComboIndex;
}
