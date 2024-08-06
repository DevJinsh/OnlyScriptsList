using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMachineBullet : MonoBehaviour
{
    private Rigidbody _rb;
    private float _speed = .1f;
    private Vector3 _dir;
    public bool stop;
    public LayerMask collisionMask;
    public GameObject destroyEffect;

    private void OnEnable()
    {
        _dir = this.transform.rotation * Vector3.up;
        _rb = GetComponent<Rigidbody>();
        stop = false;
    }
    void FixedUpdate()
    {
        if (!stop)
        {
            if (_rb.velocity.magnitude < 20)
                _rb.AddForce(_dir * _speed, ForceMode.Force);
            _dir = _rb.velocity.normalized;
        }
        else
        {
            _dir = _rb.velocity.normalized;
            this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) - 90f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & collisionMask.value) != 0)
        {
            GameObject effectObject = Instantiate(destroyEffect, this.transform.position, this.transform.rotation);
            effectObject.GetComponent<ParticleSystem>().Play();
            SoundManager.instance.SoundPlay("BulletDestroy", this.gameObject);
            Destroy(effectObject, 1f);
            Destroy(this.gameObject);
        }
    }
}
