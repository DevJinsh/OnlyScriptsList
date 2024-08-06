using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLight : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GravityZone"))
        {
            //Steam Achievement
            SteamIntegration.UnlockAchievement("Ceiling_Lights");
        }
    }
}
