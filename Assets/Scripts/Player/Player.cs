using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    public PlayerInputHandler InputHandler { get; private set; }

    public Animator Anim { get; private set; }
    public SpriteRenderer Renderer { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }

    [SerializeField]
    public PlayerData playerData;

    private Vector2 workspace;


    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    private float dashForce;

    [SerializeField]
    private float dashDrag;

    [SerializeField]
    private float dashTime;

    [SerializeField]
    private float dashDelay;

    [SerializeField]
    private float attackTime;

    [SerializeField]
    private float attackDelay;

    private enum State
    {
        Idle,
        Move,
        Dash,
        Attack
    }

    private State currentState;

    private PlayerInput playerControls;

    private bool dashEnabled;
    private bool attackEnabled;

    private Coroutine dashCorutine;
    private Coroutine attackCorutine;

    private Weapon currentWeapon;


    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");

        playerControls = new PlayerInput();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();
        RB = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<PlayerInputHandler>();

        StateMachine.Initailize(IdleState);

        //TODO: Refactoring
        playerControls.Gameplay.Dash.started += _ => Dash();
        playerControls.Gameplay.Attack.started += _ => Attack();

        dashEnabled = true;
        attackEnabled = true;

        SetCurrentWeapon();
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();

        //Move();
    }

    public void SetVelocity(Vector2 velocity)
    {
        workspace = velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    private Vector3 GetMouseDirection()
    {
        // 마우스와 플레이어의 스크린 좌표 가져오기
        Vector3 mouseScreenPoint = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // 벡터 정규화
        Vector3 mouseDirection = mouseScreenPoint - playerScreenPoint;
        mouseDirection.Normalize();

        return mouseDirection;
    }

    private void FlipByMouseDirection()
    {
        Vector3 mouseDirection = GetMouseDirection();

        // 벡터의 값을 -1 ~ 1 범위로 변환
        float mouseX = Vector3.Dot(transform.right, mouseDirection);
        float mouseY = Vector3.Dot(transform.up, mouseDirection);

        // 스프라이트 플립  
        Renderer.flipX = (mouseX < 0) ? true : false;

        // 값을 애니메이터에 적용
        Anim.SetFloat("MouseX", mouseX);
        Anim.SetFloat("MouseY", mouseY);

        Anim.SetFloat("InputX", mouseX);
        Anim.SetFloat("InputY", mouseY);
    }

    private void Dash()
    {
        if (!dashEnabled)
        {
            return;
        }

        if (currentState == State.Attack)
        {
            StopCoroutine(attackCorutine);
        }

        Vector3 mouseDirection = GetMouseDirection();
        FlipByMouseDirection();

        ChangeState(State.Dash);

        dashCorutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        Vector3 mouseDirection = GetMouseDirection();
        RB.AddForce(mouseDirection * dashForce, ForceMode2D.Impulse);
        RB.drag = dashDrag;

        yield return new WaitForSeconds(dashTime);

        RB.velocity = Vector3.zero;
        attackEnabled = true;
        Anim.SetInteger("State", (int)State.Idle);

        yield return new WaitForSeconds(dashDelay);

        ChangeState(State.Idle);
    }

    private void Attack()
    {
        if (!attackEnabled)
        {
            return;
        }

        if (currentState == State.Dash)
        {
            StopCoroutine(dashCorutine);
        }

        Vector3 mouseDirection = GetMouseDirection();
        FlipByMouseDirection();
        currentWeapon.SetMouseDirection(mouseDirection);

        ChangeState(State.Attack);

        attackCorutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        currentWeapon.Attack();

        float attackCancle = attackTime * 0.5f;
        yield return new WaitForSeconds(attackCancle);

        dashEnabled = true;

        yield return new WaitForSeconds(attackTime - attackCancle);

        RB.velocity = Vector3.zero;

        yield return new WaitForSeconds(attackDelay);

        ChangeState(State.Idle);
    }

    private void ChangeState(State state)
    {
        if (currentState == state)
        {
            return;
        }

        switch (state)
        {
            case State.Idle:
                dashEnabled = true;
                attackEnabled = true;
                RB.velocity = Vector3.zero;
                RB.drag = 0f;
                break;
            case State.Move:
                break;
            case State.Dash:
                dashEnabled = false;
                attackEnabled = false;
                break;
            case State.Attack:
                dashEnabled = false;
                attackEnabled = false;
                break;
        }

        Anim.SetInteger("State", (int)state);
        currentState = state;
    }

    private void SetCurrentWeapon()
    {
        currentWeapon = weapon;
        currentWeapon.SetAnim(Anim);
        currentWeapon.SetPlayerRb(RB);
    }
}

