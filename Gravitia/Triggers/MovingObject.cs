using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float moveSpeed;

    [SerializeField] Vector3 _originEndLocalPos;
    Vector3 _originStartLocalPos;
    private float _originDistance;

    private void Start()
    {
        _originStartLocalPos = this.transform.localPosition;
        _originDistance = (_originStartLocalPos - _originEndLocalPos).magnitude;
    }

    private void Update()
    {
        Moving();
    }
    void Moving()
    {
        Vector3 pos = (_originStartLocalPos + _originEndLocalPos) / 2f;
        pos.x += _originDistance / 2f * Mathf.Sin(Time.time * moveSpeed);
        transform.localPosition = pos;
    }
}
