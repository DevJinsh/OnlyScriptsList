using System.Collections;
using UnityEngine;

public class Door : TriggerObject
{
    private Coroutine coroutine;
    public override void TriggerActive()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    public override void TriggerInactive(float resetTime)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(ResetDoor(resetTime));
    }

    IEnumerator ResetDoor(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }
}
