using UnityEngine;

public partial class PlayerController : BaseController, IController
{
    public MovementData movementData;
    public WallRunningData wallRunningData;
    public BreakFallData breakFallData;
    public ClimpUpData climbUpData;
    public SlidingData slidingData;

    private Rigidbody _rigidBody;
    private PlayerInputActions _playerInputActions;
    public Vector3 _directionInput;
    private Vector2 _lookAroundDelta;

    public PlayerMovementComponent playerMovementComponent;
    private WallRunningComponent _wallRunningComponent;
    private BreakFallComponent _breakFallComponent;
    private SlidingComponent _slidingComponent;
    private ClimbUpComponent _climbUpComponent;

    [HideInInspector] public PlayerState playerState;

    protected override void Awake()
    {
        base.Awake();

        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.SetCallbacks(new PlayerInputActionContext(this));
        _playerInputActions.Enable();

        _rigidBody = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerState = PlayerState.Normal;
    }

    private void Start()
    {
        playerMovementComponent = new PlayerMovementComponent(this, movementData);
        _wallRunningComponent = new WallRunningComponent(this, wallRunningData);
        _breakFallComponent = new BreakFallComponent(this, breakFallData);
        _climbUpComponent = new ClimbUpComponent(this, climbUpData, movementData);

        _slidingComponent = new SlidingComponent(this, slidingData);
    }

    protected override void AddDatas()
    {
        DataDict.Add(nameof(MovementData), movementData);
        DataDict.Add(nameof(WallRunningData), wallRunningData);
		DataDict.Add(nameof(BreakFallData), breakFallData);
		DataDict.Add(nameof(ClimpUpData), climbUpData);
		DataDict.Add(nameof(SlidingData), slidingData);
    }

    private void Update()
    {
        if (playerState == PlayerState.Normal && !_climbUpComponent.IsOnClimbing)
        {
            playerMovementComponent.GroundCheck();
		}

        _climbUpComponent.CheckForClimbing();
        if (!playerMovementComponent.IsOnGround) _climbUpComponent.TryClimb();
    }


    private void FixedUpdate()
    {
		if (playerState == PlayerState.Normal && !_climbUpComponent.IsOnClimbing)
		{
			playerMovementComponent.MouseLookAround(_lookAroundDelta);
		}

		_wallRunningComponent.TryWallRun(playerMovementComponent.IsOnGround);
		if (_wallRunningComponent.IsWallRunning)
		{
			_wallRunningComponent.WallRun(playerMovementComponent.IsOnGround);
			return;
		}

		playerMovementComponent.ApplyGravity();
        switch (playerState)
        {
            case PlayerState.Normal:
                //normal

                playerMovementComponent.ApplyFriction();
                playerMovementComponent.Move(_directionInput);
                break;
            case PlayerState.BreakFall:
                _breakFallComponent.BreakFallFriction();
                break;
            case PlayerState.Sliding:
                //if(슬로프인가?) 슬로프상태면 마찰없앰
                _slidingComponent.SlidingFriction(playerMovementComponent.IsOnGround);
                break;
        }
	}

    public enum PlayerState
    {
        Normal,
        BreakFall,
        Sliding
    }

    public void PassCollisionInfo(CollisionInfo info)
    {
        switch (info.sender.name)
        {
            case "@GroundCheckCollider":
                playerMovementComponent.ReceiveCollisionInfo(info);
                break;
            case "@LeftForward" or "@LeftBack" or "@RightForward" or "@RightBack":
                _wallRunningComponent.HandleCollision(info);
                break;
        }
        _climbUpComponent.HandleCollisionInfo(info);
    }

    private void OnDrawGizmosSelected()
    {
        CapsuleCollider _col = GetComponent<CapsuleCollider>();
        Gizmos.color = Color.yellow;

        Vector3 lowerCapsuleCore = transform.position + _col.center - Vector3.up * (_col.height * 0.5f - _col.radius);
        Vector3 checkPoint = Vector3.down * _col.height * .1f;

        Gizmos.DrawWireSphere(lowerCapsuleCore + checkPoint, _col.radius);
    }
}