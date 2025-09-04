using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbillityDone;

    protected float exitTime;

    protected Vector2 mouseDirection;


    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        isAbillityDone = true;
    }

    public override void Enter()
    {
        base.Enter();
        isAbillityDone = false;

    }

    public override void Exit()
    {
        base.Exit();
        isAbillityDone = true;

        player.RB.drag = 0f;
    }
}
