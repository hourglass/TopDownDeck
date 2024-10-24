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

    [SerializeField]
    private float attackCancleDelay;


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

    private bool dashEnabled;
    private bool attackEnabled;

    private int maxComboIndex;
    private int currentComboIndex;

    private float defaultAttackDelay;


    private void Awake()
    {
        playerControls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = SpriteObject.GetComponent<Animator>();

        dashEnabled = true;
        attackEnabled = true;

        maxComboIndex = 1;
        currentComboIndex = 0;

        defaultAttackDelay = attackDelay;
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


    //===============Move===============//
    private void Move()
    {
        if (!(currentState == PlayerState.Idle || currentState == PlayerState.Move))
        {
            return;
        }

        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        if (movement.x == 0 && movement.y == 0)
        {
            ChangeState(PlayerState.Idle);
            return;
        }
        else
        {
            ChangeState(PlayerState.Move);
            rb.velocity = movement * moveSpeed * Time.deltaTime;

            myAnimator.SetFloat("InputX", movement.x);
            myAnimator.SetFloat("InputY", movement.y);
        }
    }


    //===============Mouse===============//
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

        // 값을 애니메이터에 적용
        myAnimator.SetFloat("MouseX", mouseX);
        myAnimator.SetFloat("MouseY", mouseY);

        myAnimator.SetFloat("InputX", mouseX);
        myAnimator.SetFloat("InputY", mouseY);
    }


    //===============Dash===============//
    private void DashStart()
    {
        if (!dashEnabled)
        {
            return;
        }

        if (currentState == PlayerState.Attack)
        {
            StopCoroutine(attackCoroutine);
        }

        ChangeState(PlayerState.Dash);
        ConvertMousePosToBlend();
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        Dash();

        yield return new WaitForSeconds(dashDelay);

        DashEnd();
    }

    private void Dash()
    {
        rb.AddForce(mouseDirection * dashForce, ForceMode2D.Impulse);
    }

    private void DashEnd()
    {
        ChangeState(PlayerState.Idle);
    }


    //===============Attack===============//
    private void AttackStart()
    {
        if (!attackEnabled)
        {
            return;
        }

        ChangeState(PlayerState.Attack);
        ConvertMousePosToBlend();
        attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    Coroutine attackCoroutine;
    private IEnumerator AttackCoroutine()
    {
        Attack();

        yield return new WaitForSeconds(attackCancleDelay);

        dashEnabled = true;

        yield return new WaitForSeconds(attackDelay - attackCancleDelay);

        AttackEnd();
    }

    private void Attack()
    {
        rb.AddForce(mouseDirection * attackForce, ForceMode2D.Impulse);

        myAnimator.SetFloat("AttackSpeed", defaultAttackDelay + (1f - attackDelay));
        myAnimator.SetInteger("ComboIndex", currentComboIndex);

        currentComboIndex++;
        if (currentComboIndex > maxComboIndex)
        {
            currentComboIndex = 0;
        }
    }

    private void AttackEnd()
    {
        if (currentState == PlayerState.Attack)
        {
            ChangeState(PlayerState.Idle);
        }
    }


    //===============State-===============//
    private void ChangeState(PlayerState state)
    {
        if (currentState == state)
        {
            return;
        }

        switch (state)
        {
            case PlayerState.Idle:
                rb.velocity = Vector2.zero;
                dashEnabled = true;
                attackEnabled = true;
                break;
            case PlayerState.Move:
                rb.drag = 0f;
                break;
            case PlayerState.Dash:
                rb.drag = dashDrag;
                dashEnabled = false;
                attackEnabled = false;
                break;
            case PlayerState.Attack:
                rb.drag = attackDrag;
                dashEnabled = false;
                attackEnabled = false;
                break;
        }

        myAnimator.SetInteger("State", (int)state);
        currentState = state;
    }
}

