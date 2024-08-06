using System.Collections;
using UnityEngine;
public class SlidingComponent : BaseComponent
{
    private Vector3 _originalColliderCenter;
    private float _originalColliderHeight;
    private readonly BaseController _controller;
    private SlidingData _data;

    private Rigidbody _rigidbody;
    private Camera _camera;
    private Vector3 _cameraOriginPos;
    private CapsuleCollider _capsuleCollider;

    private Coroutine _coroutine;
    private Coroutine _cameraMoveCoroutine;
    private bool _isTimeOver;
    private bool _isCoolTime;
    public SlidingComponent(BaseController controller, SlidingData data) : base(controller)
    {
        _controller = controller;
        _data = data;
        _camera = Camera.main;
        _rigidbody = _controller.GetComponent<Rigidbody>();
        _cameraOriginPos = _camera.transform.localPosition;
        _capsuleCollider = controller.GetComponent<CapsuleCollider>();
        _originalColliderCenter = _capsuleCollider.center;
        _originalColliderHeight = _capsuleCollider.height;
    }

    public void StartSliding(PlayerController playerController)
    {
        _capsuleCollider.center = _data.shrinkedColliderCenter;
        _capsuleCollider.height = _data.shrinkedColliderHeight;
        _isTimeOver = false;

        if (_cameraMoveCoroutine != null) _controller.StopCoroutine(_cameraMoveCoroutine);
        _cameraMoveCoroutine = _controller.StartCoroutine(CameraMove(_camera.transform.localPosition, _data.cameraHeight));
        
        playerController.playerState = PlayerController.PlayerState.Sliding;
        if(!_isCoolTime)
        {
            _isCoolTime = true;
            _controller.StartCoroutine(SlidingCoolTimeTimer());
            _rigidbody.AddForce(new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z).normalized * _data.slidingAcceleration);
        }
        if (_coroutine != null) _controller.StopCoroutine(_coroutine);
        _coroutine = _controller.StartCoroutine(SlidingTimer());
    }

    public void SlidingFriction(bool isOnGround)
    {
        bool isFlat = Vector3.Dot((Controller as PlayerController).playerMovementComponent.currentNormal, Vector3.up) > 0.99f;
        bool isSlopingDown = isOnGround && _rigidbody.velocity.y < 0f && !isFlat;

        float delta = _isTimeOver ? _data.slidingUpdateFriction : _data.slidingFriction;
        if (!isSlopingDown)
        {
            FrictionUpdate(delta);
        }
    }

    public void FrictionUpdate(float delta)
    {
        Vector3 frictionDirection = new Vector3(-_rigidbody.velocity.x, 0f, -_rigidbody.velocity.z).normalized * Time.fixedDeltaTime * delta;
        if (Mathf.Abs(_rigidbody.velocity.x) - Mathf.Abs(frictionDirection.x) < 0) frictionDirection.x = -_rigidbody.velocity.x;
        if (Mathf.Abs(_rigidbody.velocity.z) - Mathf.Abs(frictionDirection.z) < 0) frictionDirection.z = -_rigidbody.velocity.z;
        _rigidbody.AddForce(frictionDirection, ForceMode.VelocityChange);
    }

    public void EndSliding(PlayerController playerController)
    {
        _capsuleCollider.center = _originalColliderCenter;
        _capsuleCollider.height = _originalColliderHeight;
        _capsuleCollider.radius = 0.5f;
        playerController.playerState = PlayerController.PlayerState.Normal;

        if (_cameraMoveCoroutine != null) _controller.StopCoroutine(_cameraMoveCoroutine);
        _cameraMoveCoroutine = _controller.StartCoroutine(CameraMove(_camera.transform.localPosition, _cameraOriginPos));

        _controller.StopCoroutine(_coroutine);
    }
    IEnumerator SlidingTimer()
    {
        float timer = _data.slidingTime;
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }
        _isTimeOver = true;
    }
    IEnumerator SlidingCoolTimeTimer()
    {
        float timer = _data.slidingCoolTime;
        while (timer > 0)
        {
            yield return null;
            timer -= Time.deltaTime;
        }
        _isCoolTime = false;
    }

    IEnumerator CameraMove(Vector3 startPos, Vector3 endPos)
    {
        float current_time = 0f;
        while (current_time <= _data.cameraMoveTime)
        {
            current_time += Time.deltaTime;
            if (current_time > _data.cameraMoveTime) current_time = _data.cameraMoveTime;
            _camera.transform.localPosition = Vector3.Lerp(startPos, endPos, current_time / _data.cameraMoveTime);
            yield return null;
        }
    }
}
