using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRollState RollState { get; private set; }

    public PlayerInputHandler InputHandler { get; private set; }

    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }

    [SerializeField]
    public PlayerData playerData;

    private Vector2 workspace;

    private SpriteRenderer Renderer;

    private Camera cam;


    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        RollState = new PlayerRollState(this, StateMachine, playerData, "roll");

        playerControls = new PlayerInput();

        currentState = State.Idle;
        dashEnabled = true;
        attackEnabled = true;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        InputHandler = GetComponent<PlayerInputHandler>();

        cam = Camera.main;

        StateMachine.Initailize(IdleState);

        //TODO: Refactoring
        //playerControls.Gameplay.Dash.started += _ => Dash();
        playerControls.Gameplay.Attack.started += _ => Attack();

        SetCurrentWeapon();
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        StateMachine.CurrentState.LogicUpdate();

        //Move();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void SetVelocity(Vector2 velocity)
    {
        workspace = velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void CheckIfShouldFlip(float xInput)
    {
        Renderer.flipX = (xInput < 0f) ? true : false;
    }

    public Vector3 GetMouseDirection()
    {
        // 마우스와 플레이어의 스크린 좌표 가져오기
        Vector3 mouseScreenPoint = UnityEngine.Input.mousePosition;
        Vector3 playerScreenPoint = cam.WorldToScreenPoint(transform.position);

        // 벡터 정규화
        Vector2 mouseDirection = (mouseScreenPoint - playerScreenPoint).normalized;

        return mouseDirection;
    }

    public void SetAnimValueByMouseDirection(Vector2 mouseDirection)
    {
        // 벡터의 내적을 통해 입력 값을 -1 ~ 1 범위로 변환(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        // 값을 애니메이터에 적용
        Anim.SetFloat("mouseX", mouseX);
        Anim.SetFloat("mouseY", mouseY);

        Anim.SetFloat("inputX", mouseX);
        Anim.SetFloat("inputY", mouseY);
    }



    private void Move()
    {
        if (!(currentState == State.Idle || currentState == State.Move))
        {
            return;
        }

        Vector2 input = playerControls.Gameplay.Move.ReadValue<Vector2>();

        // 정지 상태 전환
        if (input.x == 0 && input.y == 0)
        {
            ChangeState(State.Idle);
            return;
        }
        else
        {
            ChangeState(State.Move);

            // 값을 애니메이터에 적용
            Anim.SetFloat("inputX", input.x);
            Anim.SetFloat("inputY", input.y);

            // 스프라이트 플립
            CheckIfShouldFlip(input.x);

            // 플레이어 이동
            SetVelocity(input * playerData.movementVelocity);
        }
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

        ChangeState(State.Dash);

        Vector3 mouseDirection = GetMouseDirection();
        CheckIfShouldFlip(mouseDirection.x);
        SetAnimValueByMouseDirection(mouseDirection);

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
        Anim.SetInteger("state", (int)State.Idle);

        yield return new WaitForSeconds(dashDelay);

        ChangeState(State.Idle);
    }

    private void Attack()
    {
        if (!attackEnabled)
        {
            return;
        }

        ChangeState(State.Attack);

        Vector3 mouseDirection = GetMouseDirection();
        CheckIfShouldFlip(mouseDirection.x);
        SetAnimValueByMouseDirection(mouseDirection);
        currentWeapon.SetMouseDirection(mouseDirection);

        attackCorutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        currentWeapon.Attack();

        float attackCancle = attackTime * 0.5f;
        yield return new WaitForSeconds(attackCancle);

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

        Anim.SetInteger("state", (int)state);
        currentState = state;
    }

    private void SetCurrentWeapon()
    {
        currentWeapon = weapon;
        currentWeapon.SetAnim(Anim);
        currentWeapon.SetPlayerRb(RB);
    }


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

    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    public float dashForce = 12f;

    [SerializeField]
    public float dashDrag = 5f;

    [SerializeField]
    public float dashTime = 0.3f;

    [SerializeField]
    public float dashDelay = 0.03f;

    [SerializeField]
    private float attackTime = 0.4f;

    [SerializeField]
    private float attackDelay = 0.2f;
}

