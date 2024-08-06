using System.Collections.Generic;
using UnityEngine;

public abstract class GravityZone : MonoBehaviour
{
    public float damping = 1f;
    public float gravityPower = 9.81f;

    protected List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    protected GravityGun _gravityGun;

    protected PlayerMovementManager _playerMovementManager;
    public bool hasSpring = false;
    private void OnEnable()
    {
        _gravityGun = FindAnyObjectByType<GravityGun>();
        _playerMovementManager = FindAnyObjectByType<PlayerMovementManager>();
        _gravityGun.gravityZones.Add(gameObject);
    }

    private void FixedUpdate()
    {
        if (_rigidbodies != null)
        {
            ApplyGravity();
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody item) && !item.CompareTag("Player") && !item.CompareTag("Projectile"))
        {
            if (other.gameObject.CompareTag("RemoveGravityZone"))
            {
                Destroy(gameObject);
                return;
            }
            _rigidbodies.Add(item);
            item.gameObject.GetComponent<Rigidbody>().useGravity = false;
            if (item.CompareTag("TurretBullet"))
            {
                item.GetComponent<RocketMachineBullet>().stop = true;
            }
            if (other.gameObject.TryGetComponent(out Spring spring))
            {
                hasSpring = true;
                spring.IsForce = false;
            }
        }
        else if (other.CompareTag("Player"))
        {
            EnterGravityZone(other, item);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rigidbody item))
        {
            _rigidbodies.Remove(item);
            item.gameObject.GetComponent<Rigidbody>().useGravity = true;
            if (other.gameObject.TryGetComponent(out NotUseGravity notUseGravity))
            {
                item.gameObject.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        if (other.CompareTag("Player"))
        {
            _playerMovementManager.isGravityArea = false;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_rigidbodies == null)
            return;

        foreach (var item in _rigidbodies)
        {
            if (item != null)
            {
                item.gameObject.GetComponent<Rigidbody>().useGravity = true;
                if (item.gameObject.TryGetComponent(out Spring spring))
                {
                    spring.ContactedGravityCount--;
                    if (spring.ContactedGravityCount == 0)
                    {
                        spring.JumpForce = spring.CalculateJumpForce();
                        spring.IsForce = true;
                    }
                }
                if (item.gameObject.TryGetComponent(out TagContact tagContact))
                {
                    tagContact.isContactedArray[1] = false;
                }
                if (item.gameObject.TryGetComponent(out NotUseGravity notUseGravity))
                {
                    item.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }
        _rigidbodies.Clear();

        if (_gravityGun.gravityZones.Contains(gameObject))
        {
            _gravityGun.gravityZones.Remove(gameObject);
        }
    }

    protected abstract void ApplyGravity();
    protected abstract void EnterGravityZone(Collider other, Rigidbody item);
}
