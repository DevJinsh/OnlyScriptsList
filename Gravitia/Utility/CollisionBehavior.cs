using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBehavior : MonoBehaviour
{
    public LayerMask toChangeLayer;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.layer = ReturnQuotient(toChangeLayer);
        }
    }

    int ReturnQuotient(int value)
    {
        int returnNum = 0;
        while (value / 2 != 0)
        {
            returnNum++;
            value = value / 2;
        }
        return returnNum;
    }
}
