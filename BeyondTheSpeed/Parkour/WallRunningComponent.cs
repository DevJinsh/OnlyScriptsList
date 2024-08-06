using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallRunningComponent : BaseComponent
{
	private WallRunningData _data;

	private Rigidbody _rigidbody;

	private bool _wallLeft;

	private bool _wallLeftForward;

	private bool _wallLeftBack;

	private bool _wallRight;

	private bool _wallRightForward;

	private bool _wallRightBack;

	private bool _angleIsAccepted;

	private bool _canStartWallRun;

	private Vector3 _direction;

	private float _upwardforce;

	private float _desiredSpeed;

	private float _minRuntimeCounter = 0;

	private Vector3 _directionWhenStartWallRun;

	private Coroutine _cameraEndCoroutine;

	private Camera _camera;

	private float _timer;

	private Vector3 _initialRotation;

	private float _cameraAngle;

	private bool _isTurning;

	public bool IsWallRunning { get; private set; }

	public Vector3 WallForward => _direction;

	public WallRunningComponent(BaseController controller, WallRunningData data) : base(controller)
	{
		_data = data;
		_camera = Camera.main;
		_rigidbody = controller.GetComponent<Rigidbody>();
	}

	public void TryWallRun(bool isOnGround)
	{
		if (isOnGround)
		{
			_canStartWallRun = true;
		}

		bool wallDetected = (_wallLeft || _wallRight) && _angleIsAccepted;
		if (!isOnGround && wallDetected && _canStartWallRun)
		{
			if (_cameraEndCoroutine != null)
			{
				Controller.StopCoroutine(_cameraEndCoroutine);
				_cameraEndCoroutine = null;
				_camera.transform.eulerAngles = new(_camera.transform.eulerAngles.x, _camera.transform.eulerAngles.y, _initialRotation.z);
			}

			// wall run
			_canStartWallRun = false;
			PrepareWallRun();
			_minRuntimeCounter = _data.minRuntime;
			_directionWhenStartWallRun = Controller.transform.forward;
			IsWallRunning = true;
		}
	}

	public void WallRun(bool isOnGround)
	{
		if (!IsWallRunning)
		{
			return;
		}

		_minRuntimeCounter -= Time.fixedDeltaTime;
		_timer += Time.fixedDeltaTime;

		bool wallDetected = _wallLeftForward || _wallLeftBack || _wallRightForward || _wallRightBack;
		if (!wallDetected || isOnGround)
		{
			StopWallRun();
			return;
		}

		if (_rigidbody.velocity.y < _data.minYVelocity && _minRuntimeCounter < 0)
		{
			StopWallRun();
			return;
		}

		_rigidbody.velocity = new(_direction.x * _desiredSpeed, _upwardforce, _direction.z * _desiredSpeed);
		float delta = _upwardforce > 0 ? _data.upwardDelta : _data.downwardDelta;
		_upwardforce -= delta * Time.fixedDeltaTime;

		float z = Mathf.LerpAngle(_initialRotation.z, _cameraAngle, _timer / _data.cameraStartTime);
		_camera.transform.eulerAngles = new(_camera.transform.eulerAngles.x, _camera.transform.eulerAngles.y, z);
	}

	public void StopWallRun()
	{
		IsWallRunning = false;
		_cameraEndCoroutine = Controller.StartCoroutine(CoRevertCameraAngle());
	}

	public void TurnToWallForward()
	{
		if (_isTurning || !IsWallRunning)
		{
			return;
		}

		Controller.StartCoroutine(CoTurnToWallForward());
	}

	public void HandleCollision(CollisionInfo collisionInfo)
	{
		if (!collisionInfo.other.CompareTag("Ground"))
		{
			return;
		}

		bool isContactedWithWall = collisionInfo.phase != CollisionPhase.Exit;
		bool isRelativeCollider = true;
		_ = collisionInfo.sender.name switch
		{
			"@LeftForward" => _wallLeftForward = isContactedWithWall,
			"@LeftBack" => _wallLeftBack = isContactedWithWall,
			"@RightForward" => _wallRightForward = isContactedWithWall,
			"@RightBack" => _wallRightBack = isContactedWithWall,
			_ => isRelativeCollider = false
		};

		if (!isRelativeCollider)
		{
			return;
		}

		_wallLeft = _wallLeftForward && _wallLeftBack;
		_wallRight = _wallRightForward && _wallRightBack;

		if (_wallLeft || _wallRight)
		{
			var closestPoint = collisionInfo.other.ClosestPoint(Controller.transform.position);
			Vector3 directionToWall = (closestPoint - Controller.transform.position).normalized;
			Vector3 directionToForward = Vector3.Cross(directionToWall, Vector3.up);
			if ((Controller.transform.forward - directionToForward).magnitude > (Controller.transform.forward - -directionToForward).magnitude)
			{
				directionToForward = -directionToForward;
			}
			_direction = directionToForward;

			float dot = Vector3.Dot(Controller.transform.forward, directionToWall);
			_angleIsAccepted = dot > _data.minDotValue && dot < _data.maxDotValue;
		}
	}

	private IEnumerator CoTurnToWallForward()
	{
		_isTurning = true;
		var look = Quaternion.LookRotation(_direction);
		float timer = 0;
		while (timer < _data.turnDuration)
		{
			timer += Time.deltaTime;
			Controller.transform.rotation = Quaternion.Slerp(Controller.transform.rotation, look, timer / _data.turnDuration);
			yield return null;
		}

		Controller.transform.rotation = look;
		_isTurning = false;
	}

	private IEnumerator CoRevertCameraAngle()
	{
		float timer = 0;
		while (timer < _data.cameraEndTime)
		{
			timer += Time.deltaTime;
			float z = Mathf.LerpAngle(_cameraAngle, _initialRotation.z, timer / _data.cameraEndTime);
			_camera.transform.eulerAngles = new(_camera.transform.eulerAngles.x, _camera.transform.eulerAngles.y, z);
			yield return null;
		}

		_camera.transform.eulerAngles = new(_camera.transform.eulerAngles.x, _camera.transform.eulerAngles.y, _initialRotation.z);
		_cameraEndCoroutine = null;
	}

	private void PrepareWallRun()
	{
		_initialRotation = _camera.transform.rotation.eulerAngles;
		_upwardforce = _rigidbody.velocity.y;
		_desiredSpeed = Mathf.Max(_data.minSpeed, _rigidbody.velocity.GetFlatMagnitude() * _data.wallRunSpeedMultiplier);
		_cameraAngle = _wallRight ? _data.cameraAngle : -_data.cameraAngle;
		_timer = 0;
	}

	public void TryJump()
	{
		StopWallRun();

		float jumpForce = _rigidbody.velocity.GetFlatMagnitude() * _data.jumpForceMultiplier;
		Vector3 direction = Vector3.Cross(_directionWhenStartWallRun, Vector3.up).normalized;
		if ((_direction - direction).magnitude > (_direction - -direction).magnitude)
		{
			direction = -direction;
		}
		Vector3 jumpDirection = ((Vector3.up * 2f) + direction + Controller.transform.forward).normalized;
		_rigidbody.AddForce(jumpDirection * jumpForce, ForceMode.VelocityChange);
	}
}