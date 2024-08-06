using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnObject : MonoBehaviour
{
    protected Vector3 _respawnPos;
    void Awake()
    {
        _respawnPos = transform.position;
    }

    public virtual void RespawnObject()
    {
        transform.position = _respawnPos;
    }
}
