using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        player.SetAnimValueByMouseDirection(mouseDirection);
        player.CheckIfShouldFlip(mouseDirection.x);

        player.RB.drag = playerData.dashDrag;
        player.RB.AddForce(mouseDirection * playerData.dashForce, ForceMode2D.Impulse);

        amountOfDashsLeft--;
    }

    public override void Exit()
    {
        base.Exit();

        lastDashTime = Time.time;
        player.RB.drag = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + playerData.dashTime)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool CanDash()
    {
        if (amountOfDashsLeft > 0)
        {
            return true;
        }
        else
        {
            if (Time.time >= lastDashTime + playerData.dashCoolDown)
            {
                amountOfDashsLeft = playerData.amountOfDashs;
                return true;
            }

            return false;
        }
    }
}
