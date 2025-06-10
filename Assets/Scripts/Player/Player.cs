using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDashState DashState { get; private set; }

    public PlayerInputHandler InputHandler { get; private set; }

    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }

    [SerializeField]
    public PlayerData playerData;

    private Vector2 workspace;

    private SpriteRenderer Renderer;

    private Camera cam;


    [SerializeField]
    private Weapon weapon;

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
        DashState = new PlayerDashState(this, StateMachine, playerData, "dash");

        playerControls = new PlayerInput();
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
        // ���콺�� �÷��̾��� ��ũ�� ��ǥ ��������
        Vector3 mouseScreenPoint = Input.mousePosition;
        Vector3 playerScreenPoint = cam.WorldToScreenPoint(transform.position);

        // ���� ����ȭ
        Vector2 mouseDirection = (mouseScreenPoint - playerScreenPoint).normalized;

        return mouseDirection;
    }

    public void SetAnimValueByMouseDirection(Vector2 mouseDirection)
    {
        // ������ ������ ���� �Է� ���� -1 ~ 1 ������ ��ȯ(cos t)
        float mouseX = Vector2.Dot(Vector2.right, mouseDirection);
        float mouseY = Vector2.Dot(Vector2.up, mouseDirection);

        // ���� �ִϸ����Ϳ� ����
        Anim.SetFloat("mouseX", mouseX);
        Anim.SetFloat("mouseY", mouseY);

        Anim.SetFloat("inputX", mouseX);
        Anim.SetFloat("inputY", mouseY);
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
        CheckIfShouldFlip(mouseDirection.x);
        SetAnimValueByMouseDirection(mouseDirection);

        ChangeState(State.Dash);

        dashCorutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        Vector3 mouseDirection = GetMouseDirection();
        RB.AddForce(mouseDirection * playerData.dashForce, ForceMode2D.Impulse);
        RB.drag = playerData.dashDrag;

        yield return new WaitForSeconds(playerData.dashTime);

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

        Vector3 mouseDirection = GetMouseDirection();
        CheckIfShouldFlip(mouseDirection.x);
        SetAnimValueByMouseDirection(mouseDirection);
        currentWeapon.SetMouseDirection(mouseDirection);

        ChangeState(State.Attack);

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
}

