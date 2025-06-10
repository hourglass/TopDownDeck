using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerState
{
    protected Vector2 input;

    private bool DashInput;

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

        // 입력 값 받아오기
        input = player.InputHandler.MovementInput;

        DashInput = player.InputHandler.DashInput;

        // 대쉬 상태 전환
        if (DashInput && player.DashState.CanDash())
        {
            player.InputHandler.UseDashInput();
            //stateMachine.ChangeState(player.DashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
