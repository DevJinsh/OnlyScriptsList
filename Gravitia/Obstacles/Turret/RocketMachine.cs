using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMachine : MonoBehaviour
{
    [SerializeField] private GameObject _launchPoint;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Vector3 _dir;
    [SerializeField] private float _creationTime = 1f;
    [SerializeField] private float _destroyBulletTime = 3f;
    private float _time = 0.0f;

    private void Update()
    {
        if (_time >= _creationTime)
        {
            _time = 0.0f;
            CreateBullet();
        }
        else
        {
            _time += Time.deltaTime;
        }
    }

    void CreateBullet()
    {
        var bullet = Instantiate(_bullet, _launchPoint.transform.position, Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(_dir.y, _dir.x) - 90f));
        SoundManager.instance.SoundPlay("Bullet", this.gameObject);
        Destroy(bullet, _destroyBulletTime);
    }
}
