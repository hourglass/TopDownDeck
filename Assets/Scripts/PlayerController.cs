using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject SpriteObject;

    [SerializeField]
    private float moveSpeed;


    private enum PlayerState
    {
        Idle,
        Run
    }

    private PlayerState currentState;

    private PlayerInput playerControls;
    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator playerAnimator;


    private void Awake()
    {
        playerControls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = SpriteObject.GetComponent<Animator>();
    }


    private void OnEnable()
    {
        playerControls.Enable();
    }


    private void FixedUpdate()
    {
        GetInput();
        Move();
        ChangeMoveAnimation();
    }


    private void GetInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        // 마우스와 플레이어의 스크린 좌표 가져오기
        //Vector3 mouseScreenPoint = Input.mousePosition;
        //Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
    }


    private void Move()
    {
        rb.velocity = movement * moveSpeed * Time.deltaTime;

        if (movement.x != 0 || movement.y != 0)
        {
            ChangeState(PlayerState.Run);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }


    private void ChangeMoveAnimation()
    {
        playerAnimator.SetFloat("InputX", movement.x);
        playerAnimator.SetFloat("InputY", movement.y);
    }


    private void ChangeState(PlayerState state)
    {
        if (currentState == state)
        {
            return;
        }

        playerAnimator.SetInteger("State", (int)state);
        currentState = state;
    }
}

