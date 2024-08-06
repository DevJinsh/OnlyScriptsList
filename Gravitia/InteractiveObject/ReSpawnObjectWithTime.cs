using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnObjectWithTime : ReSpawnObject
{
    public float time = 3f;
    public override void RespawnObject()
    {
        StartCoroutine(RespawnWithTime());
    }
    IEnumerator RespawnWithTime()
    {
        DestructibleObject obj = GetComponent<DestructibleObject>();
        obj.FadeOut(time);
        yield return new WaitForSeconds(time);
        transform.position = _respawnPos;
        obj.SetRespawnObject();
        obj.FadeIn(0f);
        if (obj.TryGetComponent<Durability>(out var durability))
        {
            durability.ResetDurability();
        }
    }
}
