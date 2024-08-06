using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringSound : MonoBehaviour
{
   /* private void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Player"))
        {
            AkSoundEngine.PostEvent("SpringUp", gameObject);
        }
    }*/
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            
            AkSoundEngine.PostEvent("SpringUp", gameObject);
        }
    }
}
