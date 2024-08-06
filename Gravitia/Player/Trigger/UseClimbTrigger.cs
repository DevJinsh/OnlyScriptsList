using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseClimbTrigger : MonoBehaviour
{
    private PlayerMovementManager _playerMovementManager;
    private void Start()
    {
        _playerMovementManager = GetComponentInParent<PlayerMovementManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerMovementManager.useClimb = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _playerMovementManager.useClimb = false;
    }
}
