using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerIKSetting : MonoBehaviour
{
    public Transform Target;
    public float maxAngle = 30f;
    public float rotationSpeed = 0.01f;
    private float pos_z;
    [SerializeField] private Animator _recoilAnimator;
    [SerializeField] private Transform _recoilForearn;
    [SerializeField] private Transform _recoilArm;
    [SerializeField] private Transform _recoilBody;
    [SerializeField] private float _recoilSpeed = 10f;
    [SerializeField] private float _recoilAngle = -50f;
    [SerializeField] private MultiRotationConstraint _armStretchAim;
    private Quaternion origin = Quaternion.Euler(0, 0, 0);
    private Quaternion rightAngle = Quaternion.Euler(0, 90f, 0);
    private void Awake()
    {
        pos_z = Target.transform.position.z;
    }
    void Update()
    {
        Vector3 pos = transform.position + (GetComponent<PlayerMovementManager>().pointerPos * 100);

        pos.z = pos_z;
        Target.transform.position = pos;
        float rate = CalculateRotationRate();
        //Target.localRotation = Quaternion.Euler(0, rate * -90f, 0);
        //_armStretchAim.weight = rate * 0.5f;
        //Target.transform.localRotation = Quaternion.Slerp(Target.transform.localRotation, Quaternion.Euler(0, -GetRotationValue(), 0), rotationSpeed);
    }

    float CalculateRotationRate()
    {
        Vector3 dir = Target.position - transform.position;
        dir.z = 0;
        float value = Vector3.Dot(Vector3.up, dir.normalized);
        if (value < 0.5f)
        {
            return 0f;
        }
        value -= 0.5f;
        value *= 2f;
        return value * value;
    }

    float GetAngle(Vector2 start, Vector2 end)
    {
        Vector2 v2 = end - start;
        return Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
    }

    public float GetRotationValue()
    {
        Vector3 characterMoveDir = transform.forward;
        Vector3 mouseDir = (Target.transform.position - this.transform.position).normalized;
        float angle = Vector3.Dot(characterMoveDir, mouseDir);
        if (angle < 0.1f)
        {
            //angle = Mathf.Sign(Vector3.Dot(transform.right, Vector3.right)) * maxAngle * angle;
            angle = 180f;
        }
        else
        {
            angle = 0f;
        }
        return angle;
    }

    private void Shooting()
    {
        _recoilForearn.localRotation = Quaternion.RotateTowards(_recoilForearn.localRotation, Quaternion.Euler(0, _recoilAngle, 0), _recoilSpeed * Time.deltaTime);
        _recoilArm.localRotation = Quaternion.RotateTowards(_recoilArm.localRotation, Quaternion.Euler(0, -10f, 0), _recoilSpeed * Time.deltaTime);
        _recoilBody.localRotation = Quaternion.RotateTowards(_recoilBody.localRotation, Quaternion.Euler(0, 20f, -11f), _recoilSpeed * Time.deltaTime);
    }

    private void ExitShooting()
    {
        _recoilForearn.localRotation = Quaternion.RotateTowards(_recoilForearn.localRotation, Quaternion.Euler(0, 0, 0), _recoilSpeed * Time.deltaTime);
        _recoilArm.localRotation = Quaternion.RotateTowards(_recoilArm.localRotation, Quaternion.Euler(0, 0, 0), _recoilSpeed * Time.deltaTime);
        _recoilBody.localRotation = Quaternion.RotateTowards(_recoilBody.localRotation, Quaternion.Euler(0, 0, 0), _recoilSpeed * Time.deltaTime);
    }

    public IEnumerator ShootRecoilAction(float delayTime)
    {
        float halfTime = delayTime / 2f;
        while (delayTime > halfTime)
        {
            delayTime -= Time.deltaTime;
            Shooting();
            yield return null;
        }
        while (delayTime > 0.0f)
        {
            delayTime -= Time.deltaTime;
            ExitShooting();
            yield return null;
        }
    }

    public void StartShootingAnimation()
    {
        _recoilAnimator.SetTrigger("Shoot");
    }

}
