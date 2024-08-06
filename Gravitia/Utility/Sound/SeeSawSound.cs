using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeSawSound : MonoBehaviour
{
    float currentRotate;
    float changedRotate;

    void Update()
    {
        currentRotate = transform.rotation.z;
        changedRotate = Mathf.Floor(currentRotate * 10f) / 10f;

        if (changedRotate == 0)
        {
            AkSoundEngine.PostEvent("MetalSwing", gameObject);
        }
        
    }
}
