using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerNormalState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
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

        // ���� ���� ��ȯ
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        // �÷��̾� �̵�
        player.SetVelocity(moveInput * playerData.movementVelocity);

        // ���� �ִϸ����Ϳ� ����
        player.Anim.SetFloat("inputX", moveInput.x);
        player.Anim.SetFloat("inputY", moveInput.y);

        // ��������Ʈ �ø�
        player.CheckIfShouldFlip(moveInput);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
