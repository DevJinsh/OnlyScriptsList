using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreGravityBox : MonoBehaviour
{
    public LayerMask inBoxLayerMask;
    public LayerMask outerBoxLayerMask;
    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == outerBoxLayerMask)
        {
            other.gameObject.layer = PowersOfTwo(inBoxLayerMask.value);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (1 << other.gameObject.layer == inBoxLayerMask)
        {
            other.gameObject.layer = PowersOfTwo(outerBoxLayerMask.value);
        }
    }

    int PowersOfTwo(int num)
    {
        int value = 0;
        while (num / 2 != 0)
        {
            num = num / 2;
            value++;
        }
        return value;
    }
}
