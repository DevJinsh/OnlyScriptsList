using System.Collections;
using UnityEngine;

public class BreakFallComponent : BaseComponent
{
    private readonly BaseController _controller;
    private BreakFallData _data;

    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private Camera _camera;

	private bool _isOnBreakFall;

    public BreakFallComponent(BaseController controller, BreakFallData data) : base(controller)
    {
        _controller = controller;
        _data = data;
        _rigidbody = _controller.GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    public void InitBreakFalling(PlayerController playerController)
    {
		if (_isOnBreakFall) return;

		if (!playerController.playerMovementComponent.IsOnGround)
		{
			Controller.StartCoroutine(CoTryBreakFall(playerController));
		}
    }

	private IEnumerator CoTryBreakFall(PlayerController playerController)
	{
		_isOnBreakFall = true;

		float validTimeCounter = 0;
		float validTime = _data.validTime;
		float yVelocity = 0f;
		while (validTimeCounter < validTime)
		{
			validTimeCounter += Time.deltaTime;
			yVelocity = Mathf.Min(yVelocity, _rigidbody.velocity.y);
			if (playerController.playerMovementComponent.IsOnGround)
			{
				_playerController = playerController;
				_playerController.playerState = PlayerController.PlayerState.BreakFall;

				_controller.StartCoroutine(CoCameraRotate(_camera.transform.eulerAngles.x, 0));

				yield return new WaitForSeconds(_data.cameraSpinTime + 0.1f);

				if (yVelocity < _data.yVelocityThreshold)
				{
					_rigidbody.AddRelativeForce(Vector3.forward * _data.boostPower, ForceMode.VelocityChange);
				}
				break;
			}
			yield return null;
		}

		_isOnBreakFall = false;
        _playerController.playerState = PlayerController.PlayerState.Normal;
    }

    //구르기 마찰력 적용
    public void BreakFallFriction()
    {
        Vector3 frictionDirection = new Vector3(-_rigidbody.velocity.x, 0f, -_rigidbody.velocity.z).normalized * Time.fixedDeltaTime * _data.breakFallFriction;
        if (Mathf.Abs(_rigidbody.velocity.x) - Mathf.Abs(frictionDirection.x) < 0) frictionDirection.x = -_rigidbody.velocity.x;
        if (Mathf.Abs(_rigidbody.velocity.z) - Mathf.Abs(frictionDirection.z) < 0) frictionDirection.z = -_rigidbody.velocity.z;
        _rigidbody.AddForce(frictionDirection, ForceMode.VelocityChange);
    }

	private IEnumerator CoCameraRotate(float startXRotation, float endXRotation)
	{
		float current_time = 0f;
		float rotation = startXRotation;
		float end = (_camera.transform.eulerAngles.x % 360) + 360;

		Vector3 currentAngles = _camera.transform.eulerAngles;

		while (current_time <= _data.cameraSpinTime)
		{
			current_time += Time.deltaTime;
			float xRotation = Mathf.Lerp(rotation % 360, end, current_time / _data.cameraSpinTime);
			_camera.transform.eulerAngles = new(xRotation, currentAngles.y, currentAngles.z);
			yield return null;
		}
		_camera.transform.eulerAngles = new(endXRotation, currentAngles.y, currentAngles.z);
		(Controller as PlayerController).playerMovementComponent.camXRotation = 0;
	}
}
