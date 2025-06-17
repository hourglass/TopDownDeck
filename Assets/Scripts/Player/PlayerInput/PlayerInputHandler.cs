using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }

    public bool RollInput { get; private set; }

    public bool AttackInput { get; private set; }


    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float rollStartTime;

    private float attackStartTime;

    private void Update()
    {
        CheckRollInputHoldTime();
        CheckAttackInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 RawMovementInput = context.ReadValue<Vector2>();

        float NormInputX = (RawMovementInput.x * Vector2.right).normalized.x;
        float NormInputY = (RawMovementInput.y * Vector2.up).normalized.y;

        MovementInput = new Vector2(NormInputX, NormInputY).normalized;
    }

    public void OnRollInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RollInput = true;
            rollStartTime = Time.time;
        }
    }

    public void UseRollInput() => RollInput = false;

    private void CheckRollInputHoldTime()
    {
        if (Time.time >= rollStartTime + inputHoldTime)
        {
            RollInput = false;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
            attackStartTime = Time.time;
        }
    }

    public void UseAttackInput() => RollInput = false;

    private void CheckAttackInputHoldTime()
    {
        if (Time.time >= attackStartTime + inputHoldTime)
        {
            AttackInput = false;
        }
    }
}
