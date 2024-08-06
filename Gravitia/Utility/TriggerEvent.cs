using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent myEvent;
    public LayerMask triggerLayer;
    private void Awake()
    {
        if (myEvent == null)
        {
            myEvent = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerLayer == 1 << other.gameObject.layer)
        {
            myEvent.Invoke();
        }
    }

}
