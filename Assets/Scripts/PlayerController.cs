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
        Move,
        Attack
    }

    private PlayerState currentState;

    private PlayerInput playerControls;
    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;


    private void Awake()
    {
        playerControls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = SpriteObject.GetComponent<Animator>();
        mySpriteRenderer = SpriteObject.GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => Attack();
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        if (currentState == PlayerState.Attack) { return; }

        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        rb.velocity = movement * moveSpeed * Time.deltaTime;

        myAnimator.SetFloat("InputX", movement.x);
        myAnimator.SetFloat("InputY", movement.y);

        if (movement.x != 0 || movement.y != 0)
        {
            ChangeState(PlayerState.Move);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }


    private void Attack()
    {
        if (currentState == PlayerState.Attack) { return; }
        
        // 마우스와 플레이어의 스크린 좌표 가져오기
        Vector3 mouseScreenPoint = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // 벡터 정규화
        Vector3 mouseDirection = mouseScreenPoint - playerScreenPoint;
        mouseDirection.Normalize();

        // 벡터의 값을 -1 ~ 1 범위로 변환
        float mouseX = Vector3.Dot(transform.right, mouseDirection);
        float mouseY = Vector3.Dot(transform.up, mouseDirection);

        myAnimator.SetFloat("MouseX", mouseX);
        myAnimator.SetFloat("MouseY", mouseY);

        rb.velocity = Vector2.zero;

        ChangeState(PlayerState.Attack);

        Invoke("ReturnIdle", 0.5f);
    }


    private void ReturnIdle()
    {
        ChangeState(PlayerState.Idle);
    }


    private void ChangeState(PlayerState state)
    {
        if (currentState == state)
        {
            return;
        }

        myAnimator.SetInteger("State", (int)state);
        currentState = state;
    }
}

