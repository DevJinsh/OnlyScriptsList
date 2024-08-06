using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanClimbTrigger : MonoBehaviour
{
    private PlayerMovementManager _playerMovementManager;
    private void Start()
    {
        _playerMovementManager = GetComponentInParent<PlayerMovementManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _playerMovementManager.canClimb = false;
    }

    private void OnTriggerExit(Collider other)
    {
        _playerMovementManager.canClimb = true;
    }
}
