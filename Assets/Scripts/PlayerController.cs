using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject SpriteObject;

    [SerializeField]
    private float moveSpeed;

    [SerializeField] 
    private float AttackForce;

    [SerializeField]
    private float AttackDrag;

    [SerializeField]
    private float AttackDelay;


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
        playerControls.Combat.Attack.started += _ => AttackStart();
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


    private void ConvertMousePosToBlend()
    {
        // ���콺�� �÷��̾��� ��ũ�� ��ǥ ��������
        Vector3 mouseScreenPoint = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // ���� ����ȭ
        mouseDirection = mouseScreenPoint - playerScreenPoint;
        mouseDirection.Normalize();

        // ������ ���� -1 ~ 1 ������ ��ȯ
        float mouseX = Vector3.Dot(transform.right, mouseDirection);
        float mouseY = Vector3.Dot(transform.up, mouseDirection);

        myAnimator.SetFloat("MouseX", mouseX);
        myAnimator.SetFloat("MouseY", mouseY);
    }


    private void AttackStart()
    {
        if (currentState == PlayerState.Attack)
        {
            return;
        }

        Attack();
        ChangeState(PlayerState.Attack);
        Invoke("AttackEnd", AttackDelay);
    }



    private void Attack()
    {
        ConvertMousePosToBlend();
        
        rb.velocity = Vector2.zero;
        rb.drag = AttackDrag;
        rb.AddForce(mouseDirection * AttackForce, ForceMode2D.Impulse);

        myAnimator.SetInteger("ComboIndex", currentComboIndex);
        currentComboIndex++;
    }


    private void AttackEnd()
    {
        if (currentComboIndex > maxComboIndex)
        {
            currentComboIndex = 0;
        }

        switch (currentState)
        {
            case PlayerState.Attack: 
                ChangeState(PlayerState.Idle);
                rb.drag = 0f;
                break;
            default: break;
        }
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

