using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleGravityZone : GravityZone
{
    protected override void EnterGravityZone(Collider other, Rigidbody item)
    {
        _rigidbodies.Add(item);
        item.gameObject.GetComponent<Rigidbody>().useGravity = false;

        _playerMovementManager.isGravityArea = true;
        GameManager.Instance.animationManager.EnterGravityZone();
    }
    protected override void ApplyGravity()
    {
        foreach (var item in _rigidbodies)
        {
            if (item != null)
            {
                item.GetComponent<Rigidbody>().useGravity = false;
                Vector3 forceAtPoint = ((transform.position - item.transform.position) * gravityPower) - ((damping) * item.velocity);
                item.AddForce(forceAtPoint, ForceMode.Acceleration);
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }

    protected override void OnDestroy()
    {
        foreach (var item in _rigidbodies)
        {
            if (item != null)
            {
                item.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        _rigidbodies.Clear();
    }
}
