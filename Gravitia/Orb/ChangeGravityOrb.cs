using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravityOrb : MonoBehaviour
{
    public GameObject changeGravityPrefabs;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer.Equals(LayerMask.NameToLayer("Projectile")))
        {
            other.GetComponent<Projectile>().gravityPrefabs = changeGravityPrefabs;
            Destroy(gameObject);
        }
    }
}
