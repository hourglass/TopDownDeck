using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerDashState : PlayerAbilityState
{
    private int amountOfDashsLeft;

    private float lastDashTime;

    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        amountOfDashsLeft = playerData.amountOfDashs;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        mouseDirection = player.GetMouseDirection();

        player.CheckIfShouldFlip(mouseDirection.x);
        player.SetAnimValueByMouseDirection(mouseDirection);

        player.RB.drag = playerData.dashDrag;
        player.RB.AddForce(mouseDirection * playerData.dashForce, ForceMode2D.Impulse);

        amountOfDashsLeft--;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        if (Time.time > startTime + playerData.dashTime)
        {
            isAbilityDone = true;
            lastDashTime = Time.time;
            stateMachine.ChangeState(player.IdleState);
        }
        else 
        {
            //player.SetVelocity(mouseDirection * playerData.dashVelocity);
        }

        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool CanDash()
    {
        if (amountOfDashsLeft > 0)
        {
            if (Time.time > lastDashTime + playerData.singleDashCooldown)
            {
                return true;
            }

            return false;
        }
        else
        {
            if (Time.time > lastDashTime + playerData.dashCoolDown)
            {
                amountOfDashsLeft = playerData.amountOfDashs;
                return true;
            }

            return false;
        }
    }

    public void ResetAmountOfDashsLeft() => amountOfDashsLeft = playerData.amountOfDashs;

    public void DecreasetAmountOfDashsLeft() => amountOfDashsLeft--;
}
