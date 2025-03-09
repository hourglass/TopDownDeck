using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject spriteObject;

    [SerializeField]
    private float moveSpeed;


    [SerializeField]
    private float dashForce;

    [SerializeField]
    private float dashDrag;

    [SerializeField]
    private float dashTime;

    [SerializeField]
    private float dashDelay;


    [SerializeField]
    private float attackForce;

    [SerializeField]
    private float attackDrag;

    [SerializeField]
    private float attackTime;

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
    private SpriteRenderer mySpriteRenderer;

    Coroutine dashCorutine;
    private bool dashEnabled;

    Coroutine attackCorutine;
    private bool attackEnabled;
    private int maxComboIndex;
    private int currentComboIndex;


    private void Awake()
    {
        playerControls = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = spriteObject.GetComponent<Animator>();
        mySpriteRenderer = spriteObject.gameObject.GetComponent<SpriteRenderer>();

        maxComboIndex = 1;
        currentComboIndex = 0;

        dashEnabled = true;
        attackEnabled = true;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Movement.Dash.started += _ => Dash();
        playerControls.Combat.Attack.started += _ => AttackStart();
    }

    private void FixedUpdate()
    {
        Move();
    }


    //===============================[ Move ]==============================//
    private void Move()
    {
        if (!(currentState == PlayerState.Idle || currentState == PlayerState.Move))
        {
            return;
        }

        // 키보드 입력 값 받아오기
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        // 정지 및 이동
        if (movement.x == 0 && movement.y == 0)
        {
            ChangeState(PlayerState.Idle);
            return;
        }
        else
        {
            ChangeState(PlayerState.Move);
            rb.velocity = movement * moveSpeed * Time.deltaTime;

            // 스프라이트 플립
            mySpriteRenderer.flipX = (movement.x < 0f) ? true : false;

            // 값을 애니메이터에 적용
            myAnimator.SetFloat("InputX", movement.x);
            myAnimator.SetFloat("InputY", movement.y);
        }
    }

    //==============================[ Mouse ]==============================//
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

        // 스프라이트 플립  
        mySpriteRenderer.flipX = (mouseX < 0) ? true : false;

        // 값을 애니메이터에 적용
        myAnimator.SetFloat("MouseX", mouseX);
        myAnimator.SetFloat("MouseY", mouseY);

        myAnimator.SetFloat("InputX", mouseX);
        myAnimator.SetFloat("InputY", mouseY);
    }

    //==============================[ Dash ]==============================//
    private void Dash()
    {
        if (!dashEnabled)
        {
            return;
        }

        if (currentState == PlayerState.Attack)
        {
            StopCoroutine(attackCorutine);
        }

        ChangeState(PlayerState.Dash);
        ConvertMousePosToBlend();
        dashCorutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        rb.AddForce(mouseDirection * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(dashTime);

        rb.velocity = Vector3.zero;
        attackEnabled = true;
        myAnimator.SetInteger("State", (int)PlayerState.Idle);

        yield return new WaitForSeconds(dashDelay);

        ChangeState(PlayerState.Idle);
    }

    //==============================[ Attack ]==============================//
    private void AttackStart()
    {
        if (!attackEnabled)
        {
            return;
        }

        if (currentState == PlayerState.Dash)
        {
            StopCoroutine(dashCorutine);
        }

        ChangeState(PlayerState.Attack);
        ConvertMousePosToBlend();
        attackCorutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        Attack();

        float attackCancle = attackTime * 0.5f;
        yield return new WaitForSeconds(attackCancle);

        dashEnabled = true;

        yield return new WaitForSeconds(attackTime - attackCancle);

        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(attackDelay);

        ChangeState(PlayerState.Idle);
    }

    private void Attack()
    {
        rb.AddForce(mouseDirection * attackForce, ForceMode2D.Impulse);

        myAnimator.SetInteger("ComboIndex", currentComboIndex);

        currentComboIndex++;
        if (currentComboIndex > maxComboIndex)
        {
            currentComboIndex = 0;
        }
    }

    //==============================[ State ]==============================//
    private void ChangeState(PlayerState state)
    {
        if (currentState == state)
        {
            return;
        }

        switch (state)
        {
            case PlayerState.Idle:
                rb.velocity = Vector3.zero;
                rb.drag = 0f;
                dashEnabled = true;
                attackEnabled = true;
                break;
            case PlayerState.Move:
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

