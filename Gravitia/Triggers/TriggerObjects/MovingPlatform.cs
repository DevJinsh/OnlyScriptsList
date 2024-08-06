using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : TriggerObject
{
    public float moveTime;

    private Vector3 _originStartPos;
    private Vector3 _originEndPos;

    private float _originDistance;
    private float _tickDistance;
    private float _tickPersent;

    private GameObject _player;

    private Coroutine _coroutine;
    private void Start()
    {
        _originStartPos = transform.position;
        _originEndPos = transform.GetChild(0).position;

        _originDistance = (_originStartPos - _originEndPos).magnitude;
    }
    public override void TriggerActive()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(Moving(_originStartPos, _originEndPos, 0));
        SoundManager.instance.SoundPlay("DoorOpen", this.gameObject);
    }

    public override void TriggerInactive(float resetTime)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(Moving(_originEndPos, _originStartPos, resetTime));
    }

    IEnumerator Moving(Vector3 startPos, Vector3 endPos, float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        //닫는 경우
        float currentPercent = (_originDistance - (endPos - transform.position).magnitude) / _originDistance;
        if (startPos == _originEndPos && currentPercent < 1)
        {
            SoundManager.instance.SoundPlay("DoorClose", this.gameObject);
        }
        while (currentPercent < 1)
        {
            yield return null;
            _tickDistance = Time.deltaTime * (_originDistance / moveTime); // 프레임당 이동거리
            _tickPersent = _tickDistance / _originDistance; // 프레임당 이동거리의 퍼센트율
            transform.position = Vector3.Lerp(startPos, endPos, currentPercent);
            currentPercent += _tickPersent;
            if (_player != null)
            {
                _player.transform.position += (endPos - startPos).normalized * _tickDistance;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player") && gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _player = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            _player = null;
        }
    }
}
