using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronBarSteamBox : MonoBehaviour
{
    public GameObject ironBar;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ironBar)
        {
            //Steam Achievement
            SteamIntegration.UnlockAchievement("Stick_On_Hinge");
        }
    }
}
