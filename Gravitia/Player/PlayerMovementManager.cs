using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementManager : MonoBehaviour, InputSystemAction.IPlayerActions
{
    #region Components
    private InputSystemAction _inputActions;
    private Rigidbody _rigidbody;
    #endregion
    #region Public
    #region ShowInspector
    [Header("이동속도")]
    [Tooltip("뛰기 시작하는 시간")]
    public float walkToSprintTime; // 뛰기 시작하는 시간(가속도)
    [Tooltip("미는 속도")]
    public float pushingSpeed; // 미는 속도
    [Tooltip("공중 이동가속도")]
    public float inAirAccelationSpeed; // 공중 이동가속도
    [Tooltip("공중 최대 이동속도")]
    public float maxInAirSpeed; // 공중 최대 이동속도
    [Header("점프력")]
    [Tooltip("점프 코요테 타임")]
    public float jumpCoyoteTime; // 코요테 타임
    [Tooltip("점프 높이")]
    public float jumpHeight; // 점프 높이
    [Tooltip("최대 높이 도달 시간")]
    public float maxHeightTime; // 최대 높이 도달 시간
    [Header("낙하 인식 시간")]
    public float fallingTime; // 낙하 인식 시간
    [Header("레이어 마스크")]
    public LayerMask layerMask;
    public LayerMask slopeLayerMask;
    #endregion
    #region HideInspector
    [HideInInspector] public bool isGravityArea = false;
    [HideInInspector] public bool isGround = true;
    [HideInInspector] public bool canMove = true;
    [HideInInspector] public bool canClimb = true;
    [HideInInspector] public bool useClimb = false;
    [HideInInspector] public bool isSeesaw = false;
    [HideInInspector] public GravityGun gravityGun;
    [HideInInspector] public Vector3 pointerPos;
    #endregion
    #region Property
    public bool Charging
    {
        set { _isCharging = value; }
        get { return _isCharging; }
    }
    #endregion
    #endregion
    #region Private
    private float _inputX;
    private float _animationMoveX;
    private float _accelationX;

    private bool _isReverse = false;
    private bool _isMove = false;
    private bool _canJump = true;
    private bool _isJumping = false;
    private bool _isFalling = false;
    private bool _isClimb = false;
    private bool _isSlope = false;
    private bool _isSpringX = false;

    private bool _isCharging = false;
    private bool _isAiming = false;

    //Slope
    [Header("슬로프")]
    [SerializeField] private Transform _originRay;
    private RaycastHit _slopeHit;
    private float _maxSlopeAngle = 40f;

    private Coroutine _fallingCoroutine;
    private Coroutine _climbCoroutine;
    private Coroutine _jumpingCoroutine;
    private Coroutine _jumpCoyoteTimeCoroutine;
    //GroundCheckBoxCast
    private LiftObject _groundLiftObject;
    private float _maxGroundCheckDistance = 1f;
    private Vector3 _checkGroundBoxCastSize = new Vector3(0.2f, 0.1f, 0.2f);
    #endregion

    #region LifeCycle
    private void Awake()
    {
        _inputActions = new InputSystemAction();
        _inputActions.Player.SetCallbacks(instance: this);
        _inputActions.Enable();

        gravityGun = GetComponent<GravityGun>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        GameManager.Instance.RestartWithGameManager();
        FindAnyObjectByType<UIManager>().ImageMouseCorsor();
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        Look();
        if (isGravityArea || !canMove)
            return;
        isGround = CheckGround(transform.position + Vector3.up * 1f);
        if (!isGround && _fallingCoroutine == null)
        {
            _fallingCoroutine = StartCoroutine(FallingTimeCounting());
        }
    }
    #endregion
    #region Input System
    public void OnJump(InputAction.CallbackContext context)
    {
        JudgeController(context);
        if (GameManager.Instance.animationManager.canSkip)
        {
            GameManager.Instance.animationManager.SkipAnimation();
        }
        if (_isClimb || !canMove)
            return;
        if (context.started && _canJump && !isGravityArea && !_isJumping)
        {
            Jump();
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        JudgeController(context);
        _inputX = context.ReadValue<Vector2>().x;
        if (!canMove)
            return;
        if (context.performed && !isGravityArea)
        {
            if (!_isAiming && context.ReadValue<Vector2>() != Vector2.zero)
            {
                pointerPos = context.ReadValue<Vector2>().normalized;
            }
            _isMove = true;
        }
        if (context.canceled)
        {
            _isMove = false;
            _accelationX = 0;
        }
    }

    public void OnShot(InputAction.CallbackContext context)
    {
        JudgeController(context);
        if (_isClimb || !canMove)
            return;
        if (context.performed && gravityGun.indicator.gameObject.activeSelf)
        {
            Charging = true;
            GameManager.Instance.hapticManager.RumbleGamePad(0.1f, Mathf.Infinity);
        }
        if (context.canceled && gravityGun.indicator.gameObject.activeSelf && Charging)
        {
            Charging = false;
            gravityGun.Shoot();
        }
    }

    public void OnAiming(InputAction.CallbackContext context)
    {
        JudgeController(context);
        if (!canMove)
        {
            return;
        }
        if (context.ReadValue<Vector2>() == Vector2.zero)
        {
            _isAiming = false;
        }
        else
        {
            _isAiming = true;
        }
        if (GameManager.Instance.CurrentDevice == GameManager.Device.PC)
        {
            pointerPos = new Vector2(context.ReadValue<Vector2>().x - Camera.main.WorldToScreenPoint(transform.position).x,
                context.ReadValue<Vector2>().y - (Camera.main.WorldToScreenPoint(transform.position + Vector3.up).y)).normalized;
        }
        else if (context.ReadValue<Vector2>().normalized != Vector2.zero)
        {
            pointerPos = context.ReadValue<Vector2>().normalized;
        }
    }
    private void JudgeController(InputAction.CallbackContext context)
    {
        if (context.control.device.displayName == "Mouse" || context.control.device.displayName == "Keyboard")
        {
            GameManager.Instance.CurrentDevice = GameManager.Device.PC;
        }
        else
        {
            GameManager.Instance.CurrentDevice = GameManager.Device.GamePad;
        }
    }
    public void OnSavePointCanvas(InputAction.CallbackContext context)
    {
        JudgeController(context);
        if (context.started)
        {
            FindAnyObjectByType<SavePointController>().ShowSavePointMenu();
        }
    }
    #endregion
    #region Movement
    private void Look()
    {
        if (_inputX != 0)
        {
            _isReverse = Mathf.Sign(pointerPos.x) + Mathf.Sign(_inputX) == 0 ? true : false;
            GameManager.Instance.animationManager.Reverse(_isReverse);
        }
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 90f * Mathf.Sign(pointerPos.x), 0), 0.1f);
    }
    private void Move()
    {
        if (!canMove || isGravityArea || _isClimb)
            return;
        if (isGround && !_isJumping) // 땅에서 걸어다닐때
        {
            _rigidbody.velocity = CalcurateSlopeAngle() > _maxSlopeAngle ? Vector3.zero :
                _isSlope ? Vector3.ProjectOnPlane(Vector3.right * _inputX, _slopeHit.normal).normalized * Mathf.Abs(_animationMoveX) * 4f :
                Vector3.right * _animationMoveX * 4f + Vector3.up * _rigidbody.velocity.y;
        }
        else // 공중에 있을 때
        {
            _rigidbody.AddForce(Vector3.right * _inputX * inAirAccelationSpeed, ForceMode.Acceleration);
            if (!_isSpringX)
                _rigidbody.velocity = new Vector3(Mathf.Clamp(_rigidbody.velocity.x, -maxInAirSpeed, maxInAirSpeed), _rigidbody.velocity.y, 0);
        }
        if (_isMove) // 움직임 애니메이션 계산
        {
            _accelationX += (1 / walkToSprintTime) * Time.deltaTime;
        }
        _animationMoveX = Mathf.Clamp(_inputX * Mathf.Min(_accelationX, 1f), -1, 1);
        GameManager.Instance.animationManager.Move(Mathf.Abs(_animationMoveX), _isMove);
    }
    private void Jump()
    {
        if (_jumpCoyoteTimeCoroutine != null)
        {
            StopCoroutine(_jumpCoyoteTimeCoroutine);
            _jumpCoyoteTimeCoroutine = null;
        }
        _isJumping = true;
        _canJump = false;

        GameManager.Instance.animationManager.Jump();
        _jumpingCoroutine = StartCoroutine(Jumping());
    }

    private void Landing()
    {
        _canJump = true;

        if (_isFalling)
        {
            _isFalling = false;
            GameManager.Instance.animationManager.Land();
        }
        if(_isJumping)
            _isJumping = false;
        _isSpringX = false;
        _fallingCoroutine = null;
    }
    #endregion
    #region SubFunction
    private bool CheckGround(Vector3 _centerPos)
    {
        if (Physics.BoxCast(_centerPos, _checkGroundBoxCastSize, Vector3.down, out RaycastHit _groundHitObj, transform.rotation, _maxGroundCheckDistance, layerMask))
        {
            if ((slopeLayerMask & (1 << _groundHitObj.collider.gameObject.layer)) != 0)
            {
                _isSlope = CheckSlope(_groundHitObj);
            }
            if (_groundHitObj.collider.CompareTag("InteractiveObj") && _groundHitObj.collider.GetComponent<LiftObject>() == null && _groundHitObj.collider.GetComponent<ObjectThePlayerFollows>() != null)
            {
                if (_groundLiftObject != null)
                    Destroy(_groundLiftObject);
                _groundLiftObject = _groundHitObj.collider.AddComponent<LiftObject>();
                _groundLiftObject.player = gameObject;
            }
            return true;
        }
        else
        {
            Destroy(_groundLiftObject);
            _groundLiftObject = null;
            _isSlope = false;
            if (!_isJumping)
                _rigidbody.useGravity = true;
            return false;
        }
    }
    private bool CheckSlope(RaycastHit hit)
    {
        _slopeHit = hit;
        if ((hit.transform.up.normalized - Vector3.up) != Vector3.zero && !isSeesaw)
        {
            _rigidbody.useGravity = false;
            return true;
        }
        else
        {
            _rigidbody.useGravity = true;
            return false;
        }
    }
    private float CalcurateSlopeAngle()
    {
        if (Physics.Raycast(_originRay.position, Vector3.right * Mathf.Sign(_inputX), out RaycastHit hit, 0.6f, slopeLayerMask))
        {
            return Vector3.Angle(Vector3.up, hit.normal);
        }
        return 0f;
    }
    public Vector3 CalcurateClimbPosition()
    {
        Collider coll = FindAnyObjectByType<CanClimbTrigger>().GetComponent<BoxCollider>();
        Vector3 rayCenter = coll.bounds.center + new Vector3(Mathf.Sign(_animationMoveX) * coll.bounds.extents.x, coll.bounds.extents.y, 0);
        if (Physics.Raycast(rayCenter, Vector3.down, out RaycastHit hit, 1f, layerMask))
        {
            return hit.point;
        }
        else
        {
            return rayCenter;
        }

    }

    public void InputActionDisable()
    {
        _inputActions.Disable();
    }

    public void StopFallingCoroutine()
    {
        if (_fallingCoroutine != null)
        {
            Landing();
            StopCoroutine(_fallingCoroutine);
            _fallingCoroutine = null;
        }
    }
    public void SpringXPlayer()
    {
        _isSpringX = true;
        isGround = false;
        _canJump = false;
    }
    public void CancelAming()
    {
        _isAiming = false;
    }


    #endregion
    #region Coroutine
    IEnumerator FallingTimeCounting()
    {
        _jumpCoyoteTimeCoroutine = StartCoroutine(JumpCoyoteTime());
        float currentTime = 0f;
        while (currentTime < fallingTime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            if (isGround)
            {
                Landing();
                yield break;
            }
            if (_isClimb)
                yield break;
        }
        StartCoroutine(Falling());
    }
    IEnumerator Falling()
    {
        _isFalling = true;
        GameManager.Instance.animationManager.Fall();
        yield return new WaitUntil(() => isGround);
        Landing();
    }
    IEnumerator JumpCoyoteTime()
    {
        float currentTime = 0;
        while (currentTime < jumpCoyoteTime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            if (isGround)
                yield break;
        }
        _canJump = false;
    }
    IEnumerator Jumping()
    {
        float currentTime = 0;
        float heightPerSec = jumpHeight / maxHeightTime;

        _rigidbody.useGravity = false;
        while (currentTime < maxHeightTime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, Mathf.Lerp(heightPerSec * 2f, 0f, currentTime / maxHeightTime), 0);
            if (_climbCoroutine == null && !isGround)
                _climbCoroutine = StartCoroutine(Climb());
        }
        if (isGround)
            Landing();
        _rigidbody.useGravity = true;
        _jumpingCoroutine = null;
    }
    IEnumerator Climb()
    {
        while (!isGround)
        {
            if (canClimb && useClimb && !_isReverse && _isMove)
            {
                _animationMoveX = _inputX;
                GameManager.Instance.StopPlayerMove();
                _rigidbody.velocity = Vector3.zero;
                GameManager.Instance.animationManager.Climb();
                if (_fallingCoroutine != null)
                {
                    StopCoroutine(_fallingCoroutine);
                    _fallingCoroutine = null;
                }
                if (_jumpingCoroutine != null)
                {
                    StopCoroutine(_jumpingCoroutine);
                    _jumpingCoroutine = null;
                }
                _climbCoroutine = null;
                yield break;
            }
            yield return null;
        }
        _climbCoroutine = null;
    }

    IEnumerator ClimbingAnimation(Vector3 startPos, Vector3 targetPos)
    {
        float currentTime = 0f;
        Vector3 tickDistance = new Vector3((targetPos - startPos).x, (targetPos - startPos).y, 0);
        while (currentTime < 0.10f)
        {
            yield return null;
            currentTime += Time.deltaTime;
            transform.position += Vector3.up * Time.deltaTime * (tickDistance.y / 0.10f);
        }
        currentTime = 0f;
        while (currentTime < 0.15f)
        {
            yield return null;
            currentTime += Time.deltaTime;
            transform.position += Vector3.right * Time.deltaTime * (tickDistance.x / 0.15f);
        }
    }
    #endregion
    #region 애니메이션 이벤트
    public void ClimbStart() // Climb애니메이션 이벤트 시작 시 호출
    {
        _isClimb = true;
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        StartCoroutine(ClimbingAnimation(transform.position, CalcurateClimbPosition()));
        GameManager.Instance.animationManager.Move(0f, false);
    }
    public void ClimbEnd() // Climb애니메이션 이벤트 종료 시 호출
    {
        _isClimb = false;
        _rigidbody.useGravity = true;
        GameManager.Instance.animationManager.Move(0f, false);
        GameManager.Instance.animationManager.ClimbEnd();
        GameManager.Instance.StartPlayerMove(false);
        Landing();
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //GroundCheck
        Vector3 _centerPos = transform.position + Vector3.up * 1f;
        if (Physics.BoxCast(_centerPos, _checkGroundBoxCastSize, Vector3.down, out RaycastHit hit, transform.rotation, _maxGroundCheckDistance, layerMask))
        {
            // Hit된 지점까지 ray를 그려준다.
            Gizmos.DrawRay(_centerPos, Vector3.down * hit.distance);

            // Hit된 지점에 박스를 그려준다.
            Gizmos.DrawWireCube(_centerPos + Vector3.down * hit.distance, _checkGroundBoxCastSize);
        }
        else
        {
            // Hit가 되지 않았으면 최대 검출 거리로 ray를 그려준다.
            Gizmos.DrawRay(_centerPos, Vector3.down * _maxGroundCheckDistance);
        }
    }

}
