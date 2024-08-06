using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotRespawnObject : ReSpawnObject
{
    public float time = 3f;
    public override void RespawnObject()
    {
        StartCoroutine(DestroyAfterTime());
    }
    IEnumerator DestroyAfterTime()
    {
        DestructibleObject obj = GetComponent<DestructibleObject>();
        obj.FadeOut(time);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
