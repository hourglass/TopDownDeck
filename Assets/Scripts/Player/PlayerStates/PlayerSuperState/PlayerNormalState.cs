using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalState : PlayerState
{
    protected Vector2 moveInput;

    private bool rollInput;

    private bool attackInput;


    public PlayerNormalState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

        moveInput = player.InputHandler.MovementInput;

        attackInput = player.InputHandler.AttackInput;
        if (attackInput && player.AttackState.CanAttack())
        {
            // 공격 상태 전환
            stateMachine.ChangeState(player.AttackState);
            return;
        }

        rollInput = player.InputHandler.RollInput;
        if (rollInput && player.RollState.CanRoll())
        {
            // 대쉬 상태 전환
            stateMachine.ChangeState(player.RollState);
            player.InputHandler.UseRollInput();
            return;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
