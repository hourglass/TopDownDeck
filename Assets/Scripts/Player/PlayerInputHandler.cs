using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MovementInput { get; private set; }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }

    public void OnDashInput(InputAction.CallbackContext context) 
    { 
        
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
       
    }
} 
