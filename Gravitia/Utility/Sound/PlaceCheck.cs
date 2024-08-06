using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCheck : MonoBehaviour
{
    bool isInDoor = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            isInDoor = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isInDoor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            isInDoor = false;

        
    }

    private void Update()
    {
        if(isInDoor)
        {
            AkSoundEngine.SetState("Place", "InDoor");
        }
        else
            AkSoundEngine.SetState("Place", "OutDoor");

    }

    
}
