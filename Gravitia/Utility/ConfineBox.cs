using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfineBox : MonoBehaviour
{
    public float sizeX;
    public float sizeY;
    public RocketMachine[] turret;
    private BoxCollider boxCollider;
    

    private void Awake()
    {
        TryGetComponent<BoxCollider>(out BoxCollider collider);
        boxCollider = collider;
        if (boxCollider != null)
        {
            boxCollider.size = new Vector3(sizeX, sizeY, 1);
            boxCollider.isTrigger = true;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = this.transform.position;
        Vector3 size = new Vector3(sizeX, sizeY, 0);
        Gizmos.DrawWireCube(center, size);
    }

    public bool isInsideOfConfineBox(Vector3 pos)
    {
        Vector3 center = this.transform.position;
        if ((center.x - sizeX/2f <= pos.x && pos.x <= center.x + sizeX/2f) 
                && center.y - sizeY/2f <= pos.y && pos.y <= center.y + sizeY/2f)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (turret != null && other.CompareTag("Player"))
        {
            foreach (RocketMachine rocket in turret)
            {
                rocket.enabled = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (turret != null && other.CompareTag("Player"))
        {
            foreach (RocketMachine rocket in turret)
            {
                rocket.enabled = false;
            }
        }
    }
}
