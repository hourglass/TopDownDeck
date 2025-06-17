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
    public PlayerAttackState AttackState { get; private set; }


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
        AttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        Renderer = GetComponent<SpriteRenderer>();
        InputHandler = GetComponent<PlayerInputHandler>();

        cam = Camera.main;

        StateMachine.Initailize(IdleState);
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
        Vector3 mouseScreenPoint = UnityEngine.Input.mousePosition;
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
}

