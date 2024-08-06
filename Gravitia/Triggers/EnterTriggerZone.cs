using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTriggerZone : MonoBehaviour
{
    public GameObject triggerObject;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            TriggerObject trigger = triggerObject.GetComponentInChildren<TriggerObject>();
            if (trigger != null)
            {
                trigger.TriggerActive();
            }
            Destroy(this);
        }
    }
}
