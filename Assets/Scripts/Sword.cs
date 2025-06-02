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
        AttackLaunch();
        SetAnimByCombo();
    }

    public override void ChargingAttack()
    { 
        
    }

    private void AttackLaunch()
    {
        playerRb.AddForce(mouseDirection * attackForce, ForceMode2D.Impulse);
        playerRb.drag = attackDrag;
    }

    private void SetAnimByCombo()
    {
        playerAnim.SetInteger("ComboIndex", currentComboIndex);

        currentComboIndex++;
        if (currentComboIndex > maxComboIndex)
        {
            currentComboIndex = 0;
        }
    }

    [SerializeField]
    private float attackForce;

    [SerializeField]
    private float attackDrag;

    private int maxComboIndex;
    private int currentComboIndex;
}
