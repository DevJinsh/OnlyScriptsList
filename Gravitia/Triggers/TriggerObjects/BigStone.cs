using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigStone : TriggerObject
{
    public override void TriggerActive()
    {
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    public override void TriggerInactive(float resetTime)
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("InteractiveObj"))
        {
            this.GetComponent<Rigidbody>().velocity *= 0.8f;
        }
    }
}
