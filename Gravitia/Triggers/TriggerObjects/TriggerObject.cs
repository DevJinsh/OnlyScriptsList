using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerObject : MonoBehaviour
{
    public abstract void TriggerActive();
    public abstract void TriggerInactive(float resetTime);
}
