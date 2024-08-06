using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGravityZone : GravityZone
{
    protected override void EnterGravityZone(Collider other, Rigidbody item)
    {
        _rigidbodies.Add(item);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;
        item.velocity = (other.transform.position - transform.position).normalized * item.velocity.magnitude;
        _playerMovementManager.isGravityArea = true;
        GameManager.Instance.animationManager.EnterGravityZone();
    }
    protected override void ApplyGravity()
    {
        if (_rigidbodies != null)
        {
            foreach (var item in _rigidbodies)
            {
                if (item != null)
                {
                    item.GetComponent<Rigidbody>().useGravity = false;
                    Vector3 forceAtPoint = ((item.transform.position - transform.position) * gravityPower) - (damping * item.velocity);
                    item.AddForce(forceAtPoint, ForceMode.Acceleration);
                }
            }
        }
    }
}
