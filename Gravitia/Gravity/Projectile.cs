using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;
    private float _speed = 100f;
    private GravityGun _gravityGun;

    public bool canCreateGravityZone = true;
    public GameObject gravityPrefabs;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        Boundary.objects.Add(this.gameObject);
        _gravityGun = FindAnyObjectByType<GravityGun>();
    }
    void FixedUpdate()
    {
        _rb.AddRelativeForce(Vector3.forward * _speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(canCreateGravityZone)
        {
            SoundManager.instance.SoundPlay("gravityActive", this.gameObject);
            var item = Instantiate(gravityPrefabs);
            item.transform.position = collision.contacts[0].point;
            item.transform.localScale *= _gravityGun.gravityZoneScale;
            Destroy(item.gameObject, _gravityGun.durationTime);
        }
        Destroy(gameObject);
    }
}
