using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public bool IsForce { get; set; }
    public float JumpForce { get; set; }
    public int ContactedGravityCount { get; set; }
    [SerializeField] Transform _originTransform;
    [SerializeField] float _maxLocalY;
    [SerializeField] float _power;
    [SerializeField] float _damping;
    [SerializeField] float _jumpForceRatio;
    private bool _hit = false;
    private Rigidbody _rb;
    private List<Rigidbody> _objectsOnSpring;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        ContactedGravityCount = 0;
        IsForce = true;
        _objectsOnSpring = new List<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (IsForce)
        {
            Vector3 forceAtPoint = ((_originTransform.position - transform.position) * _power) - (_damping * _rb.velocity);
            _rb.AddForce(forceAtPoint, ForceMode.Acceleration);
            if ((transform.position - _originTransform.position).magnitude <= 0.1f)
            {
                _hit = true;
            }
        }
        if (this.transform.localPosition.y >= _maxLocalY)
        {
            this.transform.localPosition = new Vector3(transform.localPosition.x, _maxLocalY, transform.localPosition.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("InteractiveObj"))
            _objectsOnSpring.Add(other.GetComponent<Rigidbody>());
        if (other.CompareTag("GravityZone"))
            ContactedGravityCount++;
        if (IsForce && JumpForce > 0.3f && !_hit)
        {
            if (Vector3.Angle(Vector3.up, transform.up.normalized) >= 30 && other.CompareTag("Player"))
            {
                other.GetComponent<PlayerMovementManager>().SpringXPlayer();
            }

            foreach (var item in _objectsOnSpring)
            {
                Vector3 dir = item.position - this.transform.position;
                Vector3 springDir = (_originTransform.position - this.transform.position);
                if (Vector3.Dot(this.transform.up, dir) <= 0 || Vector3.Dot(this.transform.up, springDir) <= 0)
                {
                    continue;
                }
                item.velocity = Vector3.zero;
                item.AddForce((JumpForce * _jumpForceRatio) * transform.up, ForceMode.VelocityChange);
                _hit = true;

                //Steam Stat
                if (item.gameObject.CompareTag("Player"))
                {
                    SteamIntegration.AddStat("stat_spring_uses", 1);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsForce && JumpForce > 0.3f && !_hit)
        {
            if (Vector3.Angle(Vector3.up, transform.up.normalized) >= 30 && other.CompareTag("Player"))
            {
                other.GetComponent<PlayerMovementManager>().SpringXPlayer();

                //Steam Stat
                SteamIntegration.AddStat("stat_spring_uses", 1);
            }

            foreach (var item in _objectsOnSpring)
            {
                Vector3 dir = item.position - this.transform.position;
                Vector3 springDir = (_originTransform.position - this.transform.position);
                if (Vector3.Dot(this.transform.up, dir) <= 0 || Vector3.Dot(this.transform.up, springDir) <= 0)
                {
                    continue;
                }
                item.velocity = Vector3.zero;
                item.AddForce((JumpForce * _jumpForceRatio) * transform.up, ForceMode.VelocityChange);
                _hit = true;

                //Steam Stat
                if (item.gameObject.CompareTag("Player"))
                {
                    SteamIntegration.AddStat("stat_spring_uses", 1);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("InteractiveObj"))
            _objectsOnSpring.Remove(other.GetComponent<Rigidbody>());
        if (other.CompareTag("GravityZone"))
            ContactedGravityCount--;
    }

    public float CalculateJumpForce()
    {
        _hit = false;
        return (_originTransform.position - this.transform.position).magnitude;
    }

    public int CalculateSpringInGravityZone()
    {
        return GameManager.Instance.playerMovementManager.gravityGun.gravityZones.FindAll(item => item.GetComponent<GravityZone>().hasSpring).Count;
    }
}
