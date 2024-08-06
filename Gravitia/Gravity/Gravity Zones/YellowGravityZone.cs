using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowGravityZone : GravityZone
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
        ///Todo 무중력?
    }
}
