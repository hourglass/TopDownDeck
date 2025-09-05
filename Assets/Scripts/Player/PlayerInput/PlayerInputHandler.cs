using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }

    public bool RollInput { get; private set; }

    public bool AttackInput { get; private set; }

    public bool ChargingInput { get; private set; }


    private float inputResetTime = 0.2f;

    private float rollStartTime;

    private float attackStartTime;

    private float chargingEndTime;


    private void Update()
    {
        CheckRollInputResetTime();
        CheckAttackInputResetTime();
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


    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
            attackStartTime = Time.time;
        }

        if (context.performed)
        {
            ChargingInput = true;
        }

        if (context.canceled)
        {
            if (ChargingInput)
            {
                ChargingInput = false;
                chargingEndTime = Time.time;
            }
        }
    }

    public void UseAttackInput() => AttackInput = false;

    public float GetChargingEndTime() => chargingEndTime;


    private void CheckRollInputResetTime()
    {
        if (Time.time >= rollStartTime + inputResetTime)
        {
            RollInput = false;
        }
    }

    private void CheckAttackInputResetTime()
    {
        if (Time.time >= attackStartTime + inputResetTime)
        {
            AttackInput = false;
        }
    }
}
