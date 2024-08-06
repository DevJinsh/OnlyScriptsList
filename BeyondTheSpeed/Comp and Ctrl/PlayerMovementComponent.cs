using UnityEngine;

public class PlayerMovementComponent : BaseComponent
{
    private readonly BaseController _controller;

	public float camXRotation;
	public Vector3 currentNormal;

	private Transform _transform;
    private MovementData _data;

    private Rigidbody _rigidbody;
    private Camera _camera;
    private CapsuleCollider _playerCollider;

    private float _jumpVelocity;
    private float _lastJumpedTime;
	private float _coyoteTimeCounter;

    private bool _isOnGround;

	private float _initFov;
	private float _initZOffset;

    public bool IsOnGround { get { return _isOnGround; } }

    public delegate void OnGroundEvent();
    public OnGroundEvent onGroundEnter;
    public OnGroundEvent onGroundExit;

    public PlayerMovementComponent(BaseController controller, MovementData data) : base(controller)
    {
        _controller = controller;
        _data = data;

        _transform = _controller.transform;
        _rigidbody = _controller.GetComponent<Rigidbody>();
        _camera = Camera.main;
        _playerCollider = _controller.GetComponent<CapsuleCollider>();

		_initFov = _camera.fieldOfView;
		_initZOffset = _camera.transform.localPosition.z;

        CalculateValues();
    }

    public void ApplyFriction()
    {
        Vector3 frictionDirection = new Vector3(-_rigidbody.velocity.x, 0f, -_rigidbody.velocity.z).normalized * Time.fixedDeltaTime * _data.frictionAcceleration;
        if (Mathf.Abs(_rigidbody.velocity.x) - Mathf.Abs(frictionDirection.x) < 0) frictionDirection.x = -_rigidbody.velocity.x;
        if (Mathf.Abs(_rigidbody.velocity.z) - Mathf.Abs(frictionDirection.z) < 0) frictionDirection.z = -_rigidbody.velocity.z;
        if (_rigidbody.velocity.y > 0) frictionDirection = TranslateBySurfaceNormal(frictionDirection, currentNormal);
        _rigidbody.AddForce(frictionDirection, ForceMode.VelocityChange);
    }

    public void ApplyGravity()
    {
        if (Time.time - _lastJumpedTime < _data.reachTime && _rigidbody.velocity.y > 0f)
        {
            //무중력
        }
        else if (_isOnGround)
        {
            _rigidbody.AddForce(Vector3.up * _data.groundGravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
        else
        {
            float _jumpGravity = -2f * _data.jumpHeight / (_data.fallTime * _data.fallTime);
            _rigidbody.AddForce(Vector3.up * _jumpGravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }

	public void Move(Vector3 directionInput)
	{
		if (TranslateBySurfaceNormal(_rigidbody.velocity, currentNormal).magnitude < _data.thirdSpeedThreshold)
		{
			_rigidbody.AddRelativeForce(directionInput * _data.frictionAcceleration * Time.fixedDeltaTime, ForceMode.VelocityChange);
		}

		float othersideSpeed = Mathf.Abs(Controller.transform.InverseTransformDirection(_rigidbody.velocity).x);
		if (directionInput.z < 0)
		{
			othersideSpeed += Mathf.Abs(Controller.transform.InverseTransformDirection(_rigidbody.velocity).z);
		}

		Vector3 otherside = new(directionInput.x, directionInput.y, Mathf.Min(0, directionInput.z));
		Vector3 othersideForce = GetForce(otherside, othersideSpeed, _data.firstSpeedThreshold2, _data.secondSpeedThreshold2, _data.thirdSpeedThreshold2);

		Vector3 forward = new(0, directionInput.y, Mathf.Max(directionInput.z, 0));
		Vector3 forwardForce = GetForce(forward, _rigidbody.velocity.magnitude, _data.firstSpeedThreshold, _data.secondSpeedThreshold, _data.thirdSpeedThreshold);

		_rigidbody.AddRelativeForce(forwardForce + othersideForce, ForceMode.VelocityChange);
		DoDollyZoom();
	}

    public void MouseLookAround(Vector2 lookAroundDelta)
    {
        float xDelta = lookAroundDelta.x * Time.fixedDeltaTime;
        float yDelta = lookAroundDelta.y * Time.fixedDeltaTime;

		camXRotation -= yDelta;
		camXRotation = Mathf.Clamp(camXRotation, -90f, 90f);

		var z = _camera.transform.eulerAngles.z;
		_camera.transform.localRotation = Quaternion.Euler(camXRotation, 0f, 0);
		_camera.transform.eulerAngles = new(_camera.transform.eulerAngles.x, _camera.transform.eulerAngles.y, z);
		_transform.Rotate(Vector3.up * xDelta);
	}

    public void TryJump()
    {
        if (_isOnGround || _coyoteTimeCounter <= _data.coyoteTime)
        {
            DoJump();
        }
    }

    public void GroundCheck()
    {
		_coyoteTimeCounter += Time.deltaTime;
        var circleHits = Physics.SphereCastAll(_transform.position, _playerCollider.radius * 0.9f, Vector3.down, _playerCollider.height / 2f - 0.1f, LayerMask.GetMask("Ground"));
        bool isGroundFound = false;

        foreach (var circleHit in circleHits)
        {
            if (circleHit.collider.CompareTag("Ground"))
            {
                isGroundFound = true;
				_coyoteTimeCounter = 0;

				break;
            }
        }

		_isOnGround = isGroundFound;
        if (Physics.Raycast(_transform.position, Vector3.down, out var hit, _playerCollider.height * .6f, LayerMask.GetMask("Ground")))
        {
            currentNormal = hit.normal;
        }
        else currentNormal = Vector3.up;
    }

    public void ReceiveCollisionInfo(CollisionInfo info)
    {
		if (!info.other.CompareTag("Ground")) return;
        if (info.phase == CollisionPhase.Enter)
        {
			GroundEnter();
        }
        else if (info.phase == CollisionPhase.Exit)
        {
            GroundExit();
        }
    }

    /// <summary>
    /// FixedUpdate에서 호출
    /// </summary>
    public bool ShouldApplyGravity()
    {
        if (Time.time - _lastJumpedTime >= _data.reachTime || _rigidbody.velocity.y <= 0) return true;
        else return false;
    }

    void DoJump()
    {
        CalculateValues();

        _lastJumpedTime = Time.time;
        _rigidbody.AddForce(Vector3.up * (_jumpVelocity - _rigidbody.velocity.y), ForceMode.VelocityChange); //velocity가 원래 몇이든 startingJumpVelocity로 만드는 효과
	}

    void CalculateValues()
    {
        _jumpVelocity = _data.jumpHeight / _data.reachTime;
    }

    private void GroundEnter()
    {
        _isOnGround = true;
        onGroundEnter?.Invoke();
    }

    private void GroundExit()
    {
        _isOnGround = false;
        onGroundExit?.Invoke();
    }

    private Vector3 TranslateBySurfaceNormal(Vector3 originalVector, Vector3 surfaceNormal)
    {
        Vector3 translatedVector = Vector3.ProjectOnPlane(originalVector, surfaceNormal);
        return translatedVector;
    }

	private Vector3 GetForce(Vector3 directionInput, float currentSpeed, float firstThreshold, float secondThreshold, float thirdThreshold)
	{
		float lowerSpeedThreshold = 0;
		float higherSpeedThreshold = 0;
		float accelerationTime = Mathf.Infinity;

		if (currentSpeed < firstThreshold)
		{
			lowerSpeedThreshold = 0f;
			higherSpeedThreshold = firstThreshold;
			accelerationTime = _data.firstAccelerationTime;
		}
		else if (currentSpeed < secondThreshold)
		{
			lowerSpeedThreshold = firstThreshold;
			higherSpeedThreshold = secondThreshold;
			accelerationTime = _data.secondAccelerationTime;
		}
		else if (currentSpeed < thirdThreshold)
		{
			lowerSpeedThreshold = secondThreshold;
			higherSpeedThreshold = thirdThreshold;
			accelerationTime = _data.thirdAccelerationTime;
		}

		float deltaAcceleration = (higherSpeedThreshold - lowerSpeedThreshold) / accelerationTime * Time.fixedDeltaTime;
		Vector3 translatedDirectionBySlope = TranslateBySurfaceNormal(_transform.TransformDirection(directionInput), currentNormal);
		translatedDirectionBySlope = _transform.InverseTransformDirection(translatedDirectionBySlope);
		
		return translatedDirectionBySlope * deltaAcceleration;
	}

	private void DoDollyZoom()
	{
		float deltaAcceleration = (_data.secondSpeedThreshold - _data.firstSpeedThreshold) / _data.secondAccelerationTime * Time.fixedDeltaTime;
		float fov = _initFov;
		float zPos = _initZOffset;

		float forwardSpeed = Vector3.Dot(_rigidbody.velocity, Controller.transform.forward);
		if (forwardSpeed > (_data.firstSpeedThreshold + _data.secondSpeedThreshold) / 2f)
		{
			fov = 54;
			zPos = -1.085f;
			deltaAcceleration = (_data.thirdSpeedThreshold - _data.secondSpeedThreshold) / _data.thirdAccelerationTime * Time.fixedDeltaTime;
		}
		if (_rigidbody.velocity.GetFlatMagnitude() > _data.secondSpeedThreshold)
		{
			fov = _data.fov;
			zPos = _data.zOffset;
			deltaAcceleration = (_data.thirdSpeedThreshold - _data.secondSpeedThreshold) / _data.thirdAccelerationTime * Time.fixedDeltaTime;
		}

		_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, fov, deltaAcceleration);
		_camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, new Vector3(_camera.transform.localPosition.x, _camera.transform.localPosition.y, zPos), deltaAcceleration);
	}
}