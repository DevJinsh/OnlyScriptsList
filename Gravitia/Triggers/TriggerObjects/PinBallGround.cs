using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinBallGround : TriggerObject
{
    public Animator animator;

    public override void TriggerActive()
    {
        ResetAllTrigger();
        animator.SetTrigger("LeftUpTrigger");
    }

    public override void TriggerInactive(float resetTime)
    {
        ResetAllTrigger();
        animator.SetTrigger("LeftDownTrigger");
    }

    void ResetAllTrigger()
    {
        animator.ResetTrigger("LeftDownTrigger");
        animator.ResetTrigger("LeftUpTrigger");
    }
}
