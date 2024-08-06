using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialTransform : MonoBehaviour
{
    Vector3 initialLocalPosition;
    Quaternion initialLocalRotation;

    private void Awake()
    {
        initialLocalPosition = this.transform.localPosition;
        initialLocalRotation = this.transform.localRotation;
    }

    public void ResetPosAndRotation()
    {
        this.transform.localPosition = initialLocalPosition;
        this.transform.localRotation = initialLocalRotation;
    }
}
