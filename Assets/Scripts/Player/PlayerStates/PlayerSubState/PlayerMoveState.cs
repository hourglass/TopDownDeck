using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerNormalState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        if (player.StateMachine.CurrentState != player.MoveState)
        {
            return;
        }

        // 정지 상태 전환
        if (input.x == 0 && input.y == 0)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        // 플레이어 이동
        player.SetVelocity(input * playerData.movementVelocity);

        // 값을 애니메이터에 적용
        player.Anim.SetFloat("inputX", input.x);
        player.Anim.SetFloat("inputY", input.y);

        // 스프라이트 플립
        player.CheckIfShouldFlip(input);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
