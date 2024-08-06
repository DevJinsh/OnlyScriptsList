using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour
{
    //List<Rigidbody> collisionList;
    //Rigidbody _rb;
    //public float minZAngle;
    //public float maxZAngle;
    //public float speed;
    //Transform _parentTransform;
    //float t;

    //private void Awake()
    //{
    //    collisionList = new List<Rigidbody>();
    //    _parentTransform = transform.parent;
    //    _rb = _parentTransform.gameObject.GetComponent<Rigidbody>();
    //    t = 0.5f;
    //}
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementManager>().isSeesaw = true;
        }
        //if (!collisionList.Equals(other.GetComponent<Rigidbody>()))
        //{
        //    collisionList.Add(other.GetComponent<Rigidbody>());
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementManager>().isSeesaw = false;
        }
        //collisionList.Remove(other.GetComponent<Rigidbody>());
    }
    //private void Update()
    //{
    //    CalculateMass();
    //}
    //private void CalculateMass()
    //{
    //    float sum = 0;
    //    foreach(Rigidbody rb in collisionList)
    //    {
    //        if (rb == null)
    //            continue;
    //        float diffX = _rb.worldCenterOfMass.x - rb.worldCenterOfMass.x;
    //        sum += (diffX * rb.mass);
    //    }
    //    if (sum == 0)
    //    {
    //        return;
    //    }
    //    t += -Mathf.Sign(sum) * 0.01f * speed;
    //    Debug.Log("sum: " + sum + " t: " + t);
    //    t = Mathf.Clamp(t, 0f, 1f);
    //    _parentTransform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(minZAngle, maxZAngle, t));
    //}
}
