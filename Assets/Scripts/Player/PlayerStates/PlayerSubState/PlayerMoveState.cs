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

        // Ű���� �Է� �� �޾ƿ���
        input = player.InputHandler.MovementInput;

        // ���� ���� ��ȯ
        if (input.x == 0 && input.y == 0)
        {
            stateMachine.ChangeState(player.IdleState);
            return;
        }

        // �÷��̾� �̵�
        player.SetVelocity(input * playerData.movementVelocity);

        // ��������Ʈ �ø�
        player.CheckIfShouldFlip(input.x);

        // ���� �ִϸ����Ϳ� ����
        player.Anim.SetFloat("inputX", input.x);
        player.Anim.SetFloat("inputY", input.y);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
