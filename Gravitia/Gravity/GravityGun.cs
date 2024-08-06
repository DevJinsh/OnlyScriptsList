using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    public GameObject projectile;
    public List<GameObject> gravityZones;
    public float durationTime = 3f;
    public float gravityZoneScale = 1.5f;
    public float shootDelayTime = 0.5f;
    public float projectileDestroyTime = 1f;
    public Indicator indicator;
    [SerializeField] Transform GunTransform;
    private PlayerMovementManager _playerMovementManager;
    private PlayerIKSetting _playerIKSetting;

    private void Start()
    {
        _playerMovementManager = GetComponent<PlayerMovementManager>();
        _playerIKSetting = GetComponent<PlayerIKSetting>();
        gravityZones = new List<GameObject>();
    }
    void Update()
    {
        if (indicator.gameObject.activeSelf)
        {
            if (_playerMovementManager.Charging)
            {
                ChargingGun();
            }
            indicator.DrawShootLine(ProjectionToXY(GunTransform.position), _playerMovementManager.pointerPos);
            indicator.DrawGravityFieldOutLineWithShader(gravityZoneScale);
        }
        if (gravityZones.Count <= 0 && _playerMovementManager.isGravityArea)
        {
            _playerMovementManager.isGravityArea = false;
        }
    }

    Vector3 ProjectionToXY(Vector3 originPos)
    {
        return new Vector3(originPos.x, originPos.y, 0);
    }

    public void ChargingGun()
    {
        indicator.GravityOutLine.SetActive(true);
    }

    public Vector3 Debuging()
    {
        return indicator.hittedPos;
    }

    public void Shoot()
    {
        //Steam Stat
        SteamIntegration.AddStat("stat_gravity_bullets_fired", 1);

        var item = Instantiate(projectile);
        item.transform.position = new Vector3(GunTransform.position.x, GunTransform.position.y, 0);
        SoundManager.instance.SoundPlay("Shot", this.gameObject);
        GameManager.Instance.hapticManager.RumbleGamePad(0.18f, 0.3f);
        item.transform.LookAt(item.transform.position + (_playerMovementManager.pointerPos * indicator.ShotRange));
        Destroy(item.gameObject, projectileDestroyTime);
        indicator.GravityOutLine.SetActive(false);
        //StartCoroutine(_playerIKSetting.ShootRecoilAction(shootDelayTime));
        _playerIKSetting.StartShootingAnimation();

        return;
    }
}
