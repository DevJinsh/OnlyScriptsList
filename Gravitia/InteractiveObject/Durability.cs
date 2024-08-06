using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durability : MonoBehaviour
{
    public int durabilityCount = 3;
    private int initialValue;

    public bool IsBreakable { get; set; } = false;
    public ConfineBox confineBox;
    public int DurabilityCount
    {
        get { return durabilityCount; }
        set 
        {
            durabilityCount = value;
            if (durabilityCount <= 0)
            {
                durabilityCount = 0;
                IsBreakable = true;
            }
        }
    }
    private void Awake()
    {
        initialValue = durabilityCount;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GravityZone") && !confineBox.isInsideOfConfineBox(this.transform.position))
        {
            DurabilityCount--;
            if (TryGetComponent<TreatmentDestruction>(out var treatmentDestruction))
            {
                treatmentDestruction.TreatDestruction(true);
                treatmentDestruction.CrumbleObject();
            }
            if (IsBreakable)
            {
                if (TryGetComponent<ReSpawnObject>(out var reSpawnObject))
                {
                    reSpawnObject.RespawnObject();
                }
                if (TryGetComponent<DestructibleObject>(out var destructibleObject))
                {
                    destructibleObject.SetDestroyedObject();
                }
            }
        }
    }

    public void ResetDurability()
    {
        DurabilityCount = initialValue;
        IsBreakable = false;
    }
}
