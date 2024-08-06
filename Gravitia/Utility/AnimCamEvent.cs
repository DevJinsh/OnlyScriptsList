using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimCamEvent : MonoBehaviour
{
    public UnityEvent myEvent;

    public void Start()
    {
        if (myEvent == null)
        {
            myEvent = new UnityEvent();
        }
        myEvent.Invoke();
    }

    public void Update()
    {
        
    }
}
