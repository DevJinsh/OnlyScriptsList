using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProhibitCoyote : MonoBehaviour
{
    PlayerMovementManager player;
    float originCoyoteTime;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovementManager>();
        originCoyoteTime = player.jumpCoyoteTime;
        StartCoroutine(StopCoyoteCoroutine());
    }


    IEnumerator StopCoyoteCoroutine()
    {
        player.jumpCoyoteTime = 0f;
        yield return new WaitForSeconds(1f);
        player.jumpCoyoteTime = originCoyoteTime;
    }
}
