using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollOnOff : MonoBehaviour
{
    public Collider mainCollider;
    public GameObject ThisGuysRig;
    public Animator ThisGuysAnimator;

    private void Start()
    {
        GetRagdollBits();
        RagdollModeOff();
    }

    Collider[] ragDollColliders;
    Rigidbody[] limbsRigidbodies;

    void GetRagdollBits()
    {
        ragDollColliders = ThisGuysRig.GetComponentsInChildren<Collider>();
        limbsRigidbodies = ThisGuysRig.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollModeOn()
    {
        foreach (Collider col in ragDollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = false;
            rigid.gameObject.layer = ReturnQuotient(LayerMask.GetMask("IgnoreGravityZone"));
        }
        ThisGuysAnimator.enabled = false;
        mainCollider.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void RagdollModeOff()
    {
        foreach (Collider col in ragDollColliders)
        {
            col.enabled = false;
        }
        foreach (Rigidbody rigid in limbsRigidbodies)
        {
            rigid.isKinematic = true;
        }
        ThisGuysAnimator.enabled = true;
        mainCollider.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
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
