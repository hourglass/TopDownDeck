using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
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

        // 키보드 입력 값 받아오기
        input = player.InputHandler.MovementInput;

        // 정지 상태 전환
        if (input.x == 0 && input.y == 0)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        // 플레이어 이동
        player.SetVelocity(input * playerData.movementVelocity);

        // 스프라이트 플립
        player.Renderer.flipX = (input.x < 0f) ? true : false;

        // 값을 애니메이터에 적용
        player.Anim.SetFloat("InputX", input.x);
        player.Anim.SetFloat("InputY", input.y);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
