using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftObject : MonoBehaviour
{
    public GameObject player;
    private float _previousPosX;
    private float _currentPosX;

    private void OnEnable()
    {
        _currentPosX = transform.position.x;
    }
    void Update()
    {
        _previousPosX = _currentPosX;
        _currentPosX = transform.position.x;
        if (player != null)
        {
            player.transform.position += Vector3.right * (_currentPosX - _previousPosX);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            player = collision.collider.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = null;
        }
    }
}
