using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCenterZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile"))
        {
            other.gameObject.GetComponent<Projectile>().canCreateGravityZone = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            other.gameObject.GetComponent<Projectile>().canCreateGravityZone = true;
        }
    }
}
