using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        playerControls = new PlayerInput();
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = PlayerSprite.GetComponent<Animator>();
        playerSpriteRenderer = PlayerSprite.gameObject.GetComponent<SpriteRenderer>();

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
        playerControls.Combat.Attack.started += _ => Attack();

        SetCurrentWeapon();
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
            playerRb.velocity = movement * moveSpeed * Time.deltaTime;

            // 스프라이트 플립
            playerSpriteRenderer.flipX = (movement.x < 0f) ? true : false;

            // 값을 애니메이터에 적용
            playerAnim.SetFloat("InputX", movement.x);
            playerAnim.SetFloat("InputY", movement.y);
        }
    }

    //==============================[ Mouse ]==============================//
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

    private void FlipByMouseDirection(Vector3 mouseDirection)
    {
        // 벡터의 값을 -1 ~ 1 범위로 변환
        float mouseX = Vector3.Dot(transform.right, mouseDirection);
        float mouseY = Vector3.Dot(transform.up, mouseDirection);

        // 스프라이트 플립  
        playerSpriteRenderer.flipX = (mouseX < 0) ? true : false;

        // 값을 애니메이터에 적용
        playerAnim.SetFloat("MouseX", mouseX);
        playerAnim.SetFloat("MouseY", mouseY);

        playerAnim.SetFloat("InputX", mouseX);
        playerAnim.SetFloat("InputY", mouseY);
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

        Vector3 mouseDirection = GetMouseDirection();
        FlipByMouseDirection(mouseDirection);

        ChangeState(PlayerState.Dash);

        dashCorutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        Vector3 mouseDirection = GetMouseDirection();
        playerRb.AddForce(mouseDirection * dashForce, ForceMode2D.Impulse);
        playerRb.drag = dashDrag;

        yield return new WaitForSeconds(dashTime);

        playerRb.velocity = Vector3.zero;
        attackEnabled = true;
        playerAnim.SetInteger("State", (int)PlayerState.Idle);

        yield return new WaitForSeconds(dashDelay);

        ChangeState(PlayerState.Idle);
    }

    //==============================[ Attack ]==============================//
    private void Attack()
    {
        if (!attackEnabled)
        {
            return;
        }

        if (currentState == PlayerState.Dash)
        {
            StopCoroutine(dashCorutine);
        }

        Vector3 mouseDirection = GetMouseDirection();
        FlipByMouseDirection(mouseDirection);
        currentWeapon.SetMouseDirection(mouseDirection);

        ChangeState(PlayerState.Attack);

        attackCorutine = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        currentWeapon.Attack();

        float attackCancle = attackTime * 0.5f;
        yield return new WaitForSeconds(attackCancle);

        dashEnabled = true;

        yield return new WaitForSeconds(attackTime - attackCancle);

        playerRb.velocity = Vector3.zero;

        yield return new WaitForSeconds(attackDelay);

        ChangeState(PlayerState.Idle);
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
                dashEnabled = true;
                attackEnabled = true;
                playerRb.velocity = Vector3.zero;
                playerRb.drag = 0f;
                break;
            case PlayerState.Move:
                break;
            case PlayerState.Dash:
                dashEnabled = false;
                attackEnabled = false;
                break;
            case PlayerState.Attack:
                dashEnabled = false;
                attackEnabled = false;
                break;
        }

        playerAnim.SetInteger("State", (int)state);
        currentState = state;
    }

    private void SetCurrentWeapon()
    {
        currentWeapon = weapon;
        currentWeapon.SetPlayerAnim(playerAnim);
        currentWeapon.SetPlayerRb(playerRb);
    }


    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    private GameObject PlayerSprite;

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
    private Rigidbody2D playerRb;
    private Vector2 movement;

    private Animator playerAnim;
    private SpriteRenderer playerSpriteRenderer;

    private bool dashEnabled;
    private bool attackEnabled;

    private Coroutine dashCorutine;
    private Coroutine attackCorutine;

    private Weapon currentWeapon;
}

