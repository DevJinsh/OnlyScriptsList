using System.Collections;
using UnityEngine;

public class ClimbUpComponent : BaseComponent
{
	private MovementData _mData;
	private ClimpUpData _data;

	private Rigidbody _rigidbody;
	private Camera _camera;

	public bool IsOnClimbing;
	public bool CanClimbing;
	
	private bool _isWallAColliding;
	private bool _isWallBColliding;
	private bool _isWallColliding;

	private Vector3 _initialRotation;
	private float _destinationY;
	private Vector3 _destination;
	
	private Vector3 _velocity;

	private Coroutine _climbingAnimationCoroutine;

	public ClimbUpComponent(BaseController controller, ClimpUpData data, MovementData mData) : base(controller)
	{
		_data = data;
		_mData = mData;
		_rigidbody = controller.GetComponent<Rigidbody>();
		_camera = Camera.main;
	}

	//체크
	public void CheckForClimbing()
	{
		if (!IsOnClimbing && _isWallColliding) // 나중에 그라운드 체크 병행
		{
			_destinationY = GetYDestination();

			CanClimbing = _destinationY != 0;
		}
		else
		{
			CanClimbing = false;
			_velocity = Vector3.zero;
		}
	}

	public void TryClimb()
	{
		if (CanClimbing)
		{
			CanClimbing = false;
			IsOnClimbing = true;
			_destination = Controller.transform.position + Controller.transform.forward + new Vector3(0, _destinationY, 0);

			if (_climbingAnimationCoroutine != null)
				Controller.StopCoroutine(_climbingAnimationCoroutine);
			float _flatSpeed = _rigidbody.velocity.GetFlatMagnitude();
			float speed = Mathf.Max(_flatSpeed * _data.decreaseRate, _mData.jumpHeight / _mData.reachTime);
			_climbingAnimationCoroutine = Controller.StartCoroutine(ClimbingAnimation(_destination, speed));
		}
	}

	public float GetYDestination()
	{
		Vector3 position = Controller.gameObject.transform.position + (Controller.gameObject.transform.forward * 0.71f);
		for (int i = 0; i < _data.maxOverlapBoxOffset; i++)
		{
			position += Vector3.up * 0.5f;
			Collider[] colliders = Physics.OverlapBox(position, new Vector3(0.23f, 0.7f, 0.5f), Quaternion.identity, LayerMask.GetMask("Ground"));
			if (colliders.Length == 0)
			{
				return position.y;
			}
		}
		return 0;
	}

	IEnumerator ClimbingAnimation(Vector3 destination, float speed)
	{
		_initialRotation = _camera.transform.rotation.eulerAngles;

		Vector3 myVelocity = _velocity;
		_rigidbody.isKinematic = true;

		Vector3 newYPosition = new(Controller.transform.position.x, GetYDestination(), Controller.transform.position.z);
		float timer = 0;

		(Controller as PlayerController).playerMovementComponent.camXRotation = 0;
		Vector3 targetLookRotation = GetLookRotation(Controller.transform, _destination);
		//Y축이동1
		while (Vector3.Distance(Controller.transform.position, newYPosition) > 0.02f)
		{
			timer += Time.deltaTime;
			if (timer > _data.lookupStartTime)
			{
				float x = Mathf.LerpAngle(_initialRotation.x, targetLookRotation.x, (timer - _data.lookupStartTime) / _data.lookupDuration);
				_camera.transform.eulerAngles = new(x, _camera.transform.eulerAngles.y, _camera.transform.eulerAngles.z);
			}
			Controller.transform.position = Vector3.MoveTowards(Controller.transform.position, newYPosition, speed * (Time.deltaTime));

			yield return null;
		}

		//X축이동
		timer = 0;
		Vector3 newXPosition = new Vector3(destination.x, Controller.transform.position.y, destination.z);
		float currentCameraRotationX = _camera.transform.eulerAngles.x;
		float distance = Vector3.Distance(Controller.transform.position, new Vector3(destination.x, Controller.transform.position.y, destination.z));
		while (distance >= 0.02f || timer < _data.lookdownDuration)
		{
			timer += Time.deltaTime;
			float x = Mathf.LerpAngle(currentCameraRotationX, 0, timer / _data.lookdownDuration);
			_camera.transform.eulerAngles = new(x, _camera.transform.eulerAngles.y, _camera.transform.eulerAngles.z);

			// lookdown duration이 너무 크면 목표보다 더 멀리 앞으로 감.
			Controller.transform.position = Vector3.MoveTowards(Controller.transform.position, newXPosition, speed * Time.deltaTime);
			distance = Vector3.Distance(Controller.transform.position, new Vector3(destination.x, Controller.transform.position.y, destination.z));

			yield return null;
		}

		_rigidbody.isKinematic = false;

		_rigidbody.velocity = myVelocity.GetFlatVector();

		//while (Vector3.Distance(_rigidbody.velocity, myVelocity) < 0.1f)
		//{
		//	_rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, myVelocity, speed * Time.deltaTime);
		//	yield return null;
		//}

		IsOnClimbing = false;
		_velocity = Vector3.zero;
		yield return null;
	}

	private Vector3 GetLookRotation(Transform from, Vector3 to)
	{
		Vector3 direction = to - from.position;
		float distance = Vector3.Distance(from.position, to);
		float angleX = Mathf.Atan2(direction.y, distance) * Mathf.Rad2Deg;

		Vector3 currentRotation = from.eulerAngles;

		currentRotation.x = -angleX;

		return currentRotation;
	}

	public void HandleCollisionInfo(CollisionInfo collisionInfo)
	{
		if (!collisionInfo.other.CompareTag("Ground")) return;
		switch (collisionInfo.sender.name)
		{
			case "@WallColliderA":
				_isWallAColliding = collisionInfo.phase != CollisionPhase.Exit;

				break;
			case "@WallColliderB":
				_isWallBColliding = collisionInfo.phase != CollisionPhase.Exit;
				break;
			default:
				return;
		}

		_isWallColliding = _isWallAColliding && _isWallBColliding;

		// 현재 가속 방향과 바라보는 방향이 거의 같은 방향인지 확인하고, 같은 방향이면 현재 가속도를 저장
		// 완벽한 해결책은 아님. 여전히 가속도가 0으로 넘어오는 문제 있음
		if (_isWallAColliding || _isWallBColliding)
		{
			if (Vector3.Dot(_rigidbody.velocity.GetFlatVector().normalized, Controller.transform.forward) > 0.5)
			{
				_velocity = _rigidbody.velocity.GetFlatMagnitude() > _velocity.GetFlatMagnitude() ? _rigidbody.velocity : _velocity;
			}
		}
	}
}