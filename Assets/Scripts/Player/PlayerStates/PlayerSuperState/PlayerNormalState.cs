using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerState
{
    protected Vector2 input;

    private bool rollInput;

    public PlayerNormalState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        input = player.InputHandler.MovementInput;

        rollInput = player.InputHandler.RollInput;

        // 대쉬 상태 전환
        if (rollInput && player.RollState.CanRoll())
        {
            player.InputHandler.UseDashInput();
            stateMachine.ChangeState(player.RollState);
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
