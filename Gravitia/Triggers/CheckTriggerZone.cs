using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTriggerZone : MonoBehaviour
{
    public LayerMask layerMask;
    public TriggerObject triggerObject;
    [SerializeField] float _coolTime;
    int _triggerCount;
    Coroutine _spawnCoroutine;

    private void Start()
    {
        _triggerCount = 0;
    }

    private void Update()
    {
        if (ActivationCondition())
        {
            Active();
        }
        else if (_triggerCount != 0)
        {
            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
                _spawnCoroutine = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == layerMask)
        {
            _triggerCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (1 << other.gameObject.layer == layerMask)
        {
            _triggerCount--;
        }
    }

    bool ActivationCondition()
    {
        if (_triggerCount == 0)
        {
            return true;
        }
        return false;
    }

    public void Active()
    {
        if (_spawnCoroutine == null)
        {
            _spawnCoroutine = StartCoroutine(CheckCoolTime());
        }
    }
    IEnumerator CheckCoolTime()
    {
        yield return new WaitForSeconds(_coolTime);
        triggerObject.TriggerActive();
        _spawnCoroutine = null;
    }
}
