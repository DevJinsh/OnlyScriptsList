using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform playerTransform;
    private void Awake()
    {
        playerTransform = FindAnyObjectByType<PlayerMovementManager>().transform;
    }
    private void Update()
    {
        GrapDirection();
    }
    void GrapDirection()
    {
        Vector3 dir = playerTransform.position - this.transform.position;
        dir.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<RagDollOnOff>().RagdollModeOn();
        }
    }
}
