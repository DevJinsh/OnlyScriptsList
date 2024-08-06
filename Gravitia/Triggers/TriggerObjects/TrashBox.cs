using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBox : TriggerObject
{
    [SerializeField] GameObject _trashBag;
    public Transform parentTransform;
    public ConfineBox confineBox;
    public override void TriggerActive()
    {
        GameObject obj = Instantiate(_trashBag, this.transform.position, this.transform.rotation);
        obj.transform.parent = parentTransform;
        foreach (var durability in obj.transform.GetComponentsInChildren<Durability>())
        {
            durability.confineBox = confineBox;
        }
    }

    public override void TriggerInactive(float resetTime)
    {
        throw new System.NotImplementedException();
    }
}
