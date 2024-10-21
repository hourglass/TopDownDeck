using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject SpriteObject;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float dashForce;

    [SerializeField]
    private float dashDrag;

    [SerializeField]
    private float dashDelay;

    [SerializeField]
    private float attackForce;

    [SerializeField]
    private float attackDrag;

    [SerializeField]
    private float attackDelay;


    private enum PlayerState
    {
        Idle,
        Move,
        Dash,
        Attack
    }

    private PlayerState currentState;

    private PlayerInput playerControls;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 mouseDirection;

    private Animator myAnimator;

    private int maxComboIndex = 1;
    private int currentComboIndex;


    private void Awake()
    {
        playerControls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = SpriteObject.GetComponent<Animator>();

        currentComboIndex = 0;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Movement.Dash.started += _ => DashStart();
        playerControls.Combat.Attack.started += _ => AttackStart();
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        if (!(currentState == PlayerState.Idle || currentState == PlayerState.Move))
        {
            return;
        }

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


    private void ConvertMousePosToBlend()
    {
        // 마우스와 플레이어의 스크린 좌표 가져오기
        Vector3 mouseScreenPoint = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // 벡터 정규화
        mouseDirection = mouseScreenPoint - playerScreenPoint;
        mouseDirection.Normalize();

        // 벡터의 값을 -1 ~ 1 범위로 변환
        float mouseX = Vector3.Dot(transform.right, mouseDirection);
        float mouseY = Vector3.Dot(transform.up, mouseDirection);

        myAnimator.SetFloat("MouseX", mouseX);
        myAnimator.SetFloat("MouseY", mouseY);
    }


    private void DashStart()
    {
        if (!(currentState == PlayerState.Idle || currentState == PlayerState.Move))
        {
            return;
        }

        ChangeState(PlayerState.Dash);
        ConvertMousePosToBlend();
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        rb.AddForce(mouseDirection * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashDelay);

        DashEnd();
    }

    private void DashEnd()
    {
        ChangeState(PlayerState.Idle);
    }



    private void AttackStart()
    {
        if (!(currentState == PlayerState.Idle || currentState == PlayerState.Move))
        {
            return;
        }

        ChangeState(PlayerState.Attack);
        ConvertMousePosToBlend();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        Attack();

        yield return new WaitForSeconds(attackDelay);

        AttackEnd();
    }

    private void Attack()
    {
        rb.AddForce(mouseDirection * attackForce, ForceMode2D.Impulse);

        myAnimator.SetInteger("ComboIndex", currentComboIndex);
        currentComboIndex++;
    }

    private void AttackEnd()
    {
        if (currentComboIndex > maxComboIndex)
        {
            currentComboIndex = 0;
        }

        ChangeState(PlayerState.Idle);
    }


    private void ChangeState(PlayerState state)
    {
        if (currentState == state)
        {
            return;
        }

        // 상태를 벗어났을 때
        switch (currentState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                rb.velocity = Vector2.zero;
                break;
            case PlayerState.Dash:
                rb.velocity = Vector2.zero;
                rb.drag = 0f;
                break;
            case PlayerState.Attack:
                rb.drag = 0f;
                break;
        }

        // 상태에 진입했을 때
        switch (state)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Move:
                break;
            case PlayerState.Dash:
                rb.drag = dashDrag;
                break;
            case PlayerState.Attack:
                rb.drag = attackDrag;
                break;
        }

        myAnimator.SetInteger("State", (int)state);
        currentState = state;
    }
}

