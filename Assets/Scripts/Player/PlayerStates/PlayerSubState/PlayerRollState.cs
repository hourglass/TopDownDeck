using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerRollState : PlayerAbilityState
{
    private int amountOfRollsLeft;

    private float lastRollTime;

    public PlayerRollState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        amountOfRollsLeft = playerData.amountOfRolls;
    }

    public override void Enter()
    {
        base.Enter();

        mouseDirection = player.GetMouseDirection();
        player.CheckIfShouldFlip(mouseDirection);
        player.SetAnimValueByMouseDirection(mouseDirection);

        player.RB.drag = playerData.rollDrag;
        player.RB.AddForce(mouseDirection * playerData.rollForce, ForceMode2D.Impulse);

        amountOfRollsLeft--;
    }

    public override void Exit()
    {
        base.Exit();

        lastRollTime = Time.time;
        player.RB.drag = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= startTime + playerData.rollTime)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public bool CanRoll()
    {
        if (!isAbillityDone)
        {
            return false;
        }

        if (amountOfRollsLeft > 0)
        {
            return true;
        }
        else
        {
            if (Time.time >= lastRollTime + playerData.rollCoolDown)
            {
                amountOfRollsLeft = playerData.amountOfRolls;
                return true;
            }

            return false;
        }
    }
}
