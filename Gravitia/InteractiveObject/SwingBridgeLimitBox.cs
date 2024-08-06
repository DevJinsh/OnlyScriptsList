using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingBridgeLimitBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
