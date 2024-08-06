using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLamp : MonoBehaviour
{
    float time = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time = 0f;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time += Time.deltaTime;
            if (time >= 10f)
            {
                //Steam Achievement
                SteamIntegration.UnlockAchievement("Stand_Under_A_Streetlight_For_10_Seconds");
                Destroy(this);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time = 0f;
        }
    }
}
