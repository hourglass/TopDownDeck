using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private Weapon weapon;

    public PlayerData playerData;
    public MotionSetData attackMotionSet;
    public MotionSetData chargeMotionSet;
    public MotionSetData skillMotionSet;

    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerRollState RollState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }

    public PlayerInputHandler InputHandler { get; private set; }
    public MotionController CurrentMotionController { get; private set; }
    public AnimationEventReceiver CurrentAnimationEventReceiver { get; private set; }

    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }


    private Camera cam;

    private float facingDirection;


    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        RollState = new PlayerRollState(this, StateMachine, playerData, "roll");
        AttackState = new PlayerAttackState(this, StateMachine, playerData, "attack", weapon);

        CurrentMotionController = new MotionController();
        CurrentAnimationEventReceiver = new AnimationEventReceiver();
    }

    private void Start()
    {
        InputHandler = GetComponent<PlayerInputHandler>();
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();

        CurrentMotionController.Initialize(Anim);
        CurrentMotionController.RegisterMotionSet("Attack", attackMotionSet);

        CurrentAnimationEventReceiver.Initialize(Anim);
        if (weapon != null)
        {
            if (weapon.TryGetComponent(out AnimationEventManager manager))
            {
                manager.subscriber = gameObject;
                CurrentAnimationEventReceiver.RegisterEventEntries("Attack", manager.eventEntries);
            }
        }

        StateMachine.Initialize(IdleState);

        cam = Camera.main;
        facingDirection = 1f;
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
        RB.velocity = velocity;
        CurrentVelocity = velocity;
    }

    public void CheckIfShouldFlip(Vector2 input)
    {
        float xAbs = Mathf.Abs(input.x);
        float yAbs = Mathf.Abs(input.y);

        if (input.x != 0f && input.x * facingDirection < 0f && (xAbs > 0.3f || yAbs < 0.866f))
        {
            facingDirection *= -1f;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public Vector2 GetMouseDirection()
    {
        // 마우스와 플레이어의 스크린 좌표 가져오기
        Vector3 mouseScreenPoint = Input.mousePosition;
        Vector3 playerScreenPoint = cam.WorldToScreenPoint(transform.position);

        // 벡터 계산
        Vector2 mouseDirection = (mouseScreenPoint - playerScreenPoint);

        // 길이 확인 및 보정
        float sqrMag = mouseDirection.sqrMagnitude;
        if (sqrMag > 0.01f)
        {
            // 임계값 초과: 정상 정규화
            mouseDirection.Normalize();
        }
        else
        {
            if (sqrMag > 0f)
            {
                // 임계값 이하: 길이를 1로 보정
                mouseDirection *= (1f / Mathf.Sqrt(sqrMag));
            }
            else
            {
                // 완전 0 벡터 시 Vector2.up 사용
                mouseDirection = Vector2.up;
            }
        }

        return mouseDirection;
    }

    public void SetAnimValueByMoveInput(Vector2 MoveInput)
    {
        // 값을 애니메이터에 적용
        Anim.SetFloat("inputX", MoveInput.x);
        Anim.SetFloat("inputY", MoveInput.y);
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
}

