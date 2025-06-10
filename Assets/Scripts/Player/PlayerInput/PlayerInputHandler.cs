using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }

    public bool DashInput { get; private set; }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 RawMovementInput = context.ReadValue<Vector2>();

        float NormInputX = (RawMovementInput.x * Vector2.right).normalized.x;
        float NormInputY = (RawMovementInput.y * Vector2.up).normalized.y;

        MovementInput = new Vector2(NormInputX, NormInputY).normalized;
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        { 
            DashInput = true;
        }
    }

    public void UseDashInput() => DashInput = false;

    public void OnAttackInput(InputAction.CallbackContext context)
    {

    }
}
