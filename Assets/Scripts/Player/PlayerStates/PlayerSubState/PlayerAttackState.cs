using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;

    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, Weapon weapon) : base(player, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
    }

    public override void Enter()
    {
        base.Enter();
        weapon.Enter();

        player.PlayerMotionController.UpdateAnimations("Attack");
        weapon.WeaponMotionController.UpdateAnimations("VFX");

        mouseDirection = player.GetMouseDirection();
        player.CheckIfShouldFlip(mouseDirection);
        player.SetAnimValueByMouseDirection(mouseDirection);
        weapon.SetAnimValueByMouseDirection(mouseDirection);
    }

    public override void Exit()
    {
        base.Exit();
        weapon.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + 0.4f)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool CanAttack()
    {
        if (!isAbillityDone)
        {
            return false;
        }

        return true;
    }
}
